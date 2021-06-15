using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TPICAP.API.Exceptions;
using Xunit;

namespace TPICAP.API.Tests.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class InvalidRequestExceptionUnitTests
    {
        [Fact]
        public void InvalidRequestException_ShouldCreateInstance()
        {
            // Act
            var sut = new InvalidRequestException();

            // Assert
            sut.Should().BeOfType<InvalidRequestException>();
        }
    }
}
