using OMSBlazor.DomainManagers.Category;
using OMSBlazor.Dto.Category;
using OMSBlazor.Interfaces;
using OMSBlazor.Northwind.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OMSBlazor.ApplicationServices
{
    public class CategoryApplicationService : ApplicationService, ICategoryApplicationService
    {
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly ICategoryManager _categoryManager;

        public CategoryApplicationService(IRepository<Category, int> categoryRepository, ICategoryManager categoryManager)
        {
            _categoryRepository = categoryRepository;
            _categoryManager = categoryManager;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = await _categoryManager.CreateAsync(categoryDto.CategoryName);

            category.Description = categoryDto.Description;
            category.Picture = categoryDto.Picture;

            await _categoryRepository.InsertAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryManager.ThrowIfCannotDeleteAsync(id);

            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetListAsync();

            var categoriesDto = ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);

            return categoriesDto;
        }

        public async Task<CategoryDto> GetCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            var categoryDto = ObjectMapper.Map<Category, CategoryDto>(category);

            return categoryDto;
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto)
        {
            var category = await _categoryManager.UpdateAsync(id, categoryDto.CategoryName);

            category.Description = categoryDto.Description;
            category.Picture = categoryDto.Picture;

            await _categoryRepository.UpdateAsync(category);
        }
    }
}
