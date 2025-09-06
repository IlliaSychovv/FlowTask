using FlowTask.Application.Interfaces;
using FlowTask.Application.Interfaces.Repository;
using FlowTask.Application.Interfaces.Service;
using FlowTask.Application.Services;
using FlowTask.Infrastructure.Repositories;
using FlowTask.Infrastructure.Services;
using FlowTask.Infrastructure.Wrapper;

namespace FlowTask.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        
        return services;
    }
}