using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlinkAndBuys.Helpers;
using Core;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GroceryStore.Controllers {

    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase {
        #region Initiate
        private IUserRepository _userService;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        private readonly AppSettings _appSettings;

        public AccountController(IUserRepository userService, IMapper mapper, IConfiguration config,
            IOptions<AppSettings> appSettings) {
            _userService = userService;
            _mapper = mapper;
            _config = config;
            _appSettings = appSettings.Value;
        }
        #endregion

        private string GenerateJwtToken(Account user) {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<Account> AuthenticateUser(Account account) {
            var password = Core.Helper.Helpers.EncodePasswordMd5(account.Password);
            account.Password = password;
            Account user = await _userService.AuthenticateUser(account);
            return user;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<IActionResult> SignUp([FromBody]AccountModel accountModel) {
            if (ModelState.IsValid) {
                try {
                    var account = _mapper.Map<AccountModel, Account>(accountModel);
                    Account user = await AuthenticateUser(account);
                    if (user == null) {
                        account.Password = Core.Helper.Helpers.EncodePasswordMd5(accountModel.Password);
                        user = await _userService.SignUp(account);
                    }

                    if (user.Id > 0) {
                        var genToken = GenerateJwtToken(user);
                        LoginToken loginToken = new LoginToken {
                            Id = 0,
                            Token = genToken,
                            UserId = user.Id,
                            CreatedDt = DateTime.Now
                        };

                        var token = await _userService.LoginToken(loginToken);
                        var response = _mapper.Map<Account, AccountModel>(user);
                        response.Token = genToken;
                        response.Password = "";
                        return Ok(response);
                    }
                    else {
                        return NotFound();
                    }
                }
                catch (Exception ex) {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<IActionResult> SignIn([FromBody]AccountModel accountModel) {
            if (ModelState.IsValid) {
                try {
                    var account = _mapper.Map<AccountModel, Account>(accountModel);
                    var user = await AuthenticateUser(account);
                    if (user == null) return NotFound();

                    if (user.Id > 0) {
                        var genToken = GenerateJwtToken(user);
                        LoginToken loginToken = new LoginToken {
                            Token = genToken,
                            UserId = user.Id,
                            CreatedDt = DateTime.Now
                        };

                        var token = await _userService.LoginToken(loginToken);
                        var response = _mapper.Map<Account, AccountModel>(user);

                        response.Token = genToken;
                        return Ok(response);
                    }
                    else {
                        return NotFound();
                    }
                }
                catch (Exception ex) {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("email-exists/{email}")]
        public async Task<IActionResult> CheckEmailExists(string email) {
            try {
                if (!string.IsNullOrEmpty(email)) {
                    var user = await _userService.CheckEmailExists(email);
                    return Ok(user);
                }
                return NotFound();
            }
            catch (Exception ex) {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("resetPassword/{email}")]
        public async Task<IActionResult> ResetPassword(string email) {
            try {
                if (!string.IsNullOrEmpty(email)) {
                    var user = await _userService.CheckEmailExists(email);
                    if (user != null) {
                        string password = Core.Helper.PasswordStore.GeneratePassword(12, 5);
                        user.Password = password;
                        await _userService.ResetPassword(user);
                        return Ok(user);
                    }
                }
                return NotFound();
            }
            catch (Exception ex) {
                return BadRequest();
            }
        }
    }
}
