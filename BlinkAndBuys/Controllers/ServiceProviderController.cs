using System;
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
    public class ServiceProviderController : ControllerBase
    {
        #region Initiate
        private IServiceProviderRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceController> _logger;

        public ServiceProviderController(IServiceProviderRepository serviceRepository, IMapper mapper, ILogger<ServiceController> logger)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        [HttpPost]
        [Route("assignServiceProvider")]
        public async Task<IActionResult> AssignServiceProvider(BookedServiceModel bookedService)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = await _serviceRepository.AssignServiceProvider(bookedService.ServiceProviderId, bookedService.BookedServiceId, Convert.ToInt32(loggedInUser));
                return Ok(service);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("availability/{serviceProviderId}")]
        public async Task<IActionResult> GetServiceProviderAvailability(int serviceProviderId)
        {
            try
            {
                var serviceProviderAvailability = await _serviceRepository.GetserviceProviderAvailability(serviceProviderId);
                return Ok(_mapper.Map<ServiceProviderAvailability, ServiceProviderAvailabilityModel>(serviceProviderAvailability));
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("availability")]
        public async Task<IActionResult> SetServiceProviderAvailability(ServiceProviderAvailabilityModel serviceProviderAvailability)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = _mapper.Map<ServiceProviderAvailabilityModel, ServiceProviderAvailability>(serviceProviderAvailability);
                var res = await _serviceRepository.SetServiceProviderAvailability(service, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("verify/{userId}")]
        public async Task<IActionResult> VerifyServiceProvider(int userId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _serviceRepository.VerifyServiceProvider(userId, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }

        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteServiceProvider(int userId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _serviceRepository.DeleteServiceProvider(userId, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("block/{userId}")]
        public async Task<IActionResult> BlockServiceProvider(int userId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _serviceRepository.BlockServiceProvider(userId, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }

        }


        [HttpPost]
        [Route("updateStatus/{selectedPaymentStatus}/{paymentId}")]
        public async Task<IActionResult> UpdateStatus([FromBody] BookedServiceModel bookedService, string selectedPaymentStatus, int paymentId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = await _serviceRepository.UpdateStatus(_mapper.Map<BookedServiceModel, BookedService>(bookedService)
                    , selectedPaymentStatus, paymentId, Convert.ToInt32(loggedInUser));
                return Ok(service);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }
    }
}
