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
    public class CartController : ControllerBase
    {
        #region Initiate
        private IProductRepository _productRepository;
        private IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        public CartController(ILogger<ProductController> logger, IProductRepository productRepository, IMapper mapper, IServiceRepository serviceRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _mapper = mapper;
            _serviceRepository = serviceRepository;
        }
        #endregion

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddToCart(UserCartModel userCart)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                userCart.UserId = Convert.ToInt32(loggedInUser);

                var response = await _productRepository.AddToCart(_mapper.Map<UserCartModel, UserCart>(userCart));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];

                List<UserCartModel> userCarts = new List<UserCartModel>();
                UserCartModel userCart = new UserCartModel();

                var response = await _productRepository.GetCart(Convert.ToInt32(loggedInUser));
                foreach (var item in response)
                {
                    if (item.Type == "service")
                    {
                        var services = await _serviceRepository.GetServices(item.BookedItemId);
                        var serviceModels = _mapper.Map<List<Service>, List<ServiceModel>>(services);
                        userCart = new UserCartModel()
                        {
                            Service = serviceModels[0],
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            BookedItemId = item.BookedItemId,
                            Quantity = item.Quantity,
                            UserId = item.UserId,
                            CreatedDt = item.CreatedDt,
                            Type = item.Type
                        };
                        userCarts.Add(userCart);
                    }
                    else if (item.Type == "product")
                    {
                        var products = await _productRepository.GetProductsAsync(item.BookedItemId);
                        var productModels = _mapper.Map<List<Product>, List<ProductModel>>(products);

                        if (productModels.Count > 0)
                        {
                            userCart = new UserCartModel()
                            {
                                Product = productModels[0],
                                Id = item.Id,
                                IsDeleted = item.IsDeleted,
                                BookedItemId = item.BookedItemId,
                                Quantity = item.Quantity,
                                UserId = item.UserId,
                                CreatedDt = item.CreatedDt,
                                Type = item.Type
                            };
                            userCarts.Add(userCart);
                        }
                    }
                }
                return Ok(userCarts);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            try
            {
                var response = await _productRepository.DeleteCart(cartId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout(List<UserCartModel> userCart)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                //userCart.UserId = Convert.ToInt32(loggedInUser);
                var response = await _productRepository.Checkout(_mapper.Map<List<UserCartModel>, List<UserCart>>(userCart));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("billingAddress")]
        public async Task<IActionResult> BillingAddress(BillingAddressModel billingAddress)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var response = await _productRepository.BillingAddress(_mapper.Map<BillingAddressModel, BillingAddress>(billingAddress), Convert.ToInt32(loggedInUser));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("billingAddress")]
        public async Task<IActionResult> GetBillingAddress()
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var response = await _productRepository.GetBillingAddress(Convert.ToInt32(loggedInUser));
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
