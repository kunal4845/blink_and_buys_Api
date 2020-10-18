using System;
using System.Collections.Generic;
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
        private readonly AppSettings _appSettings;
        private readonly SmtpCredentials _smtpCredentials;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserRepository userService, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<SmtpCredentials> smtpCredentials,
            ILogger<AccountController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _smtpCredentials = smtpCredentials.Value;
            _logger = logger;
        }
        #endregion

        [HttpPost]
        [Route("signUp")]
        public async Task<IActionResult> SignUp([FromForm] AccountModel accountModel)
        {
            try
            {
                var account = _mapper.Map<AccountModel, Account>(accountModel);
                _logger.LogInformation("Authenticating user in database.");
                Account user = await AuthenticateUser(account);
                if (user == null)
                {
                    _logger.LogInformation("check for the id proof and cancelled cheque files.");

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
                    _logger.LogInformation("Convert images to base64.");
                    byte[] idProofImageArray = System.IO.File.ReadAllBytes(pathToSave + "\\" + accountModel.IdProof.FileName);
                    account.IdProofPath = Convert.ToBase64String(idProofImageArray);

                    byte[] chequeImageArray = System.IO.File.ReadAllBytes(pathToSave + "\\" + accountModel.CancelledCheque.FileName);
                    account.CancelledChequePath = Convert.ToBase64String(chequeImageArray);

                    account.Password = Core.Helper.Helpers.EncodePasswordMd5(accountModel.Password);
                    _logger.LogInformation("Inserting record to database.");
                    user = await _userService.SignUp(account);
                }

                if (user.Id > 0)
                {
                    _logger.LogInformation("Generating token.");
                    var genToken = GenerateJwtToken(user);
                    LoginToken loginToken = new LoginToken
                    {
                        Id = 0,
                        Token = genToken,
                        UserId = user.Id,
                        CreatedDt = DateTime.Now
                    };

                    _logger.LogInformation("Inserting toke to db.");
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

        [HttpPost]
        [Route("service-provider-register")]
        public async Task<IActionResult> ServiceProviderRegister([FromForm] AccountModel accountModel)
        {
            try
            {
                var account = _mapper.Map<AccountModel, Account>(accountModel);
                _logger.LogInformation("Authenticating user in database.");
                Account user = await AuthenticateUser(account);
                if (user == null)
                {
                    _logger.LogInformation("check for the id proof and cancelled cheque files.");

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

                    _logger.LogInformation("Convert images to base64.");
                    if (files.Count > 0)
                    {
                        if (files.Where(x => x.Name == "IdProofPath") != null)
                        {
                            byte[] idProofImageArray = System.IO.File.ReadAllBytes(pathToSave + "\\" + files.Where(x => x.Name == "IdProofPath").FirstOrDefault().FileName);
                            account.IdProofPath = Convert.ToBase64String(idProofImageArray);
                        }
                        if (files.Where(x => x.Name == "Image") != null)
                        {
                            byte[] profileImageArray = System.IO.File.ReadAllBytes(pathToSave + "\\" + files.Where(x => x.Name == "Image").FirstOrDefault().FileName);
                            account.Image = Convert.ToBase64String(profileImageArray);
                        }
                    }

                    account.Password = Core.Helper.Helpers.EncodePasswordMd5(accountModel.Password);
                    _logger.LogInformation("Inserting record to database.");
                    user = await _userService.SignUp(account);
                }

                if (user.Id > 0)
                {
                    _logger.LogInformation("Generating token.");
                    var genToken = GenerateJwtToken(user);
                    LoginToken loginToken = new LoginToken
                    {
                        Id = 0,
                        Token = genToken,
                        UserId = user.Id,
                        CreatedDt = DateTime.Now
                    };

                    _logger.LogInformation("Inserting toke to db.");
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

        [HttpPost]
        [Route("signIn")]
        public async Task<IActionResult> SignIn([FromBody] AccountModel accountModel)
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

        [HttpGet]
        [Route("email-exists/{email}/{roleId}")]
        public async Task<IActionResult> CheckEmailExists(string email, int roleId)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userService.CheckEmailExists(email, roleId);
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
        public async Task<IActionResult> ResetPassword(string email, int roleId)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userService.CheckEmailExists(email, roleId);
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

        [HttpPost]
        [Route("user-register")]
        public async Task<IActionResult> UserRegister([FromBody] AccountModel accountModel)
        {
            try
            {
                var account = _mapper.Map<AccountModel, Account>(accountModel);
                _logger.LogInformation("Authenticating user in database.");
                Account user = await AuthenticateUser(account);
                if (user == null)
                {
                    account.Password = Core.Helper.Helpers.EncodePasswordMd5(accountModel.Password);
                    _logger.LogInformation("Inserting record to database.");
                    user = await _userService.SignUp(account);
                }

                if (user.Id > 0)
                {
                    _logger.LogInformation("Generating token.");
                    var genToken = GenerateJwtToken(user);
                    LoginToken loginToken = new LoginToken
                    {
                        Id = 0,
                        Token = genToken,
                        UserId = user.Id,
                        CreatedDt = DateTime.Now
                    };

                    _logger.LogInformation("Inserting toke to db.");
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

        [HttpGet]
        [Route("getUserByUserName/{email}/{roleId}")]
        public async Task<IActionResult> GetUserByUserName(string email, int roleId)
        {
            try
            {
                _logger.LogInformation("check email exists: email {0}", email);
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userService.CheckEmailExists(email, roleId);
                    if (user != null)
                    {
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
    }
}
