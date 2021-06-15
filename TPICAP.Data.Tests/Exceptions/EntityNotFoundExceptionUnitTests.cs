using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TPICAP.Data.Exceptions;
using Xunit;

namespace TPICAP.Data.Tests.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class EntityNotFoundExceptionUnitTests
    {
        [Fact]
        public void EntityNotFoundException_ShouldCreateInstance()
        {
            // Act
            var sut = new EntityNotFoundException();

            // Assert
            sut.Should().BeOfType<EntityNotFoundException>();
        }
    }
}
