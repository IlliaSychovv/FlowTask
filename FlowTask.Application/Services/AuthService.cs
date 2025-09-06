using FlowTask.Application.DTO.Authorization;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Interfaces.Service;
using FlowTask.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FlowTask.Application.Services;

public class AuthService : IAuthService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserManagerWrapper _userManagerWrapper;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IJwtTokenService jwtTokenService, IUserManagerWrapper userManagerWrapper,
        ILogger<AuthService> logger)
    {
        _jwtTokenService = jwtTokenService;
        _userManagerWrapper = userManagerWrapper;
        _logger = logger;
    }
    
    public async Task<IdentityResult> RegisterUserAsync(RegisterDto dto)
    {
        var user = dto.Adapt<ApplicationUser>();
        var result = await _userManagerWrapper.CreateAsync(user, dto.Password);
        
        if (result.Succeeded)
            _logger.LogInformation("User {Username} registered successfully", dto.UserName);
        
        return result;
    }
    
    public async Task<string?> LoginUserAsync(string username, string password)
    {
        var user = await _userManagerWrapper.FindByNameAsync(username);
        if (user == null || !await _userManagerWrapper.CheckPasswordAsync(user, password))
        {
            _logger.LogWarning("Failed login attempt for user {Username}", username);
            return null;
        }
        
        return _jwtTokenService.GenerateToken(user!);
    }
}