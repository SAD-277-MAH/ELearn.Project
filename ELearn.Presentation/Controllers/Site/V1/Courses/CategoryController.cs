using AutoMapper;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Category;
using ELearn.Data.Models;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Courses
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork<DatabaseContext> db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet(ApiV1Routes.Category.GetCategories)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _db.CategoryRepository.GetAsync(null, o => o.OrderByDescending(c => c.DateCreated), "Parent");

            var categoriesForDetailed = new List<CategoryForDetailedDto>();
            foreach (var category in categories)
            {
                var categoryForDetailed = new CategoryForDetailedDto()
                {
                    Name = category.Name
                };
                if (category.Parent != null)
                {
                    categoryForDetailed.ParentName = category.Parent.Name;
                    categoryForDetailed.ParentId = category.ParentId;
                }
                categoriesForDetailed.Add(categoryForDetailed);
            }

            return Ok(categoriesForDetailed);
        }

        [HttpGet(ApiV1Routes.Category.GetCategory, Name = nameof(GetCategory))]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _db.CategoryRepository.GetAsync(c => c.Id == id, "Parent");

            if (category != null)
            {
                var categoryForDetailed = new CategoryForDetailedDto()
                {
                    Name = category.Name
                };
                if (category.Parent != null)
                {
                    categoryForDetailed.ParentName = category.Parent.Name;
                    categoryForDetailed.ParentId = category.ParentId;
                }

                return Ok(categoryForDetailed);
            }
            else
            {
                return BadRequest("دسته بندی یافت نشد");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost(ApiV1Routes.Category.AddCategory)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> AddCategory(CategoryForAddUpdateDto dto)
        {
            var category = await _db.CategoryRepository.GetAsync(c => c.Name == dto.Name, string.Empty);

            if (category == null)
            {
                var parent = await _db.CategoryRepository.GetAsync(dto.ParentId);

                if (parent == null)
                {
                    return BadRequest("سر دسته یافت نشد");
                }

                var newCategory = new Category()
                {
                    Name = dto.Name,
                    ParentId = dto.ParentId
                };

                await _db.CategoryRepository.AddAsync(category);

                if (await _db.SaveAsync())
                {
                    var resultCategory = new CategoryForDetailedDto()
                    {
                        Name = dto.Name,
                        ParentName = parent.Name,
                        ParentId = dto.ParentId
                    };
                    return CreatedAtRoute(nameof(GetCategory), new { id = newCategory.Id }, resultCategory);
                }
                else
                {
                    return BadRequest("خطا در ثبت اطلاعات");
                }
            }
            else
            {
                return BadRequest("دسته بندی با این نام قبلا ثبت شده است");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut(ApiV1Routes.Category.UpdateCategory)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> UpdateCategory(int id, CategoryForAddUpdateDto dto)
        {
            var category = await _db.CategoryRepository.GetAsync(id);

            if (category != null)
            {
                var bc = await _db.CategoryRepository.GetAsync(c => c.Name == dto.Name && c.Id != id, string.Empty);

                if (bc == null)
                {
                    var parent = await _db.CategoryRepository.GetAsync(dto.ParentId);

                    if (parent == null)
                    {
                        return BadRequest("سر دسته یافت نشد");
                    }

                    category.Name = dto.Name;
                    category.ParentId = dto.ParentId;

                    _db.CategoryRepository.Update(category);

                    if (await _db.SaveAsync())
                    {
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest("خطا در ثبت تغییرات");
                    }
                }
                else
                {
                    return BadRequest("دسته بندی با این نام قبلا ثبت شده است");
                }
            }
            else
            {
                return BadRequest("دسته بندی یافت نشد");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete(ApiV1Routes.Category.DeleteCategory)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _db.CategoryRepository.GetAsync(id);

            if (category != null)
            {
                _db.CategoryRepository.Delete(category);

                if (await _db.SaveAsync())
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("خطا در ثبت تغییرات");
                }
            }
            else
            {
                return BadRequest("دسته بندی یافت نشد");
            }
        }
    }
}
