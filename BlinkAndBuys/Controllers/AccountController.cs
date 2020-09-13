using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlinkAndBuys.Helpers;
using Core;
using Core.Helper;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GroceryStore.Controllers
{

    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        #region Initiate
        private IUserRepository _userService;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly SmtpCredentials _smtpCredentials;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserRepository userService, IMapper mapper, IConfiguration config,
            IOptions<AppSettings> appSettings, IOptions<SmtpCredentials> smtpCredentials, ILogger<AccountController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _config = config;
            _appSettings = appSettings.Value;
            _smtpCredentials = smtpCredentials.Value;
            _logger = logger;
        }
        #endregion

        private string GenerateJwtToken(Account user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<Account> AuthenticateUser(Account account)
        {
            var password = Core.Helper.Helpers.EncodePasswordMd5(account.Password);
            account.Password = password;
            Account user = await _userService.AuthenticateUser(account);
            return user;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<IActionResult> SignUp([FromForm] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _mapper.Map<AccountModel, Account>(accountModel);
                    _logger.LogError("Authenticating user in database.");
                    Account user = await AuthenticateUser(account);
                    if (user == null)
                    {
                        _logger.LogError("check for the id proof and cancelled cheque files.");

                        var files = Request.Form.Files;
                        var folderName = Path.Combine("Resources", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        if (files.Any(f => f.Length == 0))
                        {
                            _logger.LogError("no files found, exception returns.");
                            return BadRequest();
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
                        }
                        account.IdProofPath = pathToSave + "\\" + accountModel.IdProof.FileName;
                        account.CancelledChequePath = pathToSave + "\\" + accountModel.CancelledCheque.FileName;
                        account.Password = Core.Helper.Helpers.EncodePasswordMd5(accountModel.Password);
                        _logger.LogError("Inserting record to database.");
                        user = await _userService.SignUp(account);
                    }

                    if (user.Id > 0)
                    {
                        _logger.LogError("Generating token.");
                        var genToken = GenerateJwtToken(user);
                        LoginToken loginToken = new LoginToken
                        {
                            Id = 0,
                            Token = genToken,
                            UserId = user.Id,
                            CreatedDt = DateTime.Now
                        };

                        _logger.LogError("Inserting toke to db.");
                        var token = await _userService.LoginToken(loginToken);
                        var response = _mapper.Map<Account, AccountModel>(user);
                        response.Token = genToken;
                        response.Password = "";
                        return Ok(response);
                    }
                    else
                    {
                        _logger.LogError("user Id is less than 0");
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Following exception has occurred: {0}", ex);
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<IActionResult> SignIn([FromBody] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _mapper.Map<AccountModel, Account>(accountModel);
                    var user = await AuthenticateUser(account);
                    if (user == null) return NotFound();

                    if (user.Id > 0)
                    {
                        var genToken = GenerateJwtToken(user);
                        LoginToken loginToken = new LoginToken
                        {
                            Token = genToken,
                            UserId = user.Id,
                            CreatedDt = DateTime.Now
                        };

                        var token = await _userService.LoginToken(loginToken);
                        var response = _mapper.Map<Account, AccountModel>(user);

                        response.Token = genToken;
                        return Ok(response);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Following exception has occurred: {0}", ex);
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("email-exists/{email}")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userService.CheckEmailExists(email);
                    return Ok(user);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("resetPassword/{email}")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userService.CheckEmailExists(email);
                    if (user != null)
                    {
                        string password = Core.Helper.PasswordStore.GeneratePassword(12, 5);
                        user.Password = password;
                        var res = await _userService.ResetPassword(user);
                        if (res != null)
                        {
                            _smtpCredentials.Subject = "Reset Password";
                            var isSent = await EmailHelper.SendEmail("ResetPassword.cshtml", _smtpCredentials);
                            if (isSent)
                            {
                                return Ok(isSent);
                            }
                        }
                        return Ok(user);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest(ex);
            }
        }
    }
}
