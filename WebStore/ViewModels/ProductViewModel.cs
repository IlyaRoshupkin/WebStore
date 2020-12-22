﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entityes.Base.Interfaces;

namespace WebStore.ViewModels
{
    public class ProductViewModel : INamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
    }
    public class CatalogViewModel
    {
         public IEnumerable<ProductViewModel> Products { get; set; }
        public int? SectionId { get; set;  }
        public int? BrandId { get; set; }
    }
}
