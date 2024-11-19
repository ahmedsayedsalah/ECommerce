using AutoMapper;
using ECommerce.Core.Dtos;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order_Aggregate;
using IdentityAddress= ECommerce.Core.Entities.Identity.Address;
using OrderAddress= ECommerce.Core.Entities.Order_Aggregate.Address;

namespace ECommerce.Core.AutoMapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ProductToReturnDto>()
                .ForMember(d=> d.ProductBrand,o=> o.MapFrom(s=> s.ProductBrand.Name))
                .ForMember(d=> d.ProductType,o=> o.MapFrom(s=> s.ProductType.Name))
                .ForMember(d=> d.PictureUrl,o=> o.MapFrom<ProductPictureUrlResolver>())
                .ReverseMap();

            CreateMap<IdentityAddress, AddressDto>().ReverseMap();

            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d=> d.DeliveryMethod, o=> o.MapFrom(s=> s.DeliveryMethod.ShortName))
                .ForMember(d=> d.DeliveryMethodCost, o=> o.MapFrom(s=> s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
