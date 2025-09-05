using FlowTask.Domain.Entities;

namespace FlowTask.Application.Interfaces.Service;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}