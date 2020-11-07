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
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DealerController> _logger;

        public DealerController(IDealerRepository dealerRepository, ILogger<DealerController> logger)
        {
            _dealerRepository = dealerRepository;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [Route("verify/{userId}")]
        public async Task<IActionResult> VerifyDealer(int userId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _dealerRepository.VerifyDealer(userId, Convert.ToInt32(loggedInUser));
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
        public async Task<IActionResult> DeleteDealer(int userId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _dealerRepository.DeleteDealer(userId, Convert.ToInt32(loggedInUser));
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
        public async Task<IActionResult> BlockDealer(int userId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _dealerRepository.BlockDealer(userId, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }

        }
    }
}
