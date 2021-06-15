using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPICAP.API.Interfaces;
using TPICAP.API.Models;
using TPICAP.Data.Exceptions;

namespace TPICAP.API.Controllers
{
    [Route("api/persons")]
    [Authorize]
    public class PersonsController : ApiControllerBase
    {
        private readonly ILogger<PersonsController> Logger;
        private readonly IPersonsService PersonsService;

        public PersonsController(ILogger<PersonsController> logger, IPersonsService personsService)
        {
            this.Logger = logger ??
                throw new ArgumentException(nameof(logger));
            this.PersonsService = personsService ??
                throw new ArgumentException(nameof(personsService)); ;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PersonResponseModel>>> Get()
        {
            var persons = await this.PersonsService.GetAllAsync();
            var result = this.Ok(persons);
            return result;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonResponseModel>> GetById(
            [FromRoute] int id)
        {
            try
            {
                var person = await this.PersonsService.GetByIdAsync(id);
                return this.Ok(person);
            }
            catch (EntityNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonResponseModel>> Create(
            [FromBody]  PersonCreationModel person)
        {
            try
            {
                var personCreated = await this.PersonsService.AddAsync(person);
                return this.Created($"{ Request.Path}/{personCreated.Id}", personCreated);
            }
            catch (Data.Exceptions.InvalidRequestException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonResponseModel>> Change(
            [FromRoute] int id,
            [FromBody] PersonModificationModel person)
        {
            try
            {
                this.ValidateId(id, person.Id);
            }
            catch (Exceptions.InvalidRequestException)
            {
                return this.BadRequest();
            }
            try
            {
                var personModified = await this.PersonsService.ModifyAsync(person);
                return this.Ok(personModified);
            }
            catch (Data.Exceptions.InvalidRequestException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(
            [FromRoute] int id)
        {
            try
            {
                await this.PersonsService.RemoveByIdAsync(id);
                return this.NoContent();
            }
            catch (EntityNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
