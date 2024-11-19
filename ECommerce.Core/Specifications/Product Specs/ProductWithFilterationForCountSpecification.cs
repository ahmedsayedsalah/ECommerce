using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Specifications.Product_Specs
{
    public class ProductWithFilterationForCountSpecification: BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
            : base(p=>
                       (!specParams.BrandId.HasValue || p.ProductBrandId==specParams.BrandId)&&
                       (!specParams.TypeId.HasValue || p.ProductTypeId == specParams.TypeId)
                  )
        {
            
        }
    }
}
