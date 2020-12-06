using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entityes;

namespace WebStore.Infrastucture.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();
        IEnumerable<Brand> GetBrands();
    }
}
