using System;
using System.Collections.Generic;
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
    public class BookedProductController : ControllerBase
    {
        #region Initiate
        private IBookedProductRepository _bookedProductRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<BookedProductController> _logger;
        private IPaymentRepository _paymentRepository;
        private IServiceRepository _serviceRepository;
        public BookedProductController(
            ILogger<BookedProductController> logger,
            IProductRepository productRepository,
            IBookedProductRepository bookedProductRepository,
            IMapper mapper,
            IUserRepository userService,
            IPaymentRepository paymentRepository,
            IServiceRepository serviceRepository
        )
        {
            _logger = logger;
            _bookedProductRepository = bookedProductRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _userService = userService;
            _paymentRepository = paymentRepository;
            _serviceRepository = serviceRepository;
        }
        #endregion

        [HttpGet]
        [Route("list/{id?}")]
        public async Task<IActionResult> GetActionAsync(int? id)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var bookedProducts = await _bookedProductRepository.GetBookedProductAsync(id, Convert.ToInt32(loggedInUser));
                var bookedProductModels = _mapper.Map<List<BookedProduct>, List<BookedProductModel>>(bookedProducts);
                foreach (var item in bookedProductModels)
                {
                    var products = await _productRepository.GetProductsAsync(item.ProductId);
                    var payment = await _paymentRepository.Get(item.PaymentId);
                    var address = await _productRepository.GetBillingAddress(item.UserId);
                    var user = await _userService.GetUserById(item.UserId);

                    item.Product = products.Count > 0 ? _mapper.Map<Product, ProductModel>(products[0]) : new ProductModel();

                    item.Payment = _mapper.Map<Payment, PaymentModel>(payment);
                    item.BillingAddress = _mapper.Map<BillingAddress, BillingAddressModel>(address);
                    item.User = _mapper.Map<Account, AccountModel>(user);
                };

                return Ok(bookedProductModels);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("service/{id?}")]
        public async Task<IActionResult> GetServicesActionAsync(int? id)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var bookedServices = await _bookedProductRepository.GetBookedServicesAsync(id, Convert.ToInt32(loggedInUser));
                var bookedServiceModels = _mapper.Map<List<BookedService>, List<BookedServiceModel>>(bookedServices);
                foreach (var item in bookedServiceModels)
                {
                    var services = await _serviceRepository.GetServices(item.ServiceId);
                    var payment = await _paymentRepository.Get(item.PaymentId);
                    var address = await _productRepository.GetBillingAddress(item.UserId);
                    var user = await _userService.GetUserById(item.UserId);

                    item.Service = services.Count > 0 ? _mapper.Map<Service, ServiceModel>(services[0]) : new ServiceModel();

                    item.Payment = _mapper.Map<Payment, PaymentModel>(payment);
                    item.BillingAddress = _mapper.Map<BillingAddress, BillingAddressModel>(address);
                    item.User = _mapper.Map<Account, AccountModel>(user);
                };

                return Ok(bookedServiceModels);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("cancel/{bookedProductId}")]
        public async Task<IActionResult> CancelOrder(int bookedProductId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _bookedProductRepository.CancelOrder(bookedProductId, Convert.ToInt32(loggedInUser));
                if (res)
                {
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cancelService/{bookedServiceId}")]
        public async Task<IActionResult> CancelService(int bookedServiceId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _bookedProductRepository.CancelService(bookedServiceId, Convert.ToInt32(loggedInUser));
                if (res)
                {
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }
    }
}
