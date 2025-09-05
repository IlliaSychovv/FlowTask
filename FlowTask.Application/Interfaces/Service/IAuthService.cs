using FlowTask.Application.DTO.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FlowTask.Application.Interfaces.Service;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterDto dto);
    Task<string?> LoginUserAsync(string username, string password);
}