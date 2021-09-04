using AutoMapper;
using ELearn.Common.Filters;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Users;
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
    [Authorize(Policy = "RequireStudentRole")]
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;

        public StudentController(IUnitOfWork<DatabaseContext> db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet(ApiV1Routes.Student.GetStudent, Name = nameof(GetStudent))]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [ProducesResponseType(typeof(UserForStudentDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudent(string id)
        {
            var user = await _db.UserRepository.GetAsync(expression: u => u.Id == id, includeEntity: "Student");

            if (user == null || user.Student == null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<UserForStudentDetailedDto>(user);

            return Ok(mappedUser);
        }
    }
}
