using AutoMapper;
using Banking.Data.Dtos.Common.Pagination;
using ELearn.Common.Extentions;
using ELearn.Common.Filters;
using ELearn.Common.Helpers.Interface;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Course;
using ELearn.Data.Models;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Upload.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Courses
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUtilities _utilities;

        public CourseController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IUploadService uploadService, IUtilities utilities)
        {
            _db = db;
            _mapper = mapper;
            _uploadService = uploadService;
            _utilities = utilities;
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Course.GetCourses)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetCourses(string userId, [FromQuery] PaginationDto pagination)
        {
            var courses = await _db.CourseRepository.GetPagedListAsync(pagination, pagination.Filter.ToCourseExpression(false, userId), pagination.SortHeader.ToCourseOrderBy(pagination.SortDirection), "Teacher,Category,Prerequisites");
            Response.AddPagination(courses.CurrentPage, courses.PageSize, courses.TotalCount, courses.TotalPages);
            var coursesForDetailed = _mapper.Map<List<CourseForDetailedDto>>(courses);

            return Ok(coursesForDetailed);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Course.GetCourse, Name = nameof(GetCourse))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetCourse(string id, string userId)
        {
            var course = await _db.CourseRepository.GetAsync(c => c.Id == id, string.Empty);

            if (course != null)
            {
                if (course.TeacherId == userId)
                {
                    var courseForDetailed = _mapper.Map<CourseForDetailedDto>(course);

                    return Ok(courseForDetailed);
                }
                else
                {
                    return Unauthorized("دسترسی غیر مجاز");
                }
            }
            else
            {
                return BadRequest("دوره یافت نشد");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost(ApiV1Routes.Course.AddCourse)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> AddCourse(string userId, [FromForm] CourseForAddDto dto)
        {
            dto.Title = dto.Title.Trim();
            var course = await _db.CourseRepository.GetAsync(b => b.Title == dto.Title, string.Empty);

            if (course == null)
            {
                var category = await _db.CategoryRepository.GetAsync(dto.CategoryId);
                if (category == null)
                {
                    return BadRequest("دسته بندی یافت نشد");
                }

                if (dto.HasPrerequisites)
                {
                    if (string.IsNullOrWhiteSpace(dto.PrerequisitesId))
                    {
                        return BadRequest("پیشنیاز را انتخاب کنید");
                    }
                    else
                    {
                        var courseExist = await _db.CourseRepository.GetAsync(dto.PrerequisitesId);
                        if (courseExist == null)
                        {
                            return BadRequest("پیشنیاز یافت نشد");
                        }
                    }
                }
                else
                {
                    dto.PrerequisitesId = null;
                }

                if (!dto.File.IsImage())
                {
                    return BadRequest("تصویر معتبر نیست");
                }

                PersianCalendar pc = new PersianCalendar();
                var uploadResult = await _uploadService.UploadFile(dto.File, string.Format("{0}://{1}{2}", Request.Scheme, Request.Host.Value, Request.PathBase.Value), "images\\Courses\\" + pc.GetYear(DateTime.Now) + "\\" + pc.GetMonth(DateTime.Now) + "\\" + pc.GetDayOfMonth(DateTime.Now));

                if (uploadResult.Status)
                {
                    var newCourse = new Course()
                    {
                        TeacherId = userId,
                        Status = 1,
                        PhotoUrl = uploadResult.Url
                    };

                    _mapper.Map(dto, newCourse);

                    await _db.CourseRepository.AddAsync(newCourse);

                    if (await _db.SaveAsync())
                    {
                        var resultCourse = _mapper.Map<CourseForDetailedDto>(newCourse);
                        return CreatedAtRoute(nameof(GetCourse), new { id = newCourse.Id }, resultCourse);
                    }
                    else
                    {
                        return BadRequest("خطا در ثبت اطلاعات");
                    }
                }
                else
                {
                    return BadRequest("خطا در آپلود تصویر");
                }
            }
            else
            {
                return BadRequest("دوره با این نام قبلا ثبت شده است");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPut(ApiV1Routes.Course.UpdateCourse)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> UpdateCourse(string id, string userId, [FromForm] CourseForUpdateDto dto)
        {
            var course = await _db.CourseRepository.GetAsync(id);
            if (course != null)
            {
                if (course.TeacherId == userId)
                {
                    dto.Title = dto.Title.Trim();
                    var courseExist = await _db.CourseRepository.GetAsync(c => c.Title == dto.Title && c.Id != id, string.Empty);

                    if (courseExist == null)
                    {
                        _mapper.Map(dto, course);

                        if (dto.File != null)
                        {
                            if (!dto.File.IsImage())
                            {
                                return BadRequest("تصویر معتبر نیست");
                            }

                            PersianCalendar pc = new PersianCalendar();
                            var uploadResult = await _uploadService.UploadFile(dto.File, string.Format("{0}://{1}{2}", Request.Scheme, Request.Host.Value, Request.PathBase.Value), "images\\Courses\\" + pc.GetYear(DateTime.Now) + "\\" + pc.GetMonth(DateTime.Now) + "\\" + pc.GetDayOfMonth(DateTime.Now));

                            if (uploadResult.Status)
                            {
                                _uploadService.RemoveFileFromLocal(_utilities.FindLocalPathFromUrl(course.PhotoUrl));

                                course.PhotoUrl = uploadResult.Url;
                            }
                            else
                            {
                                return BadRequest("خطا در آپلود تصویر");
                            }
                        }
                        course.Status = 1;
                        course.DateModified = DateTime.Now;

                        _db.CourseRepository.Update(course);

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
                        return BadRequest("دوره با این نام قبلا ثبت شده است");
                    }
                }
                else
                {
                    return Unauthorized("دسترسی غیر مجاز");
                }
            }
            else
            {
                return BadRequest("دوره یافت نشد");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpDelete(ApiV1Routes.Course.DeleteCourse)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _db.CourseRepository.GetAsync(id);

            if (course != null)
            {
                _uploadService.RemoveFileFromLocal(_utilities.FindLocalPathFromUrl(course.PhotoUrl));

                _db.CourseRepository.Delete(course);

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
                return BadRequest("دوره یافت نشد");
            }
        }
    }
}
