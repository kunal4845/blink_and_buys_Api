using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

        [HttpPost]
        [Route("update-profile-image")]
        public async Task<IActionResult> UpdateProfileImage()
        {
            try
            {
                var userId = Request.HttpContext.Items["userId"];

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

                var profileImage = "";
                if (files.Count > 0)
                {
                    if (files.Where(x => x.Name == "Image") != null)
                    {
                        byte[] profileImageArray = System.IO.File.ReadAllBytes(pathToSave + "\\" + files.Where(x => x.Name == "image").FirstOrDefault().FileName);
                        profileImage = "data:image/jpg;base64," + Convert.ToBase64String(profileImageArray);
                    }
                }

                var user = await _userService.UpdateProfileImage(Convert.ToInt32(userId), profileImage);
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