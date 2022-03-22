using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Net.Http;
using ToDoTask.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ToDoTask.IntegrationTest
{
    public class ToDoTaskControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        public ToDoTaskControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<ToDoTaskDbContext>));

                        services
                            .Remove(dbContextOptions);

                        services
                            .AddDbContext<ToDoTaskDbContext>(options => options.UseInMemoryDatabase("ToDoDB"));
                    });
                });
            _client = _factory.CreateClient();
        }

        private void SeedToDoTask(ToDo toDo)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<ToDoTaskDbContext>();

            _dbContext.ToDos.Add(toDo);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GettAll_ReturnOkResult()
        {
            //act
            var response = await _client.GetAsync("/todotask");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnOkResult()
        {
            //arrange
            ToDo toDo = new ToDo()
            {
                DateAndTimeOfExiry = new DateTime(2022, 3, 24, 16, 00, 00),
                Title = "Creat web appliction",
                Description = "Application about book collection",
                PercentComplete = 0,
                IsDone = false,
            };

            SeedToDoTask(toDo);

            //act
            var resonse = await _client.GetAsync("/todotask/" + toDo.Id);

            //assert
            resonse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(62)]
        [InlineData(50)]
        public async Task GetById_WithId_ReturnNotFoundResult(int id)
        {
            //act
            var resonse = await _client.GetAsync("/todotaks/" + id);

            //assert
            resonse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("query=today")]
        [InlineData("query=next day")]
        public async Task GetIncomingDays_GethQueryDays_ReturnOkResult(string query)
        {
            //act
            var response = await _client.GetAsync("/todotask/incomingdays?" + query);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("week")]
        [InlineData("sunday")]
        public async Task GetIncomingDays_GetInvalidWithQueryDays_ReturnBadRequestResult(string query)
        {
            //act
            var response = await _client.GetAsync("/todotask/incomingdays?query=" + query);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreatToDo_GetNewToDo_ReturnCreatedResult()
        {
            var model = new ToDo()
            {
                DateAndTimeOfExiry = new DateTime(2022, 3, 24, 16, 00, 00),
                Title = "Creat web appliction",
                Description = "Application about book collection",
                PercentComplete = 0,
                IsDone = false,
            };
            var json = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var reponse = await _client.PostAsync("/todotask", httpContent);

            reponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task DelateToDo_WithActiveToDo_ReturnNoContentResult()
        {
            var model = new ToDo()
            {
                DateAndTimeOfExiry = new DateTime(2022, 3, 24, 16, 00, 00),
                Title = "Creat web appliction",
                Description = "Application about book collection",
                PercentComplete = 0,
                IsDone = false,
            };
            SeedToDoTask(model);

            var resopnse = await _client.DeleteAsync("/todotask/" + model.Id);

            resopnse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DelateToDo_WithoutToDo_ReturnNotFoundResult()
        {
            var resopnse = await _client.DeleteAsync("/todotask/10");

            resopnse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
