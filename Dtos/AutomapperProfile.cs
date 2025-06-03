using AutoMapper;
using ProjectWorkAPI.Dtos;
using ProjectWorkAPI.Models;


public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Qty))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));

        CreateMap<Order, OrderPrepDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Qty))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.TableId));

        CreateMap<Order, OrderStatsDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Qty))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product.Image));

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.IdCategoryNavigation.Name));

        CreateMap<Category, CategoryDto>();

        CreateMap<ProductPrepStation, ProductPrepStationDto>();
    }
}