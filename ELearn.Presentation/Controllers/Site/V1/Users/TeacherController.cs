using AutoMapper;
using Banking.Data.Dtos.Common.Pagination;
using ELearn.Common.Extentions;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Users;
using ELearn.Data.Enums;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class TeacherController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;

        public TeacherController(IUnitOfWork<DatabaseContext> db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet(ApiV1Routes.Teacher.GetTeachersForAdmin)]
        [ProducesResponseType(typeof(List<TeacherForAdminDetailedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeachersForAdmin([FromQuery] PaginationDto pagination, [FromQuery] int? status)
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

            var teachers = await _db.TeacherRepository.GetPagedListAsync(pagination, pagination.Filter.ToTeacherExpression(statusType), pagination.SortHeader.ToTeacherOrderBy(pagination.SortDirection), "User");
            Response.AddPagination(teachers.CurrentPage, teachers.PageSize, teachers.TotalCount, teachers.TotalPages);
            
            var teachersForDetailed = _mapper.Map<List<TeacherForAdminDetailedDto>>(teachers);

            return Ok(teachersForDetailed);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet(ApiV1Routes.Teacher.GetTeacher, Name = nameof(GetTeacher))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(UserForTeacherDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeacher(string id)
        {
            var user = await _db.UserRepository.GetAsync(expression: u => u.Id == id, includeEntity: "Teacher");

            if (user == null || user.Teacher == null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<UserForTeacherDetailedDto>(user);

            return Ok(mappedUser);
        }
    }
}
