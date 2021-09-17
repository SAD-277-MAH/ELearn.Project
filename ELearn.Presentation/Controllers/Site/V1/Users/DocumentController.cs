using AutoMapper;
using ELearn.Common.Extentions;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Document;
using ELearn.Data.Dtos.Site.Users;
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

namespace ELearn.Presentation.Controllers.Site.V1.Users
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public DocumentController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IUploadService uploadService)
        {
            _db = db;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Document.GetDocuments)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(List<DocumentForDetailedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocuments(string userId)
        {
            var documents = await _db.DocumentRepository.GetAsync(d => d.TeacherId == userId, o => o.OrderByDescending(c => c.DateCreated), "Teacher");

            var documentsForDetailed = _mapper.Map<List<DocumentForDetailedDto>>(documents);

            return Ok(documentsForDetailed);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet(ApiV1Routes.Document.GetDocumentsForAdmin)]
        [ProducesResponseType(typeof(DocumentForAdminCompleteDetailedDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocumentsForAdmin(string userId)
        {
            var user = await _db.UserRepository.GetAsync(t => t.Id == userId, "Teacher");
            var teacherForDetailed = _mapper.Map<UserForTeacherDetailedDto>(user);

            var documents = await _db.DocumentRepository.GetAsync(d => d.TeacherId == userId, o => o.OrderByDescending(c => c.DateCreated), "Teacher");
            var documentsForDetailed = _mapper.Map<List<DocumentForAdminDetailedDto>>(documents);

            var model = new DocumentForAdminCompleteDetailedDto()
            {
                Teacher = teacherForDetailed,
                Document = documentsForDetailed
            };

            return Ok(model);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Document.GetDocument, Name = nameof(GetDocument))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(DocumentForDetailedDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocument(string id, string userId)
        {
            var document = await _db.DocumentRepository.GetAsync(d => d.Id == id && d.TeacherId == userId, o => o.OrderByDescending(c => c.DateCreated), string.Empty);

            var documentForDetailed = _mapper.Map<DocumentForDetailedDto>(document);

            return Ok(documentForDetailed);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost(ApiV1Routes.Document.AddDocument)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(DocumentForDetailedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDocument(string userId, [FromForm] DocumentForAddDto dto)
        {
            var teacher = await _db.TeacherRepository.GetAsync(t => t.UserId == userId, string.Empty);
            if (teacher == null)
            {
                return BadRequest("کاربر یافت نشد");
            }
            if (teacher.Status == 0)
            {
                return BadRequest("مدارک کاربر در حال بررسی می باشد. امکان ارسال مدارک وجود ندارد");
            }
            else if (teacher.Status == 1)
            {
                return BadRequest("مدارک کاربر تایید شده است. امکان ارسال مدارک وجود ندارد");
            }

            var uploadResult = await _uploadService.UploadFile(dto.File, string.Format("{0}://{1}{2}", Request.Scheme, Request.Host.Value, Request.PathBase.Value), "profile\\" + userId + "\\documents");

            if (uploadResult.Status)
            {
                var newDocument = new Document()
                {
                    TeacherId = userId,
                    FileUrl = uploadResult.Url,
                    Message = "",
                };

                await _db.DocumentRepository.AddAsync(newDocument);

                teacher.Status = 0;
                _db.TeacherRepository.Update(teacher);

                if (await _db.SaveAsync())
                {
                    var resultDocument = _mapper.Map<DocumentForDetailedDto>(newDocument);
                    return CreatedAtRoute(nameof(GetDocument), new { userId = teacher.UserId, id = newDocument.Id }, resultDocument);
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

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPatch(ApiV1Routes.Document.UpdateDocumentStatus)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDocumentStatus(string userId, DocumentForUpdateStatusDto dto)
        {
            var teacher = await _db.TeacherRepository.GetAsync(t => t.UserId == userId, string.Empty);
            if (teacher != null)
            {
                teacher.Status = dto.Status;
                _db.TeacherRepository.Update(teacher);

                var document = (await _db.DocumentRepository.GetAsync(d => d.TeacherId == userId, null, string.Empty)).LastOrDefault();
                if (document != null)
                {
                    document.Message = dto.Message;
                    _db.DocumentRepository.Update(document);
                }

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
                return BadRequest("کاربر یافت نشد");
            }
        }
    }
}
