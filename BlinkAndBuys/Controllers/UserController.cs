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
    public class UserController : ControllerBase
    {
        #region Initiate
        private IUserRepository _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = Request.HttpContext.Items["userId"];
                var user = new Account();
                if (userId != null)
                {
                    user = await _userService.GetUserById(Convert.ToInt32(userId));
                }

                user.Password = "";
                return Ok(_mapper.Map<Account, AccountModel>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword(AccountModel account)
        {
            try
            {
                var userId = Request.HttpContext.Items["userId"];
                account.Id = Convert.ToInt32(userId);

                account.Password = Core.Helper.Helpers.EncodePasswordMd5(account.Password);
                account.ConfirmPassword = Core.Helper.Helpers.EncodePasswordMd5(account.ConfirmPassword);

                var user = await _userService.UpdatePassword(Convert.ToInt32(userId), account.Password, account.ConfirmPassword);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }
    }
}