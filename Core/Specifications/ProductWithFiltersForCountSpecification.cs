using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParamas) : base(x =>
         (string.IsNullOrEmpty(productParamas.Search) || x.Name.ToLower().Contains(productParamas.Search)) &&
         (!productParamas.BrandId.HasValue || x.ProductBrandId == productParamas.BrandId) &&
        (!productParamas.TypeId.HasValue || x.ProductTypeId == productParamas.TypeId))
        {
        }
    }
}