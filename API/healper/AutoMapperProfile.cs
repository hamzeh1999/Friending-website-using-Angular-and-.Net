using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.entities;
using API.Extensions;
using AutoMapper;

namespace API.healper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<PhotoDTO, Photo>();
            CreateMap<Photo, PhotoDTO>();
            CreateMap<updateMemberDTO,AppUser>();
            CreateMap<AppUser, MemberDTO>()
            .ForMember(dest => dest.photoUrl, opt => opt.MapFrom(src => src.photos.FirstOrDefault(x =>x.isMan).url))
            .ForMember(dist=>dist.age,opt=>opt.MapFrom(src=>src.dateOfBirth.calculateAge()));
            // CreateMap<MemberDTO, AppUser>();
            CreateMap<AppUser,RegisterDto>();

            CreateMap<RegisterDto,AppUser>();
        }

        //  int getAge(DateTime dob)
        // {
        //     var today=DateTime.Today;
        //     var age=today.Year-dob.Year;
        //     if(dob.Date>today.AddYears(-age)) age--;
        //     return age; 
        // }
    }
}