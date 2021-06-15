using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TPICAP.API.Models;
using TPICAP.Data.Models;
using Xunit;

namespace TPICAP.API.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [Trait("Integration Tests", "Functional")]
    [Collection("Sequential Integration Tests")]
    public class PersonsControllerIntegrationTests : IDisposable
    {
        private readonly TestServer TestServer;
        private readonly HttpClient HttpClient;
        private readonly string ConnectionString;

        public PersonsControllerIntegrationTests()
        {
            var configuration = this.GetConfiguration();
            this.TestServer = new TestServer(new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<Startup>());
            this.HttpClient = TestServer.CreateClient();
            this.ConnectionString = configuration.GetConnectionString("PersonsConnection");
        }

        public void Dispose()
        {
            this.RemoveAddedPersons();
        }

        [Fact]
        public async Task GetAll_Having2Persons_ShouldReturn200OK()
        {
            // Arrange
            string expectedBody = "[{\"id\":1,\"firstName\":\"John\",\"lastName\":\"Rambo\",\"dob\":\"2021-06-12T23:48:00.727\",\"salutation\":\"Hi 123\"},{\"id\":2,\"firstName\":\"Bruce\",\"lastName\":\"Wayne\",\"dob\":\"2021-06-12T23:48:00.727\",\"salutation\":\"Hi\"}]";

            // Act
            var response = await this.HttpClient.GetAsync("/api/persons");
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            responseBody.Should().Be(expectedBody);
            response.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetPerson_HavingThePerson_ShouldReturn200OK()
        {
            // Arrange
            string expectedBody = "{\"id\":2,\"firstName\":\"Bruce\",\"lastName\":\"Wayne\",\"dob\":\"2021-06-12T23:48:00.727\",\"salutation\":\"Hi\"}";

            // Act
            var response = await this.HttpClient.GetAsync("/api/persons/2");
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            responseBody.Should().Be(expectedBody);
            response.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetPerson_HavingNonExistingPerson_ShouldReturn404NotFound()
        {
            // Act
            var response = await this.HttpClient.GetAsync("/api/persons/999");
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Post_HavingValidPerson_ShouldReturn201Created()
        {
            // Arrange
            var person = new PersonCreationModel
            {
                FirstName = "Peter",
                LastName = "Parker",
                Dob = DateTime.UtcNow,
                Salutation = "Your friendly neighbor"
            };
            var json = this.ConvertToJson<PersonCreationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PostAsync("/api/persons", content);

            // Assert
            response.StatusCode.Should().Be(201);
        }
        
        [Fact]
        public async Task Post_HavingPersonInvalidFirstName_ShouldReturn400BadRequest()
        {
            // Arrange
            var person = new PersonCreationModel
            {
                FirstName = new string('X', 110),
                LastName = "Parker",
                Dob = DateTime.UtcNow,
                Salutation = "Your friendly neighbor"
            };
            var json = this.ConvertToJson<PersonCreationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PostAsync("/api/persons", content);

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Post_HavingPersonInvalidLastName_ShouldReturn400BadRequest()
        {
            // Arrange
            var person = new PersonCreationModel
            {
                FirstName = "Peter",
                LastName = new string('X', 110),
                Dob = DateTime.UtcNow,
                Salutation = "Your friendly neighbor"
            };
            var json = this.ConvertToJson<PersonCreationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PostAsync("/api/persons", content);

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Post_HavingPersonInvalidSalutation_ShouldReturn400BadRequest()
        {
            // Arrange
            var person = new PersonCreationModel
            {
                FirstName = "Peter",
                LastName = "Parker",
                Dob = DateTime.UtcNow,
                Salutation = new string('X', 160)
            };
            var json = this.ConvertToJson<PersonCreationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PostAsync("/api/persons", content);

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Put_HavingValidPerson_ShouldReturn200OK()
        {
            // Arrange
            int id = this.AddPerson();
            var person = new PersonModificationModel
            {
                Id = id,
                FirstName = "Peter",
                LastName = "Parker",
                Dob = DateTime.UtcNow,
                Salutation = "Your friendly neighbor"
            };
            var json = this.ConvertToJson<PersonModificationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PutAsync($"/api/persons/{id.ToString()}", content);

            // Assert
            response.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Put_HavingPersonInvalidFirstName_ShouldReturn400BadRequest()
        {
            // Arrange
            int id = this.AddPerson();
            var person = new PersonModificationModel
            {
                Id = id,
                FirstName = new string('X', 110),
                LastName = "Parker",
                Dob = DateTime.UtcNow,
                Salutation = "Your friendly neighbor"
            };
            var json = this.ConvertToJson<PersonModificationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PutAsync($"/api/persons/{id.ToString()}", content);

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Put_HavingPersonInvalidLastName_ShouldReturn400BadRequest()
        {
            // Arrange
            int id = this.AddPerson();
            var person = new PersonModificationModel
            {
                Id = id,
                FirstName = "Peter",
                LastName = new string('X', 110),
                Dob = DateTime.UtcNow,
                Salutation = "Your friendly neighbor"
            };
            var json = this.ConvertToJson<PersonModificationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PutAsync($"/api/persons/{id.ToString()}", content);

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Put_HavingPersonInvalidSalutation_ShouldReturn400BadRequest()
        {
            // Arrange
            int id = this.AddPerson();
            var person = new PersonModificationModel
            {
                Id = id,
                FirstName = "Peter",
                LastName = "Parker",
                Dob = DateTime.UtcNow,
                Salutation = new string('X', 160)
            };
            var json = this.ConvertToJson<PersonModificationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PutAsync($"/api/persons/{id.ToString()}", content);

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Put_HavingPersonInvalidId_ShouldReturn400BadRequest()
        {
            // Arrange
            int id = this.AddPerson();
            var person = new PersonModificationModel
            {
                Id = id + 1,
                FirstName = "Peter",
                LastName = "Parker",
                Dob = DateTime.UtcNow,
                Salutation = "Your friendly neighbor"
            };
            var json = this.ConvertToJson<PersonModificationModel>(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await this.HttpClient.PutAsync($"/api/persons/{id.ToString()}", content);

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Delete_HavingExistingPerson_ShouldReturn204NoContent()
        {
            // Arrange
            int id = this.AddPerson();

            // Act
            var response = await this.HttpClient.DeleteAsync($"/api/persons/{id.ToString()}");

            // Assert
            response.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_HavingNonExistingPerson_ShouldReturn404NotFound()
        {
            // Arrange
            int id = 1000;

            // Act
            var response = await this.HttpClient.DeleteAsync($"/api/persons/{id.ToString()}");

            // Assert
            response.StatusCode.Should().Be(404);
        }

        private IConfigurationRoot GetConfiguration()
        {
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private void RemoveAddedPersons()
        {
            using (var context = new PersonsDatabaseContext(this.ConnectionString))
            {
                var entities = context.Persons.Where(x => x.Id != 1 && x.Id != 2).ToList();
                foreach (var entity in entities)
                    context.Persons.Remove(entity);
                context.SaveChanges();
            }
        }

        private int AddPerson()
        {
            var newPerson = new Person
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Dob = DateTime.UtcNow,
                Salutation = Guid.NewGuid().ToString()
            };
            EntityEntry<Person> entity = null;
            using (var context = new PersonsDatabaseContext(this.ConnectionString))
            {
                entity = context.Persons.Add(newPerson);
                context.SaveChanges();
            }
            return entity.Entity.Id;
        }

        private string ConvertToJson<T>(T person)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(person, serializerSettings);
        }
    }
}
