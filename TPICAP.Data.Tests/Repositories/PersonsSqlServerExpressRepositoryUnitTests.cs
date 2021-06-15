using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TPICAP.Data.Exceptions;
using TPICAP.Data.Interfaces;
using TPICAP.Data.Models;
using TPICAP.Data.Repositories;
using Xunit;

namespace TPICAP.Data.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    [Trait("Unit Tests", "Functional")]
    public class PersonsSqlServerExpressRepositoryUnitTests
    {
        [Fact]
        public void PersonsSqlServerExpressRepository_ShouldCreateInstance()
        {
           // Arrange
           var context = this.CreateInMemoryPersonsDatabaseContext();

           // Act
           var sut = new PersonsSqlServerExpressRepository(context);

            // Assert
            sut.Should().BeOfType<PersonsSqlServerExpressRepository>();
            sut.Should().BeAssignableTo<IPersonsRepository>();
        }

        [Fact]
        public void PersonsSqlServerExpressRepository_HavingPersonsDatabaseContextNull_ShouldThrowArgumentException()
        {
            // Act
            Action action = () => new PersonsSqlServerExpressRepository(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("context");
        }

        [Fact]
        public async Task GetAllAsync_Having2Persons_ShouldReturn2Persons()
        {
            // Arrange
            var context = this.CreateInMemoryPersonsDatabaseContext();
            context.Persons.Add(new Person() { Id = 1});
            context.Persons.Add(new Person() { Id = 2 });
            context.SaveChanges();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            var result = await sut.GetAllAsync();

            // Assert
            result.Should().BeOfType<List<Person>>();
            result.Should().BeAssignableTo<IEnumerable<Person>>();
            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetByIdAsync_Having2Persons_ShouldReturn1Persons()
        {
            // Arrange
            var context = this.CreateInMemoryPersonsDatabaseContext();
            context.Persons.Add(new Person() { Id = 1 });
            context.Persons.Add(new Person() { Id = 2 });
            context.Persons.Add(new Person() { Id = 3 });
            context.SaveChanges();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            var result = await sut.GetByIdAsync(2);

            // Assert
            result.Should().BeOfType<Person>();
            result.Id.Should().Be(2);
        }

        [Fact]
        public async Task GetByIdAsync_HavingNonExistingPerson_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var context = this.CreateInMemoryPersonsDatabaseContext();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            Func<Task> action = async () => await sut.GetByIdAsync(2);

            // Assert
            action.Should().Throw<EntityNotFoundException>();
        }

        [Fact]
        public async Task AddAsync_ShouldAdd()
        {
            // Arrange
            var newPerson = new Person() { Id = 1 };
            var context = CreateInMemoryPersonsDatabaseContext();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            var result = await sut.AddAsync(newPerson);

            // Assert
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task AddAsync_HavingInvalidFirstName_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var newPerson = new Person() { FirstName = new string('X', 110) };
            var context = CreateInMemoryPersonsDatabaseContext();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            Func<Task> action = async () => await sut.AddAsync(newPerson);

            // Assert
            action.Should().Throw<InvalidRequestException>()
                .WithMessage("Invalid 'First Name'");
        }

        [Fact]
        public async Task AddAsync_HavingInvalidLastName_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var newPerson = new Person() { LastName = new string('X', 110) };
            var context = CreateInMemoryPersonsDatabaseContext();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            Func<Task> action = async () => await sut.AddAsync(newPerson);

            // Assert
            action.Should().Throw<InvalidRequestException>()
                .WithMessage("Invalid 'Last Name'");
        }

        [Fact]
        public async Task AddAsync_HavingInvalidSalutation_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var newPerson = new Person() { Salutation = new string('X', 160) };
            var context = CreateInMemoryPersonsDatabaseContext();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            Func<Task> action = async () => await sut.AddAsync(newPerson);

            // Assert
            action.Should().Throw<InvalidRequestException>()
                .WithMessage("Invalid 'Salutation'");
        }

        [Fact]
        public async Task ModifyAsync_ShouldChange()
        {
            // Arrange
            var person = new Person() { Id = 1, FirstName = "name" };
            var context = CreateInMemoryPersonsDatabaseContext();
            context.Persons.Add(person);
            context.SaveChanges();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            person.FirstName = "important";
            var result = await sut.ModifyAsync(person);

            // Assert
            result.FirstName.Should().Be("important");
        }

        [Fact]
        public async Task ModifyAsync_HavingInvalidFirstName_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var person = new Person() { Id = 1, FirstName = "name" };
            var context = CreateInMemoryPersonsDatabaseContext();
            context.Persons.Add(person);
            context.SaveChanges();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            person.FirstName = new string('X', 110);
            Func<Task> action = async () => await sut.ModifyAsync(person);

            // Assert
            action.Should().Throw<InvalidRequestException>()
                .WithMessage("Invalid 'First Name'");
        }

        [Fact]
        public async Task ModifyAsync_HavingInvalidLastName_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var person = new Person() { Id = 1, LastName = "name" };
            var context = CreateInMemoryPersonsDatabaseContext();
            context.Persons.Add(person);
            context.SaveChanges();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            person.LastName = new string('X', 110);
            Func<Task> action = async () => await sut.ModifyAsync(person);

            // Assert
            action.Should().Throw<InvalidRequestException>()
                .WithMessage("Invalid 'Last Name'");
        }

        [Fact]
        public async Task ModifyAsync_HavingInvalidSalutation_ShouldThrowInvalidRequestException()
        {
            // Arrange
            var person = new Person() { Id = 1, Salutation = "Hi" };
            var context = CreateInMemoryPersonsDatabaseContext();
            context.Persons.Add(person);
            context.SaveChanges();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            person.Salutation = new string('X', 160);
            Func<Task> action = async () => await sut.ModifyAsync(person);

            // Assert
            action.Should().Throw<InvalidRequestException>()
                .WithMessage("Invalid 'Salutation'");
        }

        [Fact]
        public async Task RemoveByIdAsync_ShouldRemove()
        {
            // Arrange
            var person = new Person() { Id = 1 };
            var context = CreateInMemoryPersonsDatabaseContext();
            context.Persons.Add(person);
            context.SaveChanges();
            var sut = new PersonsSqlServerExpressRepository(context);

            // Act
            await sut.RemoveByIdAsync(1);

            // Assert
            context.Persons.Should().BeEmpty();
        }

        protected PersonsDatabaseContext CreateInMemoryPersonsDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<PersonsDatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new PersonsDatabaseContext(options);
        }
    }
}
