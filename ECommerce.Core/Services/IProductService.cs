using ECommerce.Core.AutoMapper;
using ECommerce.Core.Dtos;
using ECommerce.Core.Entities;
using ECommerce.Core.Specifications.Product_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services
{
    public interface IProductService
    {
        Task<Pagination<ProductToReturnDto>> GetProductsAsync(ProductSpecParams specParams);
        Task<ProductToReturnDto?> GetProductByIdAsync(int id);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetTypesAsync();
    }
}
