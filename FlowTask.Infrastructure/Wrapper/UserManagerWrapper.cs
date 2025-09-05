using FlowTask.Application.Interfaces;
using FlowTask.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlowTask.Infrastructure.Wrapper;

public class UserManagerWrapper : IUserManagerWrapper
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UserManagerWrapper(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return _userManager.CreateAsync(user, password);
    }

    public Task<ApplicationUser?> FindByNameAsync(string username)
    {
        return _userManager.FindByNameAsync(username);
    }

    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return _userManager.CheckPasswordAsync(user, password);
    }
}