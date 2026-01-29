using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions;
using Pustok.Business.Services.Abstractions;
using Pustok.Core.Entites;
using Pustok.DataAccess.Repositories.Abstractions;

namespace Pustok.Business.Services.Implementations;

internal class ProductService(IProductRepository _repository, IMapper _mapper, ICloudinaryService _cloudinaryService) : IProductService
{
    public async Task<ResultDto> CreateAsync(ProductCreateDto dto)
    {
        var product = _mapper.Map<Product>(dto);


        var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
        product.ImagePath = imagePath;

        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();

        return new();
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException("Project is not found");

        _repository.Delete(product);
        await _repository.SaveChangesAsync();


        await _cloudinaryService.FileDeleteAsync(product.ImagePath);

        return new();
    }

    public async Task<ResultDto<List<ProductGetDto>>> GetAllAsync()
    {
        var products = await _repository.GetAll(true).Include(x => x.Category).ToListAsync();

        var dtos = _mapper.Map<List<ProductGetDto>>(products);

        return new(dtos);
    }

    public async Task<ResultDto<ProductGetDto>> GetAsync(Guid id)
    {

        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException("Project is not found");

        var dto = _mapper.Map<ProductGetDto>(product);

        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(ProductUpdateDto dto)
    {

        var product = await _repository.GetByIdAsync(dto.Id);

        if (product is null)
            throw new NotFoundException("Project is not found");

        product = _mapper.Map(dto, product);

        if (dto.Image is not null)
        {
            await _cloudinaryService.FileDeleteAsync(product.ImagePath);
            var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
            product.ImagePath = imagePath;
        }

        _repository.Update(product);
        await _repository.SaveChangesAsync();

        return new();
    }
}
