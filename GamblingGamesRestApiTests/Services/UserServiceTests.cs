using AutoFixture;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;
using GamblingGamesRestApi.Infrastructure;
using GamblingGamesRestApi.Services;
using Microsoft.Extensions.Logging;
using GamblingGamesRestApi.Repositories;

namespace GamblingGamesRestApiTests.Services;

public class UserServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IUserManagerWrapper> _userManagerWrapperMock;
    private readonly Mock<IPointRepository> _pointRepositoryMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _fixture = new Fixture();
        _userManagerWrapperMock = new Mock<IUserManagerWrapper>();
        _pointRepositoryMock = new Mock<IPointRepository>();
        _loggerMock = new Mock<ILogger<UserService>>();

        _userService = new UserService(_userManagerWrapperMock.Object, _pointRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_CreateUserAndAddPoints()
    {
        // Arrange
        var expectedUser = _fixture.Create<ApplicationUser>();
        var expectedPassword = _fixture.Create<string>();
        var expectedIdentityResult = IdentityResult.Success;
        _userManagerWrapperMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(expectedIdentityResult);
        _pointRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);

        // Act
        var result = await _userService.CreateAsync(expectedUser.Email, expectedPassword);

        // Assert
        Assert.Equal(expectedIdentityResult, result);
        _userManagerWrapperMock.Verify(x => x.CreateAsync(It.Is<ApplicationUser>(u => u.Email == expectedUser.Email && u.UserName == expectedUser.Email),
                                                          It.Is<string>(p => p == expectedPassword)), Times.Once);
        _pointRepositoryMock.Verify(x => x.UpdateAsync(It.Is<string>(e => e == expectedUser.Email), It.Is<int>(p => p == 10000)), Times.Once);
    }
}
