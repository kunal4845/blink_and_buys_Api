using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Core.Helper;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlinkAndBuys.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        #region Initiate
        private IDealerRepository _dealerRepository;
        private readonly IMapper _mapper;
        private readonly SmtpCredentials _smtpCredentials;

        public DealerController(IDealerRepository dealerRepository, IMapper mapper, IOptions<SmtpCredentials> smtpCredentials)
        {
            _dealerRepository = dealerRepository;
            _mapper = mapper;
            _smtpCredentials = smtpCredentials.Value;
        }
        #endregion

        [HttpGet]
        [Route("verify/{userId}")]
        public async Task<IActionResult> VerifyDealer(int userId)
        {
            var loggedInUser = Request.HttpContext.Items["userId"];
            var res = await _dealerRepository.VerifyDealer(userId, Convert.ToInt32(loggedInUser));
            return BadRequest(res);
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteDealer(int userId)
        {
            var loggedInUser = Request.HttpContext.Items["userId"];
            var res = await _dealerRepository.DeleteDealer(userId, Convert.ToInt32(loggedInUser));
            return Ok(res);
        }

        [HttpGet]
        [Route("block/{userId}")]
        public async Task<IActionResult> BlockDealer(int userId)
        {
            var loggedInUser = Request.HttpContext.Items["userId"];
            var res = await _dealerRepository.BlockDealer(userId, Convert.ToInt32(loggedInUser));
            return Ok(res);
        }
    }
}
