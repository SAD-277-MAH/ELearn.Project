using AutoMapper;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Exam;
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

namespace ELearn.Presentation.Controllers.Site.V1.Exams
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;

        public ExamController(IUnitOfWork<DatabaseContext> db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Exam.GetExams)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(List<ExamForDetailedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExams(string userId, string courseId)
        {
            var course = await _db.CourseRepository.GetAsync(c => c.Id == courseId, "Sessions");

            if (course == null)
            {
                return BadRequest("دوره یافت نشد");
            }

            if (course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            var sessionIds = course.Sessions.Select(s => s.Id).ToList();

            var exams = _mapper.Map<List<ExamForDetailedDto>>(await _db.ExamRepository.GetAsync(e => sessionIds.Contains(e.SessionId), null, string.Empty));

            return Ok(exams);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Exam.GetExam, Name = nameof(GetExam))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(ExamForDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExam(string userId, string id)
        {
            var exam = await _db.ExamRepository.GetAsync(id);
            if (exam == null)
            {
                return BadRequest("آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            var examForDetailed = _mapper.Map<ExamForDetailedDto>(exam);

            return Ok(examForDetailed);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost(ApiV1Routes.Exam.AddExam)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(ExamForDetailedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddExam(string userId, ExamForAddDto dto)
        {
            var exam = await _db.ExamRepository.GetAsync(e => e.SessionId == dto.SessionId, string.Empty);
            if (exam != null)
            {
                return BadRequest("برای این جلسه قبلا آزمون ثبت شده است");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == dto.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            dto.Title = dto.Title.Trim();
            var newExam = _mapper.Map<Exam>(dto);

            await _db.ExamRepository.AddAsync(newExam);

            if (await _db.SaveAsync())
            {
                var resultExam = _mapper.Map<ExamForDetailedDto>(newExam);
                return CreatedAtRoute(nameof(GetExam), new { userId = userId, id = newExam.Id }, resultExam);
            }
            else
            {
                return BadRequest("خطا در ثبت اطلاعات");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPut(ApiV1Routes.Exam.UpdateExam)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExam(string userId, string id, ExamForUpdateDto dto)
        {
            var exam = await _db.ExamRepository.GetAsync(id);
            if (exam == null)
            {
                return BadRequest("آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            dto.Title = dto.Title.Trim();
            _mapper.Map(dto, exam);
            exam.DateModified = DateTime.Now;

            _db.ExamRepository.Update(exam);

            if (await _db.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest("خطا در ثبت تغییرات");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPatch(ApiV1Routes.Exam.UpdatePassingGrade)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePassingGrade(string userId, string id, ExamForUpdatePassingGradeDto dto)
        {
            var exam = await _db.ExamRepository.GetAsync(id);
            if (exam == null)
            {
                return BadRequest("آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            int examQuestionCount = (await _db.ExamQuestionRepository.GetAsync(e => e.ExamId == id, null, string.Empty)).Count();
            if (dto.PassingGrade > examQuestionCount)
            {
                return BadRequest("نمره قبولی باید کمتر از تعداد سوالات باشد");
            }

            exam.PassingGrade = dto.PassingGrade;
            exam.DateModified = DateTime.Now;

            _db.ExamRepository.Update(exam);

            if (await _db.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest("خطا در ثبت تغییرات");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpDelete(ApiV1Routes.Exam.DeleteExam)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteExam(string userId, string id)
        {
            var exam = await _db.ExamRepository.GetAsync(id);
            if (exam == null)
            {
                return BadRequest("آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            _db.ExamRepository.Delete(exam);

            if (await _db.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest("خطا در ثبت تغییرات");
            }
        }
    }
}
