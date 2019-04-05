using AutoMapper;
using BasketApi.Common.Contracts;
using BasketApp.Api.Data.Entities;

namespace BasketApp.Api.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.ProductId, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, source => source.MapFrom(src => src.Price));
        }
    }
}
