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
    [ApiController]
    public class PaymentController : ControllerBase
    {
        #region Initiate
        private IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(ILogger<PaymentController> logger, IPaymentRepository paymentRepository, IMapper mapper)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
        #endregion

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(PaymentModel payment)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var response = await _paymentRepository.PostAsync(_mapper.Map<PaymentModel, Payment>(payment), Convert.ToInt32(loggedInUser));
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
