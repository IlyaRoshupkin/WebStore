using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entityes;
using WebStore.Infrastucture.Interfaces;

namespace WebStore.Infrastucture.Services
{
    [Obsolete("This class is obsolete because there is no need to place data in a memory. Use the class SqlProductData", true)]
    public class InMemoryProductData : IProductData
    {
        public Brand GetBrandById(int id) => throw new NotSupportedException();

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Product> GetProductById(int id) => throw new NotSupportedException ();

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            var query = TestData.Products;

            if (Filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);
            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);
            return query;
        }

        public Section GetSectionById(int id) => throw new NotSupportedException();

        public IEnumerable<Section> GetSections() => TestData.Sections;

        Product IProductData.GetProductById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
