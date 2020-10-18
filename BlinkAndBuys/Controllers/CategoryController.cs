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


namespace BlinkAndBuys.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region Initiate
        private ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [Route("{categoryId?}")]
        public async Task<IActionResult> Get(int? categoryId)
        {
            try
            {
                var categories = await _categoryRepository.Get(categoryId);
                return Ok(_mapper.Map<List<Category>, List<CategoryModel>>(categories));
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var res = await _categoryRepository.Delete(categoryId, Convert.ToInt32(loggedInUser));
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Addcategory(CategoryModel categoryModel)
        {
            try
            {
                var loggedInUser = Request.HttpContext.Items["userId"];
                var category = _mapper.Map<CategoryModel, Category>(categoryModel);
                var res = await _categoryRepository.Upsert(category, Convert.ToInt32(loggedInUser));
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
