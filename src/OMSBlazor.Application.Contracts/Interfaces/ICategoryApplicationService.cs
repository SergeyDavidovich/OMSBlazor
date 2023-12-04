using OMSBlazor.Dto.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Application.Contracts.Interfaces
{
    public interface ICategoryApplicationService : IApplicationService
    {
        public Task<List<CategoryDto>> GetCategoriesAsync();

        public Task<CategoryDto> GetCategoryAsync(int id);

        public Task DeleteCategoryAsync(int id);

        public Task CreateCategoryAsync(CreateCategoryDto categoryDto);

        public Task UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto);
    }
}
