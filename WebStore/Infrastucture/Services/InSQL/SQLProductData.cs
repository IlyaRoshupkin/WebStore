using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entityes;
using WebStore.Infrastucture.Interfaces;

namespace WebStore.Infrastucture.Services.InSQL
{
    public class SQLProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SQLProductData(WebStoreDB db) => _db = db;

        public Brand GetBrandById(int id) => _db.Brands
            .Include(brand => brand.Products)
            .FirstOrDefault(b => b.Id == id);

        public IEnumerable<Brand> GetBrands() => _db.Brands.Include(brand => brand.Products);

        public Product GetProductById(int id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == id);

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p=>p.Brand)
                .Include(p=>p.Section);
            if(Filter?.Ids?.Length > 0)
            {
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            }
            else
            {
                if (Filter?.BrandId != null)
                    query = query.Where(product => product.BrandId == Filter.BrandId);

                if (Filter?.SectionId != null)
                    query = query.Where(product => product.SectionId == Filter.SectionId);
            }
            return query;
        }

        //public Section GetSectionById(int id) => _db.Sections.Find(id);

        public Section GetSectionById(int id) => _db.Sections
            .Include(section => section.Products)
            .FirstOrDefault(s => s.Id == id);

        public IEnumerable<Section> GetSections() => _db.Sections.Include(section => section.Products);
    }
}
