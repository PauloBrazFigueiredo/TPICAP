using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TPICAP.API.Models;
using TPICAP.API.Services;
using Xunit;

namespace TPICAP.API.Tests.Services
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class AuthenticationServiceUnitTests
    {
        [Fact]
        public async void CreateJwtSecurityToken_ShouldReturnToken()
        {
            // Arrange
            var authenticationService = new AuthenticationService();
            var login = new LoginModel { UserName = "user", Password = "pass" };

            // Act
            var result = await authenticationService.CreateJwtSecurityToken(login);

            // Assert
            result.Should().NotBeNullOrEmpty(result);
        }
    }
}
