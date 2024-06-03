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
            CreateMap<CheckoutDto, Checkout>();
            CreateMap<Checkout, CheckoutDto>();

            CreateMap<VoucherDto, Voucher>();
            CreateMap<Voucher, VoucherDto>();

            CreateMap<AppointmentDto, Appointment>();
            CreateMap<Appointment, AppointmentDto>();

            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentDto>();

            CreateMap<GoodsDto, Goods>();
            CreateMap<Goods, GoodsDto>();
        }
    }
}
