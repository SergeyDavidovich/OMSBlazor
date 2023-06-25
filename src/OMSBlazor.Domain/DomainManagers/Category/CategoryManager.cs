using OMSBlazor.Northwind.ProductAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OMSBlazor.DomainManagers.Category
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IRepository<Northwind.ProductAggregate.Category, int> categoryRepository;

        public CategoryManager(IRepository<Northwind.ProductAggregate.Category, int> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Northwind.ProductAggregate.Category> CreateAsync(string name)
        {
            var categories = await categoryRepository.GetListAsync();
            if (categories.Any(x => x.CategoryName == name))
            {
                throw new CategoryNameDuplication();
            }

            var key = categories.Last().Id + 1;

            var category = new Northwind.ProductAggregate.Category(key, name);
            return category;
        }
    }
}
