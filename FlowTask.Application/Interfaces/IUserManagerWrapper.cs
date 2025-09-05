using FlowTask.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlowTask.Application.Interfaces;

public interface IUserManagerWrapper
{ 
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    public Task<ApplicationUser?> FindByNameAsync(string username);
    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}