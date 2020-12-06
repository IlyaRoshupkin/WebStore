using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain.Entityes;
using WebStore.Infrastucture.Interfaces;

namespace WebStore.Infrastucture.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;
    }
}
