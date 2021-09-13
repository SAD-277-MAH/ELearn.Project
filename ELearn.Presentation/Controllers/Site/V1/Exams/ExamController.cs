using AutoMapper;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Exam;
using ELearn.Data.Models;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Site.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Exams
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public ExamController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IOrderService orderService)
        {
            _db = db;
            _mapper = mapper;
            _orderService = orderService;
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

            var exams = _mapper.Map<List<ExamForDetailedDto>>(await _db.ExamRepository.GetAsync(e => sessionIds.Contains(e.SessionId), null, "ExamQuestions"));

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
            var exam = await _db.ExamRepository.GetAsync(e => e.Id == id, "ExamQuestions");
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

        [Authorize(Policy = "RequireTeacherOrStudentRole")]
        [HttpGet(ApiV1Routes.Exam.GetExamForSession)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(ExamForDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExamForSession(string userId, string sessionId)
        {
            var exam = await _db.ExamRepository.GetAsync(e => e.SessionId == sessionId, "ExamQuestions");
            if (exam == null)
            {
                return NotFound();
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (User.HasClaim(ClaimTypes.Role, "Teacher") && session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }
            if (User.HasClaim(ClaimTypes.Role, "Student") && (!await _orderService.IsUserInCourseAsync(userId, session.CourseId)))
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

        [Authorize(Policy = "RequireStudentRole")]
        [HttpPost(ApiV1Routes.Exam.TakeExam)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(ExamForStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TakeExam(string userId, string id, List<ExamForAnswerDto> dto)
        {
            var exam = await _db.ExamRepository.GetAsync(e => e.Id == id, "ExamQuestions");
            if (exam == null)
            {
                return BadRequest("آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (!await _orderService.IsUserInCourseAsync(userId, session.CourseId))
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            var examAnswer = await _db.ExamAnswerRepository.GetAsync(e => e.UserId == userId && e.ExamId == id, string.Empty);
            if (examAnswer != null && examAnswer.Status)
            {
                return BadRequest("قبلا در آزمون شرکت کرده اید و نمره قبولی گرفته اید");
            }

            int grade = 0;
            foreach (var answer in dto)
            {
                var examQuestion = exam.ExamQuestions.Where(e => e.Id == answer.ExamQuestionId).SingleOrDefault();
                if (examQuestion != null && examQuestion.CorrectAnswer == answer.Answer)
                {
                    grade++;
                }
            }

            bool hasPassed = grade >= exam.PassingGrade;
            if (examAnswer == null)
            {
                examAnswer = new ExamAnswer()
                {
                    Grade = grade,
                    Status = hasPassed,
                    UserId = userId,
                    ExamId = id
                };

                await _db.ExamAnswerRepository.AddAsync(examAnswer);
            }
            else if (grade > examAnswer.Grade || hasPassed)
            {
                examAnswer.Grade = grade;
                examAnswer.Status = hasPassed;

                _db.ExamAnswerRepository.Update(examAnswer);
            }

            if (await _db.SaveAsync())
            {
                var result = new ExamForStatusDto()
                {
                    Grade = grade,
                    PassingGrade = exam.PassingGrade,
                    MaxGrade = exam.ExamQuestions.Count,
                    Status = hasPassed
                };

                return Ok(result);
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
