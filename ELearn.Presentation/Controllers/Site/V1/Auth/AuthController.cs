using AutoMapper;
using ELearn.Common.Helpers.Interface;
using ELearn.Data.Common.ReturnMessages;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Tokens;
using ELearn.Data.Dtos.Site.Users;
using ELearn.Data.Models;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Auth
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;
        private readonly IUtilities _utilities;
        private readonly UserManager<User> _userManager;

        public AuthController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IUtilities utilities, UserManager<User> userManager)
        {
            _db = db;
            _mapper = mapper;
            _utilities = utilities;
            _userManager = userManager;
        }

        [HttpPost(ApiV1Routes.Auth.RegisterStudent)]
        public async Task<IActionResult> RegisterStudent(UserForRegisterStudentDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.PhotoUrl = "https://res.cloudinary.com/mahsad/image/upload/v1598432094/profile.png";

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var student = new Student()
                {
                    Grade = dto.Grade,
                    Major = dto.Major,
                    UserId = user.Id
                };
                await _db.StudentRepository.AddAsync(student);
                await _db.SaveAsync();

                var userAddRole = await _userManager.FindByNameAsync(user.UserName);
                await _userManager.AddToRolesAsync(userAddRole, new[] { "Student" });

                var resultUser = await _db.UserRepository.GetAsync(expression: u => u.Id == userAddRole.Id, includeEntity: "Student");
                var registeredUser = _mapper.Map<UserForStudentDetailedDto>(resultUser);
                return CreatedAtRoute("GetStudent", new { controller = "Student", id = userAddRole.Id }, registeredUser);
            }
            else if (result.Errors.Any())
            {
                var returnMessage = new ErrorList();
                foreach (var error in result.Errors)
                {
                    returnMessage.Errors.Add(error.Description);
                }
                return BadRequest(returnMessage);
            }
            else
            {
                return BadRequest("خطا در ثبت نام");
            }
        }

        [HttpPost(ApiV1Routes.Auth.RegisterTeacher)]
        public async Task<IActionResult> RegisterTeacher(UserForRegisterTeacherDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.PhotoUrl = "https://res.cloudinary.com/mahsad/image/upload/v1598432094/profile.png";

            PersianCalendar pc = new PersianCalendar();
            DateTime birthdate = new DateTime();
            try
            {
                birthdate = pc.ToDateTime(dto.BirthYear, dto.BirthMonth, dto.BirthDay, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                return BadRequest("تاریخ تولد معتیر نیست");
            }

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var teacher = new Teacher()
                {
                    BirthDate = birthdate,
                    Degree = dto.Degree,
                    Specialty = dto.Specialty,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    Description = dto.Description,
                    Status = 0,
                    UserId = user.Id
                };
                await _db.TeacherRepository.AddAsync(teacher);
                await _db.SaveAsync();

                var userAddRole = await _userManager.FindByNameAsync(user.UserName);
                await _userManager.AddToRolesAsync(userAddRole, new[] { "Teacher" });

                var resultUser = await _db.UserRepository.GetAsync(expression: s => s.Id == userAddRole.Id, includeEntity: "Teacher");
                var registeredUser = _mapper.Map<UserForTeacherDetailedDto>(resultUser);
                return CreatedAtRoute("GetTeacher", new { controller = "Teacher", id = userAddRole.Id }, registeredUser);
            }
            else if (result.Errors.Any())
            {
                var returnMessage = new ErrorList();
                foreach (var error in result.Errors)
                {
                    returnMessage.Errors.Add(error.Description);
                }
                return BadRequest(returnMessage);
            }
            else
            {
                return BadRequest("خطا در ثبت نام");
            }
        }

        [HttpPost(ApiV1Routes.Auth.Login)]
        public async Task<IActionResult> Login(TokenForRequestDto tokenForRequestDto)
        {
            switch (tokenForRequestDto.GrantType)
            {
                case "password":
                    var result = await _utilities.GenerateNewTokenAsync(tokenForRequestDto);
                    if (result.Status)
                    {
                        var user = _mapper.Map<UserForDetailedDto>(result.User);

                        var authUser = await _userManager.FindByNameAsync(tokenForRequestDto.UserName);
                        var roles = await _userManager.GetRolesAsync(authUser);

                        return Ok(new TokenForLoginResponseDto()
                        {
                            Token = result.Token,
                            RefreshToken = result.RefreshToken,
                            Roles = roles,
                            User = user
                        });
                    }
                    else
                    {
                        return Unauthorized("خطا در ورود");
                    }
                case "refresh_token":
                    var res = await _utilities.RefreshAccessTokenAsync(tokenForRequestDto);
                    if (res.Status)
                    {
                        return Ok(res);
                    }
                    else
                    {
                        return Unauthorized("خطا در اعتبار سنجی مجدد");
                    }
                default:
                    return Unauthorized("خطا در اعتبارسنجی");
            }
        }
    }
}
