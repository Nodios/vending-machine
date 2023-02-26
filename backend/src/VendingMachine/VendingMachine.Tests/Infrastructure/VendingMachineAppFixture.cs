using Bogus;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FastEndpoints;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace VendingMachine.Tests.Infrastructure
{
    public class VendingMachineAppFixture : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        #region Fields

        private readonly TestcontainerDatabase _database = new ContainerBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "vending_machine",
                Username = "root",
                Password = "root"
            }).Build();

        private HttpClient? _authClient;
        private HttpClient? _client;

        #endregion Fields

        #region Properties

        public HttpClient AuthClient => _authClient ??= CreateClient();
        public HttpClient Client => _client ??= CreateClient();

        #endregion Properties

        #region Methods

        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();
            await _database.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _database.StartAsync();

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VendingMachineDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public async Task<Membership.Login.Response> LoginAsync(string username, string password, bool rememberMe = false)
        {
            var (response, result) = await Client.POSTAsync<Membership.Login.Endpoint, Membership.Login.Request, Membership.Login.Response>(new Membership.Login.Request
            {
                Username = username,
                Password = password,
                RememberMe = rememberMe
            });

            if (response == null || !response.IsSuccessStatusCode)
            {
                throw new Exception("Login failed.");
            }

            AuthClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);

            return result;
        }

        public async Task SeedRoleAsync(string name)
        {
            var scope = Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Domain.Identity.Role>>();

            var faker = new Faker();
            var id = faker.Random.Uuid();

            var role = new Domain.Identity.Role
            {
                Id = id.ToString(),
                Name = name
            };

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to seed role.");
            }
        }

        public async Task<(string Username, string Password)> SeedUserAsync(string role)
        {
            var scope = Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Domain.Identity.User>>();

            var faker = new Faker();
            var id = faker.Random.Uuid();
            var email = faker.Person.Email;
            var username = faker.Person.UserName;
            var password = faker.Internet.Password();

            var user = new Domain.Identity.User
            {
                Id = id.ToString(),
                Email = email,
                UserName = username,
                EmailConfirmed = true,
                Deposit = 0,
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to seed user.");
            }

            await userManager.AddToRoleAsync(user, role);

            return (username, password);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(l => l.ClearProviders());

            builder.ConfigureAppConfiguration(c =>
            {
                var config = new Dictionary<string, string>
                {
                    {"ConnectionStrings:DefaultConnection", _database.ConnectionString},
                    {"Auth:Secret", "GgREWfgrsf43424fg#GG"},
                    {"Settings:AcceptedCoins", "[5,10,20,50,100]" },
                };

                c.AddInMemoryCollection(config);
            });
        }

        #endregion Methods
    }
}