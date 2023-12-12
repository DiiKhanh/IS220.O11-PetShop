using AutoMapper;
using Newtonsoft.Json;
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
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Images)));

            CreateMap<DogItem, DogItemResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<string[]>(src.Images)));

            CreateMap<DogItemResponse, DogItem>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Images)));
            CreateMap<Order, OrderDto>();

        }
    }
}
