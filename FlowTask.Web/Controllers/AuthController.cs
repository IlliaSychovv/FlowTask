using FlowTask.Application.DTO.Authorization;
using FlowTask.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlowTask.Controllers;

[ApiController]
[Route("/api/v1/users")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
    {
        var user = await _authService.RegisterUserAsync(dto);

        if (!user.Succeeded)
            return BadRequest();
        
        return Created(string.Empty, new { Email = dto.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginUserAsync(dto.Name, dto.Password);
        
        if (token == null)
            return Unauthorized();
        
        return Ok(new { Token = token });
    }
}