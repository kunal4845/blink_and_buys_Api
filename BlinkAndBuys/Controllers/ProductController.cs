using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


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

        [HttpGet]
        [Route("category/{id?}")]
        public async Task<IActionResult> GetProductCategory(int? id)
        {
            try
            {
                var categories = await _productRepository.GetProductCategory(id);
                return Ok(_mapper.Map<List<ProductCategory>, List<ProductCategoryModel>>(categories));
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Upsert(ProductModel productModel)
        {
            try
            {
                var product = _mapper.Map<ProductModel, Product>(productModel);
                int id = await _productRepository.Upsert(product);

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
                var files = Request.Form.Files;
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                _logger.LogInformation("Number of files are: {0}", files.Count);

                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }
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
                        ImagePath = Convert.ToBase64String(image),
                        IsPrimaryImage = fileName == masterImage ? true : false,
                        ProductId = productId,
                        IsDeleted = false,
                        CreatedDt = DateTime.Now,
                        CreatedBy = 1
                    };
                    product.ProductImages.Add(productImage);
                }

                var productImages = _mapper.Map<List<ProductImageModel>, List<ProductImage>>(product.ProductImages);
                var id = await _productRepository.UploadProductImage(productImages, productId);
                _logger.LogInformation("Images saved to db and id returned:{0}", id);

                return Ok(id);
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
                    _logger.LogInformation("Fetching product images for product id: {0}", product.Id);
                    var images = _productRepository.GetProductImages(product.Id);
                    var productImages = _mapper.Map<List<ProductImage>, List<ProductImageModel>>(images);
                    product.ProductImages = productImages;
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
                var product = await _productRepository.GetProductsAsync(productId);
                if (product.Count > 0)
                {
                    product[0].IsVerified = true;
                    product[0].ModifiedBy = 1;
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
                var product = await _productRepository.GetProductsAsync(productId);
                if (product.Count > 0)
                {
                    product[0].IsDeleted = true;
                    product[0].ModifiedBy = 1;
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
        [Route("list/recommended")]
        public async Task<IActionResult> GetRecommendedProducts()
        {
            try
            {
                var products = await _productRepository.GetRecommendedProducts();
                var response = _mapper.Map<List<Product>, List<ProductModel>>(products);

                foreach (var product in response)
                {
                    _logger.LogInformation("Fetching product images for product id: {0}", product.Id);
                    var images = _productRepository.GetProductImages(product.Id);
                    var productImages = _mapper.Map<List<ProductImage>, List<ProductImageModel>>(images);
                    product.ProductImages = productImages;
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
