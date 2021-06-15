using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TPICAP.API.Controllers;
using TPICAP.API.Interfaces;
using TPICAP.API.Models;
using TPICAP.API.Tests.Mocks;
using TPICAP.Data.Exceptions;
using Xunit;

namespace TPICAP.API.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class PersonsControllerUnitTests
    {
        [Fact]
        public void PersonsController_ShouldCreateInstance()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();

            // Act
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);

            // Assert
            sut.Should().BeOfType<PersonsController>();
        }

        [Fact]
        public void PersonsService_HavingLoggerNull_ShouldThrowArgumentException()
        {
            // Arrange
            var mockPersonsService = new Mock<IPersonsService>();

            // Act
            Action action = () => new PersonsController(null, mockPersonsService.Object);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("logger");
        }

        [Fact]
        public void PersonsService_HavingPersonsServiceNull_ShouldThrowArgumentException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();

            // Act
            Action action = () => new PersonsController(mockLogger.Object, null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("personsService");
        }

        [Fact]
        public async void Get_HavingNoPersons_ShouldReturnStatus200OK()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new List<PersonResponseModel>());
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);

            // Act
            var result = await sut.Get();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetById_HavingPerson_ShouldReturnStatus200OK()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new PersonResponseModel());
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);

            // Act
            var result = await sut.GetById(1);

            // Assert
            result.Should().BeOfType<ActionResult<PersonResponseModel>>();
        }

        [Fact]
        public async void GetById_NonExistingPerson_ShouldReturnStatus404NotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.GetByIdAsync(It.IsAny<int>())).Throws(new EntityNotFoundException());
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);

            // Act
            var result = await sut.GetById(1);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void Create_ShouldReturnStatus201Created()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.AddAsync(It.IsAny<PersonCreationModel>())).ReturnsAsync(new PersonResponseModel());
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);
            sut.Request = new MockHttpRequest();
            var person = new PersonCreationModel();

            // Act
            var result = await sut.Create(person);

            // Assert
            result.Result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async void Create_HavingInvalidFirstName_ShouldReturnStatus400Badrequest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.AddAsync(It.IsAny<PersonCreationModel>()))
                .Throws(new InvalidRequestException("Invalid 'First Name'"));
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);
            sut.Request = new MockHttpRequest();
            var person = new PersonCreationModel();

            // Act
            var result = await sut.Create(person);

            // Assert
            result.Should().BeOfType<ActionResult<PersonResponseModel>>();
        }

        [Fact]
        public async void Change_ShouldReturnStatus200OK()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.ModifyAsync(It.IsAny<PersonModificationModel>())).ReturnsAsync(new PersonResponseModel());
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);
            var person = new PersonModificationModel() { Id = 6};

            // Act
            var result = await sut.Change(6, person);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Change_HavingInvalidIds_ShouldReturnStatus400Badrequest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);
            var person = new PersonModificationModel() { Id = 99 };

            // Act
            var result = await sut.Change(3, person);

            // Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Change_HavingInvalidFirstName_ShouldReturnStatus400Badrequest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.ModifyAsync(It.IsAny<PersonModificationModel>()))
                .Throws(new InvalidRequestException("Invalid 'First Name'"));
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);
            var person = new PersonModificationModel() { Id = 3, FirstName = new string('X', 110) };

            // Act
            var result = await sut.Change(3, person);

            // Assert
            result.Should().BeOfType<ActionResult<PersonResponseModel>>();
        }

        [Fact]
        public async void Delete_ExistingPerson_ShouldReturnStatus204NoContent()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.RemoveByIdAsync(It.IsAny<int>()));
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);

            // Act
            var result = await sut.Delete(2);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        
        [Fact]
        public async void Delete_NonExistingPerson_ShouldReturnStatus204NoContent()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PersonsController>>();
            var mockPersonsService = new Mock<IPersonsService>();
            mockPersonsService.Setup(mock => mock.RemoveByIdAsync(It.IsAny<int>())).Throws(new EntityNotFoundException());
            var sut = new PersonsController(mockLogger.Object, mockPersonsService.Object);

            // Act
            var result = await sut.Delete(2);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
