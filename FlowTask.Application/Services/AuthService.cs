using FlowTask.Application.DTO.Authorization;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Interfaces.Service;
using FlowTask.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace FlowTask.Application.Services;

public class AuthService : IAuthService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserManagerWrapper _userManagerWrapper;

    public AuthService(IJwtTokenService jwtTokenService, IUserManagerWrapper userManagerWrapper)
    {
        _jwtTokenService = jwtTokenService;
        _userManagerWrapper = userManagerWrapper;
    }
    
    public async Task<IdentityResult> RegisterUserAsync(RegisterDto dto)
    {
        var user = dto.Adapt<ApplicationUser>();
  
        var result = await _userManagerWrapper.CreateAsync(user, dto.Password);
        
        return result;
    }
    
    public async Task<string?> LoginUserAsync(string username, string password)
    {
        var user = await _userManagerWrapper.FindByNameAsync(username);

        var passwordValid = await _userManagerWrapper.CheckPasswordAsync(user!, password);
        if (!passwordValid)
            return null;
        
        return _jwtTokenService.GenerateToken(user!);
    }
}