using AutoMapper;
using ECommerce.Core.Dtos;
using ECommerce.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Core.AutoMapper
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{configuration["ApiUrl"]}{source.PictureUrl}";

            return string.Empty;
        }
    }
}
