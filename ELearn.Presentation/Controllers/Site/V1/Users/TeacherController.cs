using AutoMapper;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Users;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Users
{
    [Authorize(Policy = "RequireTeacherRole")]
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

        [HttpGet(ApiV1Routes.Teacher.GetTeacher, Name = nameof(GetTeacher))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
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
