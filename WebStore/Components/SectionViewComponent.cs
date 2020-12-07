using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastucture.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    //[ViewComponent(Name="Sections")]
    public class SectionViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionViewComponent(IProductData ProductData) => _ProductData = ProductData;
        public IViewComponentResult Invoke()
        {
            var sections = _ProductData.GetSections().ToArray();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_section_views = parent_sections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order
                })
            .ToList();

            foreach(var parent_section in parent_section_views)
            {
                var child = sections.Where(s => s.ParentId == parent_section.Id);
                foreach(var child_section in child)
                {
                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        ParentSection = parent_section
                    });
                    parent_section.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
                }
            }
            parent_section_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return View(parent_section_views);
        }
        //public async Task<IViewComponentResult> InvokeAsync() => View();
    }
}
