using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using TPICAP.API.Controllers;
using TPICAP.API.Interfaces;
using TPICAP.API.Models;
using Xunit;

namespace TPICAP.API.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class AuthenticationControllerUnitTests
    {
        [Fact]
        public void AuthenticationController_ShouldCreateInstance()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AuthenticationController>>();
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            // Act
            var sut = new AuthenticationController(mockLogger.Object, mockAuthenticationService.Object);

            // Assert
            sut.Should().BeOfType<AuthenticationController>();
        }

        [Fact]
        public void AuthenticationController_HavingLoggerNull_ShouldThrowArgumentException()
        {
            // Arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            // Act
            Action action = () => new AuthenticationController(null, mockAuthenticationService.Object);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("logger");
        }

        [Fact]
        public void AuthenticationController_HavingAuthenticationServiceNull_ShouldThrowArgumentException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AuthenticationController>>();

            // Act
            Action action = () => new AuthenticationController(mockLogger.Object, null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("authenticationService");
        }

        [Fact]
        public async void Login_HavingValidToken_ShouldReturnStatus200OK()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AuthenticationController>>();
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService.Setup(mock => mock.CreateJwtSecurityToken(It.IsAny<LoginModel>())).ReturnsAsync("token");
            var sut = new AuthenticationController(mockLogger.Object, mockAuthenticationService.Object);

            // Act
            var result = await sut.Login(new LoginModel());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Login_HavingNoToken_ShouldReturnStatus400Badrequest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AuthenticationController>>();
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService.Setup(mock => mock.CreateJwtSecurityToken(It.IsAny<LoginModel>())).ReturnsAsync(string.Empty);
            var sut = new AuthenticationController(mockLogger.Object, mockAuthenticationService.Object);

            // Act
            var result = await sut.Login(new LoginModel());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
