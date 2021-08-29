﻿using AutoMapper;
using Banking.Data.Dtos.Common.Pagination;
using ELearn.Common.Extentions;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Comment;
using ELearn.Data.Dtos.Site.Common;
using ELearn.Data.Enums;
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
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;

        public CommentController(IUnitOfWork<DatabaseContext> db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet(ApiV1Routes.Comment.GetComments)]
        public async Task<IActionResult> GetComments(string courseId)
        {
            var comments = await _db.CommentRepository.GetAsync(c => c.CourseId == courseId && c.Status == 1, o => o.OrderByDescending(c => c.DateCreated), "User");
            var commentsDetailed = _mapper.Map<List<CommentForDetailedDto>>(comments);
            return Ok(commentsDetailed);
        }

        [Authorize]
        [HttpGet(ApiV1Routes.Comment.GetCommentsForAdmin)]
        public async Task<IActionResult> GetCommentsForAdmin([FromQuery] PaginationDto pagination)
        {
            var comments = await _db.CommentRepository.GetPagedListAsync(pagination, pagination.Filter.ToCommentExpression(StatusType.All), pagination.SortHeader.ToCommentOrderBy(pagination.SortDirection), "User,Course");
            Response.AddPagination(comments.CurrentPage, comments.PageSize, comments.TotalCount, comments.TotalPages);
            var commentsDetailed = _mapper.Map<List<CommentForAdminDetailedDto>>(comments);
            return Ok(commentsDetailed);
        }

        [Authorize]
        [HttpPost(ApiV1Routes.Comment.AddComment)]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> AddComment(string courseId, string userId, CommentForAddDto dto)
        {
            var course = await _db.CourseRepository.GetAsync(c => c.Id == courseId && c.Status == 1, string.Empty);

            if (course == null)
            {
                return BadRequest("دوره یافت نشد");
            }

            var comment = new Comment()
            {
                CourseId = courseId,
                UserId = userId,
                Text = dto.Text,
                Status = 0
            };
            await _db.CommentRepository.AddAsync(comment);
            await _db.SaveAsync();

            return Ok();
        }

        [Authorize]
        [HttpPatch(ApiV1Routes.Comment.UpdateCommentStatus)]
        public async Task<IActionResult> UpdateCommentStatus(string id, UpdateStatusDto dto)
        {
            var comment = await _db.CommentRepository.GetAsync(id);
            if (comment != null)
            {
                comment.Status = dto.Status;
                _db.CommentRepository.Update(comment);
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
                return BadRequest("کامنت یافت نشد");
            }
        }
    }
}
