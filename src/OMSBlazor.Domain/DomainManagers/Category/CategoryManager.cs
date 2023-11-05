using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OMSBlazor.DomainManagers.Category
{
    public class CategoryManager : DomainService, ICategoryManager
    {
        private readonly IRepository<Northwind.OrderAggregate.Category, int> _categoryRepository;
        private readonly IRepository<Northwind.OrderAggregate.Product, int> _productRepository;

        public CategoryManager(
            IRepository<Northwind.OrderAggregate.Category, int> categoryRepository, 
            IRepository<Northwind.OrderAggregate.Product, int> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<Northwind.OrderAggregate.Category> CreateAsync(string name)
        {
            var categories = await _categoryRepository.GetListAsync();
            if (categories.Any(x => x.CategoryName == name))
            {
                throw new CategoryNameDuplicationException();
            }

            var key = categories.Last().Id + 1;

            var category = new Northwind.OrderAggregate.Category(key, name);

            return category;
        }

        public async Task ThrowIfCannotDeleteAsync(int id)
        {
            if (!(await _categoryRepository.AnyAsync(x => x.Id == id)))
            {
                throw new EntityNotFoundException(typeof(Northwind.OrderAggregate.Category), id);
            }

            var dependentProduct = _productRepository.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (await _productRepository.AnyAsync(x => x.CategoryId == id))
            {
                throw new DependentProductExistException(id, dependentProduct.Id);
            }
        }

        public async Task<Northwind.OrderAggregate.Category> UpdateNameAsync(int id, string name)
        {
            if (!(await _categoryRepository.AnyAsync(x => x.Id == id)))
            {
                throw new EntityNotFoundException(typeof(Northwind.OrderAggregate.Category), id);
            }

            var category = await _categoryRepository.SingleAsync(x => x.Id == id);

            if (await _categoryRepository.AnyAsync(x=>x.CategoryName==name && x.Id != id))
            {
                throw new CategoryNameDuplicationException();
            }

            category.SetCategoryName(name);

            return category;
        }
    }
}
