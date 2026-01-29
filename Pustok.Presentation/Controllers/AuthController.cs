using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Dtos.UserDtos;
using Pustok.Business.Services.Abstractions;

namespace Pustok.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _service) : ControllerBase
{

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {

        var result = await _service.RegisterAsync(dto);

        return Ok(result);
    }
}
