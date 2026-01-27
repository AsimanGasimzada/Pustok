using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions;
using Pustok.Business.Services.Abstractions;
using Pustok.Core.Entites;
using Pustok.DataAccess.Repositories.Abstractions;

namespace Pustok.Business.Services.Implementations;

internal class ProductService(IProductRepository _repository, IMapper _mapper, ICloudinaryService _cloudinaryService) : IProductService
{
    public async Task CreateAsync(ProductCreateDto dto)
    {
        var product = _mapper.Map<Product>(dto);


        var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
        product.ImagePath = imagePath;

        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException("Project is not found");

        _repository.Delete(product);
        await _repository.SaveChangesAsync();


        await _cloudinaryService.FileDeleteAsync(product.ImagePath);
    }

    public async Task<List<ProductGetDto>> GetAllAsync()
    {
        var products = await _repository.GetAll().Include(x => x.Category).ToListAsync();

        var dtos = _mapper.Map<List<ProductGetDto>>(products);

        return dtos;
    }

    public async Task<ProductGetDto> GetAsync(Guid id)
    {

        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException("Project is not found");

        var dto = _mapper.Map<ProductGetDto>(product);

        return dto;
    }

    public async Task UpdateAsync(ProductUpdateDto dto)
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
    }
}
