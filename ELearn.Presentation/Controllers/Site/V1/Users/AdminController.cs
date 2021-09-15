using AutoMapper;
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
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;

        public AdminController(IUnitOfWork<DatabaseContext> db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireSystemOrAdminRole")]
        [HttpGet(ApiV1Routes.Admin.GetAdmin, Name = nameof(GetAdmin))]
        [ProducesResponseType(typeof(UserForAdminDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdmin(string id)
        {
            var user = await _db.UserRepository.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<UserForAdminDetailedDto>(user);

            return Ok(mappedUser);
        }
    }
}
