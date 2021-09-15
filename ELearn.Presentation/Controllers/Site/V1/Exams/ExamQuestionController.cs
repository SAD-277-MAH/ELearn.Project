using AutoMapper;
using ELearn.Common.Filters;
using ELearn.Common.Helpers.Interface;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.ExamQuestion;
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

namespace ELearn.Presentation.Controllers.Site.V1.Exams
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class ExamQuestionController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUtilities _utilities;

        public ExamQuestionController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IUploadService uploadService, IUtilities utilities)
        {
            _db = db;
            _mapper = mapper;
            _uploadService = uploadService;
            _utilities = utilities;
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.ExamQuestion.GetExamQuestions)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(List<ExamQuestionForDetailedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExamQuestions(string userId, string examId)
        {
            var exam = await _db.ExamRepository.GetAsync(examId);
            if (exam == null)
            {
                return BadRequest("آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            var examQuestions = await _db.ExamQuestionRepository.GetAsync(e => e.ExamId == examId, null, string.Empty);

            var examQuestionsForDetailed = _mapper.Map<List<ExamQuestionForDetailedDto>>(examQuestions);

            return Ok(examQuestionsForDetailed);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.ExamQuestion.GetExamQuestion, Name = nameof(GetExamQuestion))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(ExamQuestionForDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExamQuestion(string userId, string examId, string id)
        {
            var exam = await _db.ExamRepository.GetAsync(examId);
            if (exam == null)
            {
                return BadRequest("آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            var examQuestion = await _db.ExamQuestionRepository.GetAsync(e => e.Id == id && e.ExamId == examId, string.Empty);
            if (examQuestion == null)
            {
                return BadRequest("سوال آزمون یافت نشد");
            }

            var examQuestionForDetailed = _mapper.Map<ExamQuestionForDetailedDto>(examQuestion);

            return Ok(examQuestionForDetailed);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost(ApiV1Routes.ExamQuestion.AddExamQuestion)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(ExamQuestionForDetailedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddExamQuestion(string userId, string examId, [FromForm] ExamQuestionForAddDto dto)
        {
            var exam = await _db.ExamRepository.GetAsync(examId);
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
            var newExamQuestion = new ExamQuestion()
            {
                ExamId = examId,
            };

            if (dto.File != null)
            {
                var uploadResult = await _uploadService.UploadFile(dto.File, string.Format("{0}://{1}{2}", Request.Scheme, Request.Host.Value, Request.PathBase.Value), "files\\Exams" + examId);

                if (uploadResult.Status)
                {
                    newExamQuestion.FileUrl = uploadResult.Url;
                }
                else
                {
                    return BadRequest("خطا در آپلود فایل");
                }
            }

            _mapper.Map(dto, newExamQuestion);

            await _db.ExamQuestionRepository.AddAsync(newExamQuestion);

            if (await _db.SaveAsync())
            {
                var resultExamQuestion = _mapper.Map<ExamQuestionForDetailedDto>(newExamQuestion);
                return CreatedAtRoute(nameof(GetExamQuestion), new { id = newExamQuestion.Id, examId = examId, userId = userId }, resultExamQuestion);
            }
            else
            {
                return BadRequest("خطا در ثبت اطلاعات");
            }
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPut(ApiV1Routes.ExamQuestion.UpdateExamQuestion)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExamQuestion(string userId, string examId, string id, [FromForm] ExamQuestionForUpdateDto dto)
        {
            var examQuestion = await _db.ExamQuestionRepository.GetAsync(e => e.Id == id, "Exam");
            if (examQuestion == null)
            {
                return BadRequest("سوال آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == examQuestion.Exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            dto.Title = dto.Title.Trim();
            _mapper.Map(dto, examQuestion);
            if (dto.File != null)
            {
                var uploadResult = await _uploadService.UploadFile(dto.File, string.Format("{0}://{1}{2}", Request.Scheme, Request.Host.Value, Request.PathBase.Value), "files\\Exams" + examId);

                if (uploadResult.Status)
                {
                    _uploadService.RemoveFileFromLocal(_utilities.FindLocalPathFromUrl(examQuestion.FileUrl));

                    examQuestion.FileUrl = uploadResult.Url;
                }
                else
                {
                    return BadRequest("خطا در آپلود فایل");
                }
            }
            examQuestion.DateModified = DateTime.Now;

            _db.ExamQuestionRepository.Update(examQuestion);

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
        [HttpDelete(ApiV1Routes.ExamQuestion.DeleteExamQuestion)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ServiceFilter(typeof(DocumentApproveFilter))]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteExamQuestion(string userId, string examId, string id)
        {
            var examQuestion = await _db.ExamQuestionRepository.GetAsync(e => e.Id == id, "Exam");
            if (examQuestion == null)
            {
                return BadRequest("سوال آزمون یافت نشد");
            }

            var session = await _db.SessionRepository.GetAsync(s => s.Id == examQuestion.Exam.SessionId, "Course");
            if (session.Course.TeacherId != userId)
            {
                return Unauthorized("دسترسی غیر مجاز");
            }

            int examQuestionCount = (await _db.ExamQuestionRepository.GetAsync(e => e.ExamId == examQuestion.Exam.Id, null, string.Empty)).Count();
            if (examQuestion.Exam.PassingGrade > (examQuestionCount - 1))
            {
                return BadRequest("نمره قبولی پس از حذف سوال آزمون باید کمتر از تعداد سوالات باشد");
            }

            if (!string.IsNullOrEmpty(examQuestion.FileUrl))
            {
                _uploadService.RemoveFileFromLocal(_utilities.FindLocalPathFromUrl(examQuestion.FileUrl));
            }

            _db.ExamQuestionRepository.Delete(examQuestion);

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
