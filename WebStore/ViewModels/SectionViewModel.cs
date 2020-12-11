using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entityes.Base.Interfaces;

namespace WebStore.ViewModels
{
    public class SectionViewModel:INamedEntity,IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<SectionViewModel> ChildSections { get; set; } = new();
        public SectionViewModel ParentSection { get; set; }
        public int ProductsCount { get; set; }
    }
}
