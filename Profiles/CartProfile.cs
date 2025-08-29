using AutoMapper;
using E_commerceAPI.DTOs;
using E_commerceAPI.Models;

namespace E_commerceAPI.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartDTO, Cart>();
              
        }
    }
}
