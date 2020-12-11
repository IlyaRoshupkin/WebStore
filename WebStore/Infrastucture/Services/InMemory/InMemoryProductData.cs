﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entityes;
using WebStore.Infrastucture.Interfaces;

namespace WebStore.Infrastucture.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            var query = TestData.Products;

            if (Filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);
            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);
            return query;
        }

        public IEnumerable<Section> GetSections() => TestData.Sections;
    }
}
