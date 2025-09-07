using FlowTask.Application.DTO.Authorization;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Interfaces.Service;
using FlowTask.Application.Services;
using FlowTask.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shouldly;
using Moq;

namespace FlowTask.Tests.UnitTests;

public class AuthServiceTest
{
    private readonly Mock<IUserManagerWrapper> _userManagerWrapperMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly AuthService _authService;

    public AuthServiceTest()
    {
        _userManagerWrapperMock = new Mock<IUserManagerWrapper>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _loggerMock = new Mock<ILogger<AuthService>>();
        
        _authService = new AuthService(
            _jwtTokenServiceMock.Object,
            _userManagerWrapperMock.Object,
            _loggerMock.Object
        );
    }
    
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnSucceeded_WhenUserIsCreatedSuccessfully()
    {
        var dto = new RegisterDto
        {
            UserName = "TestUser", 
            Password = "Password123!"
        };
        
        _userManagerWrapperMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _authService.RegisterUserAsync(dto);

        result.Succeeded.ShouldBeTrue();
        _userManagerWrapperMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnFailed_WhenUserCreationFails()
    {
        var dto = new RegisterDto
        {
            UserName = "TestUser", 
            Password = "Password123!"
        };
        
        var failedResult = IdentityResult.Failed(new IdentityError { Description = "Error" });
        _userManagerWrapperMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
            .ReturnsAsync(failedResult);

        var result = await _authService.RegisterUserAsync(dto);

        result.Succeeded.ShouldBeFalse();
        result.Errors.ShouldContain(x => x.Description == "Error");
    }
    
    [Fact]
    public async Task LoginUserAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var username = "TestUser";
        var password = "Password123!";
        var user = new ApplicationUser
        {
            UserName = username
        };

        _userManagerWrapperMock
            .Setup(x => x.FindByNameAsync(username))
            .ReturnsAsync(user);
        _userManagerWrapperMock
            .Setup(x => x.CheckPasswordAsync(user, password))
            .ReturnsAsync(true);
        _jwtTokenServiceMock
            .Setup(x => x.GenerateToken(user))
            .Returns("JWT_TOKEN");
        
        var token = await _authService.LoginUserAsync(username, password);

        token.ShouldBe("JWT_TOKEN");
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var username = "NonExistingUser";
        _userManagerWrapperMock
            .Setup(x => x.FindByNameAsync(username))
            .ReturnsAsync((ApplicationUser)null!);

        var token = await _authService.LoginUserAsync(username, "anyPassword");

        token.ShouldBeNull();
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnNull_WhenPasswordIsInvalid()
    {
        var username = "TestUser";
        var user = new ApplicationUser
        {
            UserName = username
        };

        _userManagerWrapperMock
            .Setup(x => x.FindByNameAsync(username))
            .ReturnsAsync(user);
        _userManagerWrapperMock
            .Setup(x => x.CheckPasswordAsync(user, "WrongPassword"))
            .ReturnsAsync(false);

        var token = await _authService.LoginUserAsync(username, "WrongPassword");

        token.ShouldBeNull();
    }
}