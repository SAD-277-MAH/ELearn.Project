using AutoMapper;
using ELearn.Data.Dtos.Site.Users;
using ELearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Common.Extentions
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
        }
    }
}
