using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPICAP.API.Interfaces;
using TPICAP.API.Models;
using TPICAP.Data.Interfaces;
using TPICAP.Data.Models;

namespace TPICAP.API.Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IPersonsRepository PersonsRepository;
        private readonly IMapper Mapper;

        public PersonsService(IMapper mapper, IPersonsRepository personsRepository)
        {
            this.Mapper = mapper ??
                throw new ArgumentException(nameof(mapper));
            this.PersonsRepository = personsRepository ??
                throw new ArgumentException(nameof(personsRepository));
        }

        public async Task<IEnumerable<PersonResponseModel>> GetAllAsync()
        {
            var result = await this.PersonsRepository.GetAllAsync();
            var a = this.Mapper.Map<List<Person>, List<PersonResponseModel>>(result.ToList());
            return a;
        }

        public async Task<PersonResponseModel> GetByIdAsync(int id)
        {
            var result = await this.PersonsRepository.GetByIdAsync(id);
            return this.Mapper.Map<Person, PersonResponseModel>(result);
        }

        public async Task<PersonResponseModel> AddAsync(PersonCreationModel person)
        {
            var dbPerson =  this.Mapper.Map<PersonCreationModel, Person>(person);
            var result = await this.PersonsRepository.AddAsync(dbPerson);
            return this.Mapper.Map<Person, PersonResponseModel>(result);
        }

        public async Task<PersonResponseModel> ModifyAsync(PersonModificationModel person)
        {
            var dbPerson = this.Mapper.Map<PersonModificationModel, Person>(person);
            var result = await this.PersonsRepository.ModifyAsync(dbPerson);
            return this.Mapper.Map<Person, PersonResponseModel>(result);
        }

        public async Task RemoveByIdAsync(int id)
        {
            await this.PersonsRepository.RemoveByIdAsync(id);
        }
    }
}
