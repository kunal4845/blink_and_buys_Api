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

namespace BlinkAndBuys.Controllers {

    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    public class SharedController : ControllerBase {
        #region Initiate
        private IUserRepository _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<SharedController> _logger;

        public SharedController(IUserRepository userService, IMapper mapper, ILogger<SharedController> logger) {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;

        }
        #endregion

        [HttpGet]
        [Route("listUsers")]
        public async Task<IActionResult> GetUsers() {
            try {
                var users = await _userService.GetUsers();
                return Ok(_mapper.Map<List<Account>, List<AccountModel>>(users));
            }
            catch (Exception ex) {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }
    }
}