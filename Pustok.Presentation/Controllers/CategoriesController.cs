using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Dtos;
using Pustok.Business.Services.Abstractions;

namespace Pustok.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService _service) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Ok(categories);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var category = await _service.GetByIdAsync(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok("Created");
    }


    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CategoryUpdateDto dto)
    {
        await _service.UpdateAsync(dto);
        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok("Deleted");
    }
}
