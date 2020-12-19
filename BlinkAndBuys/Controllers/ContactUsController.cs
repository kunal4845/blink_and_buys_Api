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
    public class ContactUsController : ControllerBase
    {
        #region Initiate
        private IContactUsRepository _contactUsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactUsController> _logger;

        public ContactUsController(ILogger<ContactUsController> logger, IContactUsRepository contactUsRepository
            , IMapper mapper)
        {
            _contactUsRepository = contactUsRepository;
            _mapper = mapper;
            _logger = logger;

        }
        #endregion

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(ContactUsModel contactUs)
        {
            try
            {
                var response = await _contactUsRepository.PostAsync(_mapper.Map<ContactUsModel, ContactUs>(contactUs));
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
