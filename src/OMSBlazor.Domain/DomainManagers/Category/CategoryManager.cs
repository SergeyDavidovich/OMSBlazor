using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OMSBlazor.DomainManagers.Category
{
    public class CategoryManager : DomainService, ICategoryManager
    {
        private readonly IRepository<Northwind.OrderAggregate.Category, int> categoryRepository;

        public CategoryManager(IRepository<Northwind.OrderAggregate.Category, int> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Northwind.OrderAggregate.Category> CreateAsync(string name)
        {
            var categories = await categoryRepository.GetListAsync();
            if (categories.Any(x => x.CategoryName == name))
            {
                throw new CategoryNameDuplicationException();
            }

            var key = categories.Last().Id + 1;

            var category = new Northwind.OrderAggregate.Category(key, name);

            await categoryRepository.InsertAsync(category);

            return category;
        }
    }
}
