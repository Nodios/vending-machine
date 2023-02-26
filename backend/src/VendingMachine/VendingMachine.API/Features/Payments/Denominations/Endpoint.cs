using FastEndpoints;

namespace Payments.Denominations
{
    public class Endpoint : EndpointWithoutRequest<Response>
    {
        #region Methods

        public override void Configure()
        {
            Get("payments/denominations");
        }

        public override Task HandleAsync(CancellationToken c)
        {
            var denominations = Config.GetSection("Settings:AcceptedCoins").Get<int[]>() ?? Array.Empty<int>();
            Response.Denominations = denominations;

            return SendAsync(Response);
        }

        #endregion Methods
    }
}