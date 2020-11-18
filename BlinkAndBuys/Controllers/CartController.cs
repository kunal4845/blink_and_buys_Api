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
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        public CartController(ILogger<ProductController> logger, IProductRepository productRepository, IMapper mapper)
        {
            _logger = logger;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        #endregion

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddToCart(UserCartModel userCart)
        {
            try
            {
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
        [Route("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            {
                List<UserCartModel> userCarts = new List<UserCartModel>();

                var response = await _productRepository.GetCart(userId);
                foreach (var item in response)
                {
                    var products = await _productRepository.GetProductsAsync(item.ProductId);
                    var productModels = _mapper.Map<List<Product>, List<ProductModel>>(products);

                    if (productModels.Count > 0)
                    {
                        UserCartModel userCart = new UserCartModel()
                        {
                            Product = productModels[0],
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UserId = item.UserId,
                            CreatedDt = item.CreatedDt,
                            Type = item.Type
                        };
                        userCarts.Add(userCart);
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
        [Route("billingAddress/{userId}")]
        public async Task<IActionResult> GetBillingAddress(int userId)
        {
            try
            {
                var response = await _productRepository.GetBillingAddress(userId);
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
