using AutoMapper;
using ELearn.Common.Extentions;
using ELearn.Data.Dtos.Site.Comment;
using ELearn.Data.Dtos.Site.Course;
using ELearn.Data.Dtos.Site.Order;
using ELearn.Data.Dtos.Site.Users;
using ELearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Common.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
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

            CreateMap<CourseForAddDto, Course>();
            CreateMap<CourseForUpdateDto, Course>();
            CreateMap<Course, CourseForDetailedDto>()
                .ForMember(dest => dest.DateCreated, opt =>
                {
                    opt.MapFrom(src => src.DateCreated.ToShamsiDate());
                })
                .ForMember(dest => dest.VerifiedAt, opt =>
                {
                    opt.MapFrom(src => src.VerifiedAt.ToShamsiDate());
                })
                .ForMember(dest => dest.TeacherName, opt =>
                {
                    opt.MapFrom(src => src.Teacher.UserName);
                })
                .ForMember(dest => dest.CategoryName, opt =>
                {
                    opt.MapFrom(src => src.Category.Name);
                })
                .ForMember(dest => dest.PrerequisitesTitle, opt =>
                {
                    opt.MapFrom(src => src.Prerequisites != null ? string.Empty : src.Prerequisites.Title);
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
        }
    }
}
