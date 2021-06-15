using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TPICAP.API.Models;
using Xunit;

namespace TPICAP.API.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Model")]
    public class LoginModelUnitTests
    {
        [Fact]
        public void LoginModel_ShouldCreateInstance()
        {
            // Act
            var sut = new LoginModel();

            // Assert
            sut.Should().BeOfType<LoginModel>();
        }

        [Fact]
        public void SetAndGetUserName_ShouldReturnString()
        {
            // Arrange
            var sut = new LoginModel();

            // Act
            sut.UserName = "user";
            var result = sut.UserName;

            // Assert
            result.Should().Be("user");
            result.Should().BeOfType<string>();
        }

        [Fact]
        public void SetAndGetPassword_ShouldReturnString()
        {
            // Arrange
            var sut = new LoginModel();

            // Act
            sut.Password = "pass";
            var result = sut.Password;

            // Assert
            result.Should().Be("pass");
            result.Should().BeOfType<string>();
        }
    }
}
