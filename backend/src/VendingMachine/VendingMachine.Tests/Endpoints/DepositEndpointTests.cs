using FastEndpoints;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using VendingMachine.Domain.Configuration;
using VendingMachine.Tests.Infrastructure;

namespace VendingMachine.Tests.Endpoints
{
    public class DepositEndpointTests : IClassFixture<VendingMachineAppFixture>
    {
        private readonly VendingMachineAppFixture _fixture;
        private readonly HttpClient _client;

        public DepositEndpointTests(VendingMachineAppFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.Client;
        }

        [Fact]
        public async Task Fails_for_unauthenticated()
        {
            var response = await _fixture.Client.POSTAsync<Payments.Deposit.Endpoint, Payments.Deposit.Request>(new Payments.Deposit.Request
            {
                Coins = new[] { 5, 5 },
                UserId = ""
            });

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Fails_for_non_buyer_role()
        {
            var (username, password) = await _fixture.SeedUserAsync(RoleNames.SELLER);
            var loginResult = await _fixture.LoginAsync(username, password);

            var response = await _fixture.AuthClient.POSTAsync<Payments.Deposit.Endpoint, Payments.Deposit.Request>(new Payments.Deposit.Request
            {
                Coins = new[] { 5, 5 },
                UserId = loginResult.Id,
            });

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Fails_when_no_coins_are_inserted()
        {
            var (username, password) = await _fixture.SeedUserAsync(RoleNames.BUYER);
            var loginResult = await _fixture.LoginAsync(username, password);

            var (response, result) = await _fixture.Client.POSTAsync<Payments.Deposit.Endpoint, Payments.Deposit.Request, ErrorResponse>(new Payments.Deposit.Request
            {
                Coins = new List<int>(),
                UserId = loginResult!.Id,
            });


            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result!.Errors.Keys.Should().Contain("Coins");
            result!.Errors["Coins"].Should().Equal("Insert at least one coin.");
        }

        [Fact]
        public async Task Fails_when_invalid_coin_denomination_is_added()
        {
            var (username, password) = await _fixture.SeedUserAsync(RoleNames.BUYER);
            var loginResult = await _fixture.LoginAsync(username, password);

            var (response, result) = await _fixture.Client.POSTAsync<Payments.Deposit.Endpoint, Payments.Deposit.Request, ErrorResponse>(new Payments.Deposit.Request
            {
                Coins = new[] { 1 },
                UserId = loginResult!.Id,
            });


            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result!.Errors.Keys.Should().Contain("Coins[0]");
            result!.Errors["Coins[0]"].Should().Equal("Invalid denomination. Accepted are: 5, 10, 20, 50, 100");
        }

        [Fact]
        public async Task Fails_for_each_denomination_at_their_position()
        {
            var (username, password) = await _fixture.SeedUserAsync(RoleNames.BUYER);
            var loginResult = await _fixture.LoginAsync(username, password);

            var (response, result) = await _fixture.Client.POSTAsync<Payments.Deposit.Endpoint, Payments.Deposit.Request, ErrorResponse>(new Payments.Deposit.Request
            {
                Coins = new[] { 1, 5, 1 },
                UserId = loginResult!.Id,
            });


            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result!.Errors.Keys.Should().Contain("Coins[0]", "Denomination at index 0 is invalid");
            result!.Errors["Coins[0]"].Should().Equal("Invalid denomination. Accepted are: 5, 10, 20, 50, 100");
            result!.Errors.Keys.Should().Contain("Coins[2]", "Denomination at index 2 is invalid");
            result!.Errors["Coins[2]"].Should().Equal("Invalid denomination. Accepted are: 5, 10, 20, 50, 100");
        }

    }
}
