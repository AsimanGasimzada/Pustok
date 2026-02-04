namespace Pustok.Business.Services.Abstractions;

public interface IProductService
{
    Task<ResultDto> CreateAsync(ProductCreateDto dto);
    Task<ResultDto> UpdateAsync(ProductUpdateDto dto);
    Task<ResultDto<ProductUpdateDto>> GetUpdatedDtoAsync(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<ProductGetDto>>> GetAllAsync();
    Task<ResultDto<ProductGetDto>> GetAsync(Guid id);
}
