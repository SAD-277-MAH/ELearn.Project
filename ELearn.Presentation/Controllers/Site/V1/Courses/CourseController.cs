using AutoMapper;
using Banking.Data.Dtos.Common.Pagination;
using ELearn.Common.Extentions;
using ELearn.Common.Filters;
using ELearn.Common.Helpers.Interface;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Comment;
using ELearn.Data.Dtos.Site.Common;
using ELearn.Data.Dtos.Site.Course;
using ELearn.Data.Enums;
using ELearn.Data.Models;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Site.Interface;
using ELearn.Services.Upload.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
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
        private readonly IOrderService _orderService;

        public CourseController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IUploadService uploadService, IUtilities utilities, IOrderService orderService)
        {
            _db = db;
            _mapper = mapper;
            _uploadService = uploadService;
            _utilities = utilities;
            _orderService = orderService;
        }

        [HttpGet(ApiV1Routes.Course.GetCourses)]
        [ProducesResponseType(typeof(List<CourseForDetailedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourses([FromQuery] PaginationDto pagination)
        {
            var courses = await _db.CourseRepository.GetPagedListAsync(pagination, pagination.Filter.ToCourseExpression(StatusType.Approved), pagination.SortHeader.ToCourseOrderBy(pagination.SortDirection), "Teacher");
            Response.AddPagination(courses.CurrentPage, courses.PageSize, courses.TotalCount, courses.TotalPages);
            var coursesForDetailed = _mapper.Map<List<CourseForDetailedDto>>(courses);

            return Ok(coursesForDetailed);
        }

        [Authorize]
        [HttpGet(ApiV1Routes.Course.GetCoursesForAdmin)]
        [ProducesResponseType(typeof(List<CourseForAdminDetailedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCoursesForAdmin([FromQuery] PaginationDto pagination, [FromQuery] int? status)
        {
            StatusType statusType = StatusType.All;
            if (status != null)
            {
                if (status == -1)
                {
                    statusType = StatusType.Reject;
                }
                else if (status == 0)
                {
                    statusType = StatusType.Pending;
                }
                else if (status == 1)
                {
                    statusType = StatusType.Approved;
                }
            }

            var courses = await _db.CourseRepository.GetPagedListAsync(pagination, pagination.Filter.ToCourseExpression(statusType), pagination.SortHeader.ToCourseOrderBy(pagination.SortDirection), "Teacher");
            Response.AddPagination(courses.CurrentPage, courses.PageSize, courses.TotalCount, courses.TotalPages);
            var coursesForDetailed = _mapper.Map<List<CourseForAdminDetailedDto>>(courses);

            return Ok(coursesForDetailed);
        }

        [Authorize(Policy = "RequireTeacherOrStudentRole")]
        [HttpGet(ApiV1Routes.Course.GetUserCourses)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(List<CourseForTeacherDetailedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserCourses(string userId)
        {
            if (User.HasClaim(ClaimTypes.Role, "Teacher"))
            {
                var courses = await _db.CourseRepository.GetAsync(c => c.TeacherId == userId, o => o.OrderByDescending(c => c.DateCreated), "Sessions");

                var coursesForDetailed = _mapper.Map<List<CourseForTeacherDetailedDto>>(courses);

                return Ok(coursesForDetailed);
            }
            else
            {
                var studentCourses = await _db.UserCourseRepository.GetAsync(u => u.UserId == userId, o => o.OrderByDescending(c => c.DateCreated), string.Empty);

                var coursesForDetailed = new List<CourseForStudentDetailedDto>();

                foreach (var studentCourse in studentCourses)
                {
                    var course = await _db.CourseRepository.GetAsync(c => c.Id == studentCourse.CourseId, "Sessions");
                    coursesForDetailed.Add(_mapper.Map<CourseForStudentDetailedDto>(course));
                }

                return Ok(coursesForDetailed);
            }

        }

        [HttpGet(ApiV1Routes.Course.GetCourse, Name = nameof(GetCourse))]
        [ProducesResponseType(typeof(CourseForSiteDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourse(string id)
        {
            var course = await _db.CourseRepository.GetAsync(c => c.Id == id && c.Status == 1, "Teacher,Sessions,Comments");
            if (User.Identity.IsAuthenticated && User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                course = await _db.CourseRepository.GetAsync(c => c.Id == id, "Teacher,Sessions,Comments");
            }
             
            if (course != null)
            {
                var courseForDetailed = _mapper.Map<CourseForSiteDetailedDto>(course);
                courseForDetailed.Comments = _mapper.Map<List<CommentForDetailedDto>>(course.Comments.Where(c => c.Status == 1).OrderByDescending(c => c.DateCreated));

                if (User.Identity.IsAuthenticated && ((User.HasClaim(ClaimTypes.Role, "Student") && await _orderService.IsUserInCourseAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, id)) || User.HasClaim(ClaimTypes.Role, "Admin")))
                {
                    courseForDetailed.Sessions = _mapper.Map<List<SessionForDetailedDto>>(course.Sessions.OrderBy(s => s.SessionNumber));
                    courseForDetailed.Status = 1;
                }
                else
                {
                    courseForDetailed.Sessions = null;
                    courseForDetailed.Status = -1;
                }

                #region Calculate Duration
                int totalHour = 0;
                int totalMinute = 0;

                foreach (var session in course.Sessions)
                {
                    totalHour += session.Time.Hours;
                    totalMinute += session.Time.Minutes;
                }

                int tempHour = 0;
                if (totalMinute > 0)
                {
                    tempHour = totalMinute / 60;

                }
                int newHour = tempHour + totalHour;
                int newMinute = totalMinute - (tempHour * 60);

                courseForDetailed.Duration = $"{newHour}:{newMinute}";
                #endregion

                return Ok(courseForDetailed);
            }
            else
            {
                return BadRequest("دوره یافت نشد");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost(ApiV1Routes.Course.AddCourse)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(CourseForDetailedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCourse(string userId, [FromForm] CourseForAddDto dto)
        {
            dto.Title = dto.Title.Trim();
            var course = await _db.CourseRepository.GetAsync(c => c.Title == dto.Title, string.Empty);

            if (course == null)
            {
                var category = await _db.CategoryRepository.GetAsync(dto.CategoryId);
                if (category == null)
                {
                    return BadRequest("دسته بندی یافت نشد");
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
                        Status = 0,
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
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
                        course.Status = 0;
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

        [Authorize]
        [HttpPatch(ApiV1Routes.Course.UpdateCourseStatus)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourseStatus(string id, UpdateStatusDto dto)
        {
            var course = await _db.CourseRepository.GetAsync(id);
            if (course != null)
            {
                course.Status = dto.Status;
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
                return BadRequest("دوره یافت نشد");
            }
        }

        [Authorize]
        [HttpDelete(ApiV1Routes.Course.DeleteCourse)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
