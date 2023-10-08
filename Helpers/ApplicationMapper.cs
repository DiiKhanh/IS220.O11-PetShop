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
        }
    }
}
