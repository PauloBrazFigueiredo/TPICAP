using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPICAP.Data.Exceptions;
using TPICAP.Data.Interfaces;
using TPICAP.Data.Models;

namespace TPICAP.Data.Repositories
{
    public class PersonsSqlServerExpressRepository : IPersonsRepository
    {
        private readonly PersonsDatabaseContext Context;

        public PersonsSqlServerExpressRepository(PersonsDatabaseContext context)
        {
            this.Context = context ??
                throw new ArgumentException(nameof(context));
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await  this.Context.Persons
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            var result = await this.Context.Persons
                .AsNoTracking()
                .SingleOrDefaultAsync(entity => entity.Id == id);
            if (result == null)
            {
                throw new EntityNotFoundException();
            }
            return result;
        }

        public async Task<Person> AddAsync(Person person)
        {
            this.Validate(person);
            var result = this.Context.Persons.Add(person);
            await this.Context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Person> ModifyAsync(Person person)
        {
            this.Validate(person);
            var result =  this.Context.Update(person);
            await this.Context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task RemoveByIdAsync(int id)
        {
            var personal = await this.Context.Persons
                .SingleOrDefaultAsync(entity => entity.Id == id);
            if (personal == null)
            {
                throw new EntityNotFoundException();
            }

            var result = this.Context.Persons.Remove(personal);
            await this.Context.SaveChangesAsync();
        }

        private void Validate(Person person)
        {
            this.ValidateFirstNameLength(person);
            this.ValidateLastNameLength(person);
            this.ValidateSalutationLength(person);
        }

        private void ValidateFirstNameLength(Person person)
        {
            var max = this.Context.Model.FindEntityType(typeof(Person))
                .FindProperty(nameof(Person.FirstName))
                .GetMaxLength();
            if (string.IsNullOrEmpty(person.FirstName) == false && person.FirstName.Length > max)
            {
                throw new InvalidRequestException("Invalid 'First Name'");
            }
        }

        private void ValidateLastNameLength(Person person)
        {
            var max = this.Context.Model.FindEntityType(typeof(Person))
                .FindProperty(nameof(Person.LastName))
                .GetMaxLength();
            if (string.IsNullOrEmpty(person.LastName) == false && person.LastName.Length > max)
            {
                throw new InvalidRequestException("Invalid 'Last Name'");
            }
        }

        private void ValidateSalutationLength(Person person)
        {
            var max = this.Context.Model.FindEntityType(typeof(Person))
                .FindProperty(nameof(Person.Salutation))
                .GetMaxLength();
            if (string.IsNullOrEmpty(person.Salutation) == false && person.Salutation.Length > max)
            {
                throw new InvalidRequestException("Invalid 'Salutation'");
            }
        }
    }
}
