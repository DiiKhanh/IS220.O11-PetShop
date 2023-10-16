using AutoMapper;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<DogItemDto,DogItem>();
            CreateMap<DogItem, DogItemDto>();
            CreateMap<DogProductItemResponse, DogProductItem>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<DogProductItem, DogProductItemResponse>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
