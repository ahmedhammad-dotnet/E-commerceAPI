using AutoMapper;
using E_commerceAPI.DTOs;
using E_commerceAPI.Models;

namespace E_commerceAPI.Profiles
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(sourse => sourse.Category.Name));
        }
    }
}
