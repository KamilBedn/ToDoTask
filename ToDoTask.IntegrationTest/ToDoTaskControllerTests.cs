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

namespace ToDoTask.IntegrationTest
{
    public class ToDoTaskControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;

        public ToDoTaskControllerTests(WebApplicationFactory<Startup> factory, ToDoTaskDbContext context)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {

                        var dbContextOptions = services
                            .SingleOrDefault(services => services.ServiceType == typeof(ToDoTaskDbContext));

                        services
                            .Remove(dbContextOptions);

                        services
                            .AddDbContext<ToDoTaskDbContext>();
                     });
                })
            .CreateClient();
        }



        [Fact]
        public async Task GettAll_ReturnOkResult()
        {
            //act
            var resonse = await _client.GetAsync("/todotask");

            //assert
            resonse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetById_WithIdInData_ReturnOkResult(int id)
        {
            //act
            var resonse = await _client.GetAsync($"/todotaks/" + id);

            //assert
            resonse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
