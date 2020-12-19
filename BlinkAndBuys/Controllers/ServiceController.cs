using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using BlinkAndBuys.Helpers;
using Core;
using Core.Helper;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlinkAndBuys.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        #region Initiate
        private IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceRepository serviceRepository, IMapper mapper, ILogger<ServiceController> logger)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [Route("{serviceId?}")]
        public async Task<IActionResult> Get(int? serviceId)
        {
            try
            {
                var services = await _serviceRepository.GetServices(serviceId);
                return Ok(_mapper.Map<List<Service>, List<ServiceModel>>(services));
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{serviceId}")]
        public async Task<IActionResult> Delete(int serviceId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _serviceRepository.Delete(serviceId, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromForm] ServiceModel serviceModel)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = _mapper.Map<ServiceModel, Service>(serviceModel);
                if (serviceModel.ServiceIcon != null)
                {
                    _logger.LogInformation("check service icon file.");
                    var files = Request.Form.Files;
                    var folderName = Path.Combine("Resources", "ServiceIcons");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (files.Any(f => f.Length == 0))
                    {
                        _logger.LogError("no files found, exception returns.");
                    }

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
                        byte[] iconArray = System.IO.File.ReadAllBytes(pathToSave + "\\" + fileName);
                        service.ServiceIcon = "data:image/jpg;base64," + Convert.ToBase64String(iconArray);
                    }

                    _logger.LogInformation("Inserting record to database.");
                    var res = await _serviceRepository.Upsert(service, Convert.ToInt32(loggedInUser));
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddService(ServiceModel serviceModel)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = _mapper.Map<ServiceModel, Service>(serviceModel);
                var res = await _serviceRepository.Upsert(service, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("booked/{bookedServiceId?}")]
        public async Task<IActionResult> GetBookedServices(int? bookedServiceId)
        {
            try
            {
                var services = await _serviceRepository.GetBookedServices(bookedServiceId);
                return Ok(_mapper.Map<List<BookedService>, List<BookedServiceModel>>(services));
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("reject/{bookedServiceId}")]
        public async Task<IActionResult> RejectService(int bookedServiceId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = await _serviceRepository.RejectService(bookedServiceId, Convert.ToInt32(loggedInUser));
                return Ok(service);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("rejectedByServiceProvider/{bookedServiceId}")]
        public async Task<IActionResult> RejectedByServiceProvider(int bookedServiceId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = await _serviceRepository.RejectedByServiceProvider(bookedServiceId, Convert.ToInt32(loggedInUser));
                return Ok(service);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("approvedByServiceProvider/{bookedServiceId}")]
        public async Task<IActionResult> ApprovedByServiceProvider(int bookedServiceId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var service = await _serviceRepository.ApprovedByServiceProvider(bookedServiceId, Convert.ToInt32(loggedInUser));
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
