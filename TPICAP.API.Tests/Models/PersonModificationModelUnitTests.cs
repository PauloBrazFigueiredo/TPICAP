using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using TPICAP.API.Models;
using Xunit;

namespace TPICAP.API.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Model")]
    public class PersonModificationModelUnitTests
    {
        [Fact]
        public void PersonModificationModel_ShouldCreateInstance()
        {
            // Act
            var sut = new PersonModificationModel();

            // Assert
            sut.Should().BeOfType<PersonModificationModel>();
        }

        [Fact]
        public void SetAndGetId_ShouldReturnInt()
        {
            // Arrange
            var sut = new PersonModificationModel();

            // Act
            sut.Id = 99;
            var result = sut.Id;

            // Assert
            result.Should().Be(99);
            result.GetType().Should().Be(typeof(int));
        }

        [Fact]
        public void SetAndGetFirstName_ShouldReturnString()
        {
            // Arrange
            var sut = new PersonModificationModel();

            // Act
            sut.FirstName = "first";
            var result = sut.FirstName;

            // Assert
            result.Should().Be("first");
            result.Should().BeOfType<string>();
        }

        [Fact]
        public void SetAndGetLastName_ShouldReturnString()
        {
            // Arrange
            var sut = new PersonModificationModel();

            // Act
            sut.LastName = "last";
            var result = sut.LastName;

            // Assert
            result.Should().Be("last");
            result.Should().BeOfType<string>();
        }

        [Fact]
        public void SetAndGetDOB_ShouldReturnDateTime()
        {
            // Arrange
            var sut = new PersonModificationModel();

            // Act
            sut.Dob = new DateTime(2015, 10, 21);
            var result = sut.Dob;

            // Assert
            result.Should().Be(new DateTime(2015, 10, 21));
            result.GetType().Should().Be(typeof(DateTime));
        }

        [Fact]
        public void SetAndGetSalutation_ShouldReturnString()
        {
            // Arrange
            var sut = new PersonModificationModel();

            // Act
            sut.Salutation = "Hi";
            var result = sut.Salutation;

            // Assert
            result.Should().Be("Hi");
            result.Should().BeOfType<string>();
        }
    }
}
