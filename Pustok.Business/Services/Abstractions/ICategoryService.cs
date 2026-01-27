namespace Pustok.Business.Services.Abstractions;

public interface ICategoryService
{
    Task CreateAsync(CategoryCreateDto dto);
    Task UpdateAsync(CategoryUpdateDto dto);
    Task DeleteAsync(Guid id);
    Task<List<CategoryGetDto>> GetAllAsync();
    Task<CategoryGetDto> GetByIdAsync(Guid id);


    //Task<List<ProductGetDto>> GetProductsByCategoryId(Guid categoryId);
}
