using AutoMapper;
using E_commerceAPI.DTOs;
using E_commerceAPI.Models;

namespace E_commerceAPI.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUserDTOs, ApplicationUser>()
                .ForMember(dest => dest.UserName, option => option.MapFrom(sourse => sourse.name));
        }
    }
}
