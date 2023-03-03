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
            CreateMap<updateMemberDTO, AppUser>();
            CreateMap<AppUser, MemberDTO>()
            .ForMember(dest => dest.photoUrl, opt => opt.MapFrom(src => src.photos.FirstOrDefault(x => x.isMan).url))
            .ForMember(dist => dist.age, opt => opt.MapFrom(src => src.dateOfBirth.calculateAge()));
            // CreateMap<MemberDTO, AppUser>();
            CreateMap<AppUser, RegisterDto>();

            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDTO>()
            .ForMember(dis => dis.senderPhotoUrl, 
            opt => opt.MapFrom(src => src.sender.photos.FirstOrDefault(x => x.isMan).url))
            .ForMember(dis => dis.recipientPhotoUrl, 
            opt => opt.MapFrom(src => src.recipient.photos.FirstOrDefault(x => x.isMan).url));
             CreateMap<MessageDTO, Message>();
            // CreateMap<DateTime,DateTime>().
            // ConvertUsing(d=>DateTime.SpecifyKind(d,DateTimeKind.Utc));
    
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