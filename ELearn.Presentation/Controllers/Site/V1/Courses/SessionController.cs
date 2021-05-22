using AutoMapper;
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
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Sessions
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUtilities _utilities;

        public SessionController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IUploadService uploadService, IUtilities utilities)
        {
            _db = db;
            _mapper = mapper;
            _uploadService = uploadService;
            _utilities = utilities;
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Session.GetSessions)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetSessions(string courseId, string userId)
        {
            var sessions = await _db.SessionRepository.GetAsync(s => s.CourseId == courseId && s.Course.TeacherId == userId, o => o.OrderBy(s => s.SessionNumber), "Course");

            var sessionsForDetailed = _mapper.Map<List<SessionForDetailedDto>>(sessions);

            return Ok(sessionsForDetailed);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Session.GetSession, Name = nameof(GetSession))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetSession(string id, string courseId, string userId)
        {
            var session = await _db.SessionRepository.GetAsync(s => s.Id == id && s.CourseId == courseId && s.Course.TeacherId == userId, "Course");

            if (session != null)
            {
                var sessionForDetailed = _mapper.Map<SessionForDetailedDto>(session);

                return Ok(sessionForDetailed);
            }
            else
            {
                return BadRequest("جلسه دوره یافت نشد");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost(ApiV1Routes.Session.AddSession)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> AddSession(string courseId, string userId, [FromForm] SessionForAddDto dto)
        {
            dto.Title = dto.Title.Trim();
            var session = await _db.SessionRepository.GetAsync(s => s.Title == dto.Title && s.CourseId == courseId, string.Empty);

            if (session == null)
            {
                var course = await _db.CourseRepository.GetAsync(courseId);
                if (course == null)
                {
                    return BadRequest("دوره یافت نشد");
                }

                var uploadResult = await _uploadService.UploadFile(dto.File, string.Format("{0}://{1}{2}", Request.Scheme, Request.Host.Value, Request.PathBase.Value), "videos\\Courses\\" + courseId);

                if (uploadResult.Status)
                {
                    var newSession = new Session()
                    {
                        CourseId = courseId,
                        VideoUrl = uploadResult.Url
                    };

                    _mapper.Map(dto, newSession);

                    await _db.SessionRepository.AddAsync(newSession);

                    if (await _db.SaveAsync())
                    {
                        var resultSession = _mapper.Map<SessionForDetailedDto>(newSession);
                        return CreatedAtRoute(nameof(GetSession), new { id = newSession.Id, courseId = courseId, userId = userId }, resultSession);
                    }
                    else
                    {
                        return BadRequest("خطا در ثبت اطلاعات");
                    }
                }
                else
                {
                    return BadRequest("خطا در آپلود ویدئو");
                }
            }
            else
            {
                return BadRequest("جلسه دوره با این نام قبلا ثبت شده است");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPut(ApiV1Routes.Session.UpdateSession)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> UpdateSession(string id, string courseId, string userId, [FromForm] SessionForUpdateDto dto)
        {
            var session = await _db.SessionRepository.GetAsync(s => s.Id == id && s.CourseId == courseId && s.Course.TeacherId == userId, "Course");
            if (session != null)
            {
                dto.Title = dto.Title.Trim();
                var sessionExist = await _db.SessionRepository.GetAsync(s => s.Title == dto.Title && s.CourseId == courseId && s.Id != id, string.Empty);

                if (sessionExist == null)
                {
                    _mapper.Map(dto, session);

                    if (dto.File != null)
                    {
                        var uploadResult = await _uploadService.UploadFile(dto.File, string.Format("{0}://{1}{2}", Request.Scheme, Request.Host.Value, Request.PathBase.Value), "videos\\Courses\\" + courseId);

                        if (uploadResult.Status)
                        {
                            _uploadService.RemoveFileFromLocal(_utilities.FindLocalPathFromUrl(session.VideoUrl));

                            session.VideoUrl = uploadResult.Url;
                        }
                        else
                        {
                            return BadRequest("خطا در آپلود ویدئو");
                        }
                    }
                    session.DateModified = DateTime.Now;

                    _db.SessionRepository.Update(session);

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
                    return BadRequest("جلسه دوره با این نام قبلا ثبت شده است");
                }
            }
            else
            {
                return BadRequest("جلسه دوره یافت نشد");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpDelete(ApiV1Routes.Session.DeleteSession)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> DeleteSession(string id, string courseId, string userId)
        {
            var session = await _db.SessionRepository.GetAsync(s => s.Id == id && s.CourseId == courseId && s.Course.TeacherId == userId, "Course");

            if (session != null)
            {
                _uploadService.RemoveFileFromLocal(_utilities.FindLocalPathFromUrl(session.VideoUrl));

                _db.SessionRepository.Delete(session);

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
                return BadRequest("جلسه دوره یافت نشد");
            }
        }
    }
}
