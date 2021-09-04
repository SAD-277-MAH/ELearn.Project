using AutoMapper;
using ELearn.Common.Helpers.Interface;
using ELearn.Data.Common.ReturnMessages;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Services;
using ELearn.Data.Dtos.Site.Tokens;
using ELearn.Data.Dtos.Site.Users;
using ELearn.Data.Models;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Site.Interface;
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
        private readonly IMessageSender _messageSender;
        private readonly IViewRenderService _viewRenderService;

        public AuthController(IUnitOfWork<DatabaseContext> db, IMapper mapper, IUtilities utilities, UserManager<User> userManager, IMessageSender messageSender, IViewRenderService viewRenderService)
        {
            _db = db;
            _mapper = mapper;
            _utilities = utilities;
            _userManager = userManager;
            _messageSender = messageSender;
            _viewRenderService = viewRenderService;
        }

        [HttpPost(ApiV1Routes.Auth.RegisterStudent)]
        [ProducesResponseType(typeof(UserForStudentDetailedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorList), StatusCodes.Status400BadRequest)]
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

                // Send SMS
                #region Send SMS
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(userAddRole, userAddRole.UserName);
                _messageSender.SendSms(userAddRole.UserName, "ثبت نام شما با موفقیت انجام شد." + Environment.NewLine + "کد فعالسازی: " + code);
                #endregion

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
                var returnMessage = new ErrorList();
                returnMessage.Errors.Add("خطا در ثبت نام");
                return BadRequest(returnMessage);
            }
        }

        [HttpPost(ApiV1Routes.Auth.RegisterTeacher)]
        [ProducesResponseType(typeof(UserForTeacherDetailedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorList), StatusCodes.Status400BadRequest)]
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
            catch
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

                // Send SMS and Email
                #region Send Email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userAddRole);
                var activateEmailModel = new EmailUrlDto()
                {
                    Url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + Url.Action(nameof(ActivateEmail), new { UserName = userAddRole.UserName, Token = token })
                };
                string emailBody = _viewRenderService.RenderToString("_ActivateEmail", activateEmailModel);
                _messageSender.SendEmail(user.Email, "فعالسازی حساب کاربری", emailBody);
                #endregion
                #region Send SMS
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(userAddRole, userAddRole.UserName);
                _messageSender.SendSms(userAddRole.UserName, "ثبت نام شما با موفقیت انجام شد." + Environment.NewLine + "کد فعالسازی: " + code);
                #endregion

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
                var returnMessage = new ErrorList();
                returnMessage.Errors.Add("خطا در ثبت نام");
                return BadRequest(returnMessage);
            }
        }

        [HttpPost(ApiV1Routes.Auth.Login)]
        [ProducesResponseType(typeof(TokenForLoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(TokenForRequestDto tokenForRequestDto)
        {
            switch (tokenForRequestDto.GrantType)
            {
                case "password":
                    var result = await _utilities.GenerateNewTokenAsync(tokenForRequestDto);
                    if (result.Status)
                    {
                        if (await _userManager.IsInRoleAsync(result.User, "Teacher"))
                        {
                            if (!result.User.PhoneNumberConfirmed)
                            {
                                return BadRequest("شماره تلفن تأیید نشده است");
                            }
                            if (!result.User.EmailConfirmed)
                            {
                                return BadRequest("ایمیل تأیید نشده است");
                            }
                        }
                        else if (await _userManager.IsInRoleAsync(result.User, "Student"))
                        {
                            if (!result.User.PhoneNumberConfirmed)
                            {
                                return BadRequest("شماره تلفن تأیید نشده است");
                            }
                        }

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
                        var user = _mapper.Map<UserForDetailedDto>(res.User);

                        var authUser = await _userManager.FindByNameAsync(tokenForRequestDto.UserName);
                        var roles = await _userManager.GetRolesAsync(authUser);

                        return Ok(new TokenForLoginResponseDto()
                        {
                            Token = res.Token,
                            RefreshToken = res.RefreshToken,
                            Roles = roles,
                            User = user
                        });
                    }
                    else
                    {
                        return Unauthorized("خطا در اعتبار سنجی مجدد");
                    }
                default:
                    return Unauthorized("خطا در اعتبارسنجی");
            }
        }

        [HttpGet(ApiV1Routes.Auth.ResendActivationEmail)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResendActivationEmail([FromQuery] string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return BadRequest("اطلاعات اشتباه است");
            }

            if (await _userManager.IsInRoleAsync(user, "Teacher"))
            {
                if (user.EmailConfirmed)
                {
                    return BadRequest("ایمیل قبلا تأیید شده است");
                }

                #region Send Email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var activateEmailModel = new EmailUrlDto()
                {
                    Url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + Url.Action(nameof(ActivateEmail), new { UserName = user.UserName, Token = token })
                };
                string emailBody = _viewRenderService.RenderToString("_ActivateEmail", activateEmailModel);
                _messageSender.SendEmail(user.Email, "فعالسازی حساب کاربری", emailBody);
                #endregion

                return Ok();
            }
            else
            {
                return BadRequest("فقط اساتید نیاز به تأیید ایمبل دارند");
            }
        }

        [HttpGet(ApiV1Routes.Auth.ResendActivationPhoneNumber)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResendActivationPhoneNumber([FromQuery] string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return BadRequest("اطلاعات اشتباه است");
            }

            if (await _userManager.IsInRoleAsync(user, "Teacher") || await _userManager.IsInRoleAsync(user, "Student"))
            {
                if (user.PhoneNumberConfirmed)
                {
                    return BadRequest("شماره تلفن قبلا تأیید شده است");
                }

                #region Send SMS
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.UserName);
                _messageSender.SendSms(user.UserName, "کد فعالسازی: " + code);
                #endregion

                return Ok();
            }
            else
            {
                return BadRequest("فقط کاربران نیاز به تأیید شماره تلفن دارند");
            }
        }

        [HttpGet(ApiV1Routes.Auth.ActivateEmail)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivateEmail([FromQuery] string UserName, [FromQuery] string Token)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Token))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(UserName);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, Token);
                if (result.Succeeded)
                {
                    return Redirect("http://localhost:3000");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet(ApiV1Routes.Auth.ActivatePhoneNumber)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivatePhoneNumber([FromQuery] string UserName, [FromQuery] string Token)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Token))
            {
                return BadRequest("اطلاعات اشتباه است");
            }

            var user = await _userManager.FindByNameAsync(UserName);
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, UserName, Token);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("اطلاعات اشتباه است");
                }
            }
            else
            {
                return BadRequest("اطلاعات اشتباه است");
            }
        }

        [HttpGet(ApiV1Routes.Auth.ForgetPassword)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgetPassword([FromQuery] string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return BadRequest("اطلاعات اشتباه است");
            }

            #region Send SMS
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            _messageSender.SendSms(user.UserName, "کد تغییر رمز عبور: " + code);
            #endregion

            return Ok();
        }

        [HttpPost(ApiV1Routes.Auth.ResetPassword)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(UserForResetPasswordDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("اطلاعات اشتباه است");
                }
            }
            else
            {
                return BadRequest("اطلاعات اشتباه است");
            }

        }
    }
}
