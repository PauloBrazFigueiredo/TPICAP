using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TPICAP.Data.Exceptions;
using Xunit;

namespace TPICAP.Data.Tests.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class InvalidRequestExceptionUnitTests
    {
        [Fact]
        public void InvalidRequestException_ShouldCreateInstance()
        {
            // Act
            var sut = new InvalidRequestException("Invalid property");

            // Assert
            sut.Should().BeOfType<InvalidRequestException>();
        }
    }
}
