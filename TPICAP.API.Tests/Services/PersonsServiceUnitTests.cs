using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TPICAP.API.Interfaces;
using TPICAP.API.Mapping;
using TPICAP.API.Models;
using TPICAP.API.Services;
using TPICAP.Data.Interfaces;
using TPICAP.Data.Models;
using Xunit;

namespace TPICAP.API.Tests.Services
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class PersonsServiceUnitTests
    {
        [Fact]
        public void PersonsService_ShouldCreateInstance()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockPersonsRepository = new Mock<IPersonsRepository>();

            // Act
            var sut = new PersonsService(mockMapper.Object, mockPersonsRepository.Object);

            // Assert
            sut.Should().BeOfType<PersonsService>();
            sut.Should().BeAssignableTo<IPersonsService>();
        }

        [Fact]
        public void PersonsService_HavingMapperNull_ShouldThrowArgumentException()
        {
            // Arrange
            var mockPersonsRepository = new Mock<IPersonsRepository>();

            // Act
            Action action = () => new PersonsService(null, mockPersonsRepository.Object);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("mapper");
        }

        [Fact]
        public void PersonsService_HavingPersonsRepositoryNull_ShouldThrowArgumentException()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();

            // Act
            Action action = () => new PersonsService(mockMapper.Object, null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("personsRepository");
        }

        [Fact]
        public async void GetAllAsync_Having2Persons_ShouldReturn2PersonResponseModel()
        {
            // Arrange
            var mapper = new MapperConfiguration(config => config.AddProfile<PersonProfile>()).CreateMapper();
            var mockPersonsRepository = new Mock<IPersonsRepository>();
            mockPersonsRepository.Setup(mock => mock.GetAllAsync())
                .ReturnsAsync((new List<Person> { new Person(), new Person()} as IEnumerable<Person>));
            var sut = new PersonsService(mapper, mockPersonsRepository.Object);

            // Act
            var result = await sut.GetAllAsync();

            // Assert
            mockPersonsRepository.Verify(mock => mock.GetAllAsync(), Times.Once());
            (result as List<PersonResponseModel>).Count.Should().Be(2);
        }

        [Fact]
        public async void GetByIdAsync_Having1Person_ShouldReturnPersonResponseModel()
        {
            // Arrange
            var mapper = new MapperConfiguration(config => config.AddProfile<PersonProfile>()).CreateMapper();
            var mockPersonsRepository = new Mock<IPersonsRepository>();
            mockPersonsRepository.Setup(mock => mock.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Person{ Id = 1 });
            var sut = new PersonsService(mapper, mockPersonsRepository.Object);

            // Act
            var result = await sut.GetByIdAsync(1);

            // Assert
            mockPersonsRepository.Verify(mock => mock.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async void AddAsync_Add1PersonCreationModel_ShouldReturnPersonResponseModel()
        {
            // Arrange
            var mapper = new MapperConfiguration(config => config.AddProfile<PersonProfile>()).CreateMapper();
            var mockPersonsRepository = new Mock<IPersonsRepository>();
            mockPersonsRepository.Setup(mock => mock.AddAsync(It.IsAny<Person>()))
                .ReturnsAsync(new Person { Id = 1 });
            var sut = new PersonsService(mapper, mockPersonsRepository.Object);
            var creationModel = new PersonCreationModel();

            // Act
            var result = await sut.AddAsync(creationModel);

            // Assert
            mockPersonsRepository.Verify(mock => mock.AddAsync(It.IsAny<Person>()), Times.Once());
        }

        [Fact]
        public async void ModifyAsync_Modify1PersonModificationModel_ShouldReturnPersonResponseModel()
        {
            // Arrange
            var mapper = new MapperConfiguration(config => config.AddProfile<PersonProfile>()).CreateMapper();
            var mockPersonsRepository = new Mock<IPersonsRepository>();
            mockPersonsRepository.Setup(mock => mock.ModifyAsync(It.IsAny<Person>()))
                .ReturnsAsync(new Person { Id = 1 });
            var sut = new PersonsService(mapper, mockPersonsRepository.Object);
            var modificationModel = new PersonModificationModel();

            // Act
            var result = await sut.ModifyAsync(modificationModel);

            // Assert
            mockPersonsRepository.Verify(mock => mock.ModifyAsync(It.IsAny<Person>()), Times.Once());
        }

        [Fact]
        public async void RemoveByIdAsync_Having1Person_ShouldRemovePerson()
        {
            // Arrange
            var mapper = new MapperConfiguration(config => config.AddProfile<PersonProfile>()).CreateMapper();
            var mockPersonsRepository = new Mock<IPersonsRepository>();
            mockPersonsRepository.Setup(mock => mock.RemoveByIdAsync(It.IsAny<int>()));
            var sut = new PersonsService(mapper, mockPersonsRepository.Object);

            // Act
            await sut.RemoveByIdAsync(1);

            // Assert
            mockPersonsRepository.Verify(mock => mock.RemoveByIdAsync(It.IsAny<int>()), Times.Once());
        }
    }
}
