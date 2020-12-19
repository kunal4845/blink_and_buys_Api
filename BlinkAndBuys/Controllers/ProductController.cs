using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace BlinkAndBuys.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Initiate
        private IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository, IMapper mapper)
        {
            _logger = logger;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        #endregion

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Upsert(ProductModel productModel)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var product = _mapper.Map<ProductModel, Product>(productModel);
                _logger.LogInformation("Delete existing images");
                await _productRepository.DeleteProductImage(productModel.Id);

                var productImage = new ProductImageModel();
                List<ProductImageModel> productImages = new List<ProductImageModel>();

                _logger.LogInformation("Add new images");
                foreach (var item in productModel.ProductImages)
                {
                    if (item.ImagePath != "")
                    {
                        productImage = new ProductImageModel()
                        {
                            Name = item.Name,
                            ImagePath = item.ImagePath,
                            IsPrimaryImage = item.IsPrimaryImage,
                            ProductId = productModel.Id,
                            IsDeleted = false,
                            CreatedDt = DateTime.Now,
                            CreatedBy = Convert.ToInt32(loggedInUser)
                        };
                        productImages.Add(productImage);
                    }
                };
                if (productImages.Count > 0)
                {
                    var mapProductImage = _mapper.Map<List<ProductImageModel>, List<ProductImage>>(productImages);
                    await _productRepository.UploadProductImage(mapProductImage, productModel.Id);
                }
                int id = await _productRepository.Upsert(product, Convert.ToInt32(loggedInUser));
                return Ok(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload/{productId}/{masterImage}")]
        public async Task<IActionResult> UploadProductImage(int productId, string masterImage)
        {
            ProductModel product = new ProductModel();
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var files = Request.Form.Files;
                var folderName = Path.Combine("Resources", "ProductImages");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var productImage = new ProductImageModel();

                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    _logger.LogInformation("Convert images to base64.");
                    byte[] image = System.IO.File.ReadAllBytes(pathToSave + "\\" + fileName);

                    productImage = new ProductImageModel()
                    {
                        Name = fileName,
                        ImagePath = "data:image/jpg;base64," + Convert.ToBase64String(image),
                        IsPrimaryImage = fileName == masterImage ? true : false,
                        ProductId = productId,
                        IsDeleted = false,
                        CreatedDt = DateTime.Now,
                        CreatedBy = Convert.ToInt32(loggedInUser)
                    };
                    product.ProductImages.Add(productImage);
                }
                if (files.Count > 0)
                {
                    var productImages = _mapper.Map<List<ProductImageModel>, List<ProductImage>>(product.ProductImages);
                    var id = await _productRepository.UploadProductImage(productImages, productId);
                    _logger.LogInformation("Images saved to db and id returned:{0}", id);
                }
                return Ok(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("list/{id?}")]
        public async Task<IActionResult> GetProductListAsync(int? id)
        {
            try
            {
                var products = await _productRepository.GetProductsAsync(id);
                var response = _mapper.Map<List<Product>, List<ProductModel>>(products);

                foreach (var product in response)
                {
                    var images = await _productRepository.GetProductImages(product.Id);
                    product.ProductImages = _mapper.Map<List<ProductImage>, List<ProductImageModel>>(images);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("dashboard/list/{id?}")]
        public async Task<IActionResult> GetDashboardProducts(int? id)
        {
            try
            {
                var products = await _productRepository.GetProductsAsync(id);
                var filterProducts = products.FindAll(x => !x.IsDeleted && x.IsActive && x.IsVerified);

                var response = _mapper.Map<List<Product>, List<ProductModel>>(filterProducts);

                foreach (var product in response)
                {
                    var images = await _productRepository.GetProductImages(product.Id);
                    product.MasterImage = images.Find(x => x.IsPrimaryImage && !x.IsDeleted).ImagePath;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("verify/{productId}")]
        public async Task<IActionResult> VerifyProduct(int productId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];

                var product = await _productRepository.GetProductsAsync(productId);
                if (product.Count > 0)
                {
                    product[0].IsVerified = true;
                    product[0].ModifiedBy = Convert.ToInt32(loggedInUser);
                    product[0].ModifiedDt = DateTime.Now;

                    var id = await _productRepository.VerifyProduct(product[0]);
                    return Ok(product[0]);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("delete/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var product = await _productRepository.GetProductsAsync(productId);
                if (product.Count > 0)
                {
                    product[0].IsDeleted = true;
                    product[0].ModifiedBy = Convert.ToInt32(loggedInUser);
                    product[0].ModifiedDt = DateTime.Now;

                    var id = await _productRepository.Delete(product[0]);
                    return Ok(product[0]);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("block/{productId}")]
        public async Task<IActionResult> BlockProduct(int productId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];

                var product = await _productRepository.GetProductsAsync(productId);
                if (product.Count > 0)
                {
                    product[0].IsActive = false;
                    product[0].ModifiedBy = Convert.ToInt32(loggedInUser);
                    product[0].ModifiedDt = DateTime.Now;

                    var id = await _productRepository.BlockProduct(product[0]);
                    return Ok(product[0]);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("list/recommended")]
        public async Task<IActionResult> GetRecommendedProducts()
        {
            try
            {
                //var products = await _productRepository.GetRecommendedProducts();
                //var response = _mapper.Map<List<Product>, List<ProductModel>>(products);

                //foreach (var product in response)
                //{
                //    _logger.LogInformation("Fetching product images for product id: {0}", product.Id);
                //    var images = await _productRepository.GetProductImages(product.Id);
                //    var productImages = _mapper.Map<List<ProductImage>, List<ProductImageModel>>(images);
                //    product.ProductImages = productImages;
                //}

                //return Ok(response);


                var products = await _productRepository.GetRecommendedProducts();
                var filterProducts = products.FindAll(x => !x.IsDeleted && x.IsActive && x.IsVerified);

                var response = _mapper.Map<List<Product>, List<ProductModel>>(filterProducts);

                foreach (var product in response)
                {
                    var images = await _productRepository.GetProductImages(product.Id);
                    product.MasterImage = images.Find(x => x.IsPrimaryImage && !x.IsDeleted).ImagePath;
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }
    }
}
