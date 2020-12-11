﻿using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Brand> GetBrands() => _db.Brands.Include(brand => brand.Products);

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products;
            if(Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            return query;
        }

        public IEnumerable<Section> GetSections() => _db.Sections.Include(section => section.Products);
    }
}
