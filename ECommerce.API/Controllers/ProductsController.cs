using AutoMapper;
using ECommerce.API.Errors;
using ECommerce.API.Helpers;
using ECommerce.Core.AutoMapper;
using ECommerce.Core.Dtos;
using ECommerce.Core.Entities;
using ECommerce.Core.Services;
using ECommerce.Core.Specifications.Product_Specs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
        //[Authorize/*(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)*/]
        [HttpGet]
        [Cached(6000)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams)
            => Ok(await productService.GetProductsAsync(specParams));
        
        [HttpGet("{id}")]
        [Cached(6000)]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
             var product = await productService.GetProductByIdAsync(id);

             if (product is null) return NotFound(new ApiResponse(400));

             return Ok(product);
        }

        [HttpGet("brands")]
        [Cached(6000)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
            => Ok(await productService.GetBrandsAsync());

        [HttpGet("types")]
        [Cached(6000)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
            => Ok(await productService.GetTypesAsync());
    }
}
