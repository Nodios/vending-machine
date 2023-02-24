using FastEndpoints;
using FluentValidation.Results;

namespace VendingMachine.API.Infrastructure
{
    public class RequestLogger : IGlobalPreProcessor
    {
        #region Methods

        public Task PreProcessAsync(object req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
        {
            var logger = ctx.Resolve<ILogger>();

            logger.LogInformation($"REQUEST: {req.GetType().FullName} PATH: {ctx.Request.Path}");

            return Task.CompletedTask;
        }

        #endregion Methods
    }

    public class ResponseLogger : IGlobalPostProcessor
    {
        #region Methods

        public Task PostProcessAsync(object req, object? res, HttpContext ctx, IReadOnlyCollection<ValidationFailure> failures, CancellationToken ct)
        {
            var logger = ctx.Resolve<ILogger>();

            logger.LogInformation($"RESPONSE ({ctx.Response.StatusCode}): {req.GetType().FullName} PATH: {ctx.Request.Path}");

            return Task.CompletedTask;
        }

        #endregion Methods
    }
}