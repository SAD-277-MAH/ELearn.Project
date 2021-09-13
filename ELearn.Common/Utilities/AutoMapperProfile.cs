using AutoMapper;
using ELearn.Common.Extentions;
using ELearn.Data.Dtos.Site.Comment;
using ELearn.Data.Dtos.Site.Course;
using ELearn.Data.Dtos.Site.Document;
using ELearn.Data.Dtos.Site.Exam;
using ELearn.Data.Dtos.Site.ExamQuestion;
using ELearn.Data.Dtos.Site.Order;
using ELearn.Data.Dtos.Site.Setting;
using ELearn.Data.Dtos.Site.Users;
using ELearn.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELearn.Common.Utilities
{
    public class AutoMapperProfile : Profile
    {
        private readonly IHttpContextAccessor _context;

        public AutoMapperProfile(IHttpContextAccessor context)
        {
            _context = context;

            CreateMap<UserForRegisterStudentDto, User>()
                .ForMember(dest => dest.PhoneNumber, opt =>
                {
                    opt.MapFrom(src => src.UserName);
                });
            CreateMap<UserForRegisterTeacherDto, User>()
                .ForMember(dest => dest.PhoneNumber, opt =>
                {
                    opt.MapFrom(src => src.UserName);
                });
            CreateMap<User, UserForDetailedDto>();
            CreateMap<User, UserForStudentDetailedDto>()
                .ForMember(dest => dest.Grade, opt =>
                {
                    opt.MapFrom(src => src.Student.Grade);
                })
                .ForMember(dest => dest.Major, opt =>
                {
                    opt.MapFrom(src => src.Student.Major);
                });
            CreateMap<User, UserForTeacherDetailedDto>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.Teacher.BirthDate.ToAge());
                })
                .ForMember(dest => dest.Degree, opt =>
                {
                    opt.MapFrom(src => src.Teacher.Degree);
                })
                .ForMember(dest => dest.Phone, opt =>
                {
                    opt.MapFrom(src => src.Teacher.Phone);
                })
                .ForMember(dest => dest.Address, opt =>
                {
                    opt.MapFrom(src => src.Teacher.Address);
                })
                .ForMember(dest => dest.Description, opt =>
                {
                    opt.MapFrom(src => src.Teacher.Description);
                })
                .ForMember(dest => dest.Status, opt =>
                {
                    opt.MapFrom(src => src.Teacher.Status);
                });

            CreateMap<Teacher, TeacherForAdminDetailedDto>()
                .ForMember(dest => dest.UserName, opt =>
                {
                    opt.MapFrom(src => src.User.UserName);
                })
                .ForMember(dest => dest.FirstName, opt =>
                {
                    opt.MapFrom(src => src.User.FirstName);
                })
                .ForMember(dest => dest.LastName, opt =>
                {
                    opt.MapFrom(src => src.User.LastName);
                })
                .ForMember(dest => dest.NationalCode, opt =>
                {
                    opt.MapFrom(src => src.User.NationalCode);
                })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.User.PhotoUrl);
                })
                .ForMember(dest => dest.RegisterDate, opt =>
                {
                    opt.MapFrom(src => src.User.RegisterDate);
                });

            CreateMap<CourseForAddDto, Course>();
            CreateMap<CourseForUpdateDto, Course>();
            CreateMap<Course, CourseForAdminDetailedDto>()
                .ForMember(dest => dest.TeacherName, opt =>
                {
                    opt.MapFrom(src => src.Teacher.FirstName + " " + src.Teacher.LastName);
                })
                .ForMember(dest => dest.SessionCount, opt =>
                {
                    opt.MapFrom(src => src.Sessions.Count);
                });
            CreateMap<Course, CourseForTeacherDetailedDto>()
                .ForMember(dest => dest.SessionCount, opt =>
                {
                    opt.MapFrom(src => src.Sessions.Count);
                });
            CreateMap<Course, CourseForStudentDetailedDto>()
                .ForMember(dest => dest.SessionCount, opt =>
                {
                    opt.MapFrom(src => src.Sessions.Count);
                });
            CreateMap<Course, CourseForDetailedDto>()
                .ForMember(dest => dest.TeacherName, opt =>
                {
                    opt.MapFrom(src => src.Teacher.FirstName + " " + src.Teacher.LastName);
                });
            CreateMap<Course, CourseForSiteDetailedDto>()
                .ForMember(dest => dest.TeacherName, opt =>
                {
                    opt.MapFrom(src => src.Teacher.FirstName + " " + src.Teacher.LastName);
                })
                .ForMember(dest => dest.SessionCount, opt =>
                {
                    opt.MapFrom(src => src.Sessions.Count);
                });

            CreateMap<SessionForAddDto, Session>()
                .ForMember(dest => dest.Time, opt =>
                {
                    opt.MapFrom(src => TimeSpan.Parse(src.Time));
                });
            CreateMap<SessionForUpdateDto, Session>()
                .ForMember(dest => dest.Time, opt =>
                {
                    opt.MapFrom(src => TimeSpan.Parse(src.Time));
                });
            CreateMap<Session, SessionForDetailedDto>()
                .ForMember(dest => dest.Time, opt =>
                {
                    opt.MapFrom(src => src.Time.ToString(@"hh\:mm"));
                })
                .ForMember(dest => dest.VideoUrl, opt =>
                {
                    opt.MapFrom(src => $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host.Value}{_context.HttpContext.Request.PathBase.Value}/StreamFile/{src.Id}");
                })
                .ForMember(dest => dest.FileUrl, opt =>
                {
                    opt.MapFrom(src => $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host.Value}{_context.HttpContext.Request.PathBase.Value}/DownloadFile/{src.Id}");
                });

            CreateMap<Order, OrderForDetailedDto>();
            CreateMap<OrderDetail, OrderDetailForDetailedDto>()
                .ForMember(dest => dest.Title, opt =>
                {
                    opt.MapFrom(src => src.Course.Title);
                })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Course.PhotoUrl);
                });

            CreateMap<Comment, CommentForDetailedDto>()
                .ForMember(dest => dest.FirstName, opt =>
                {
                    opt.MapFrom(src => src.User.FirstName);
                })
                .ForMember(dest => dest.LastName, opt =>
                {
                    opt.MapFrom(src => src.User.LastName);
                })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.User.PhotoUrl);
                })
                .ForMember(dest => dest.Date, opt =>
                {
                    opt.MapFrom(src => src.DateCreated.ToShamsiDateTime());
                });
            CreateMap<Comment, CommentForAdminDetailedDto>()
                .ForMember(dest => dest.FirstName, opt =>
                {
                    opt.MapFrom(src => src.User.FirstName);
                })
                .ForMember(dest => dest.LastName, opt =>
                {
                    opt.MapFrom(src => src.User.LastName);
                })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.User.PhotoUrl);
                })
                .ForMember(dest => dest.CourseId, opt =>
                {
                    opt.MapFrom(src => src.Course.Id);
                })
                .ForMember(dest => dest.CourseTitle, opt =>
                {
                    opt.MapFrom(src => src.Course.Title);
                })
                .ForMember(dest => dest.Date, opt =>
                {
                    opt.MapFrom(src => src.DateCreated.ToShamsiDateTime());
                });

            CreateMap<Document, DocumentForDetailedDto>()
                .ForMember(dest => dest.Status, opt =>
                {
                    opt.MapFrom(src => src.Teacher.Status);
                });
            CreateMap<Document, DocumentForAdminDetailedDto>()
                .ForMember(dest => dest.Status, opt =>
                {
                    opt.MapFrom(src => src.Teacher.Status);
                });

            CreateMap<Setting, SettingForSiteDto>();
            CreateMap<SettingForSiteDto, Setting>();
            CreateMap<Setting, SettingForMessageSenderDto>();
            CreateMap<SettingForMessageSenderDto, Setting>();

            CreateMap<ExamForAddDto, Exam>();
            CreateMap<ExamForUpdateDto, Exam>();
            CreateMap<Exam, ExamForDetailedDto>();

            CreateMap<ExamQuestionForAddDto, ExamQuestion>();
            CreateMap<ExamQuestionForUpdateDto, ExamQuestion>();
            CreateMap<ExamQuestion, ExamQuestionForDetailedDto>();
        }
    }
}
