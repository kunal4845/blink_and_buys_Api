﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using BlinkAndBuys.Helpers;
using Core;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BlinkAndBuys.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Initiate
        private IUserRepository _userService;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        private readonly AppSettings _appSettings;

        public UserController(IUserRepository userService, IMapper mapper, IConfiguration config,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _config = config;
            _appSettings = appSettings.Value;
        }
        #endregion

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = Request.HttpContext.Items["userId"];
                userId = 1;
                var user = new Account();
                if (userId != null)
                {
                    user = await _userService.GetUserById(Convert.ToInt32(userId));
                }
                return Ok(_mapper.Map<Account, AccountModel>(user));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}