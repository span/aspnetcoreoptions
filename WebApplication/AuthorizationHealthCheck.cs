using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    /// <summary>
    /// REST API protected endpoint health check
    /// </summary>
    public class AuthorizationHealthCheck : IHealthCheck
    {
        private readonly ILogger<AuthorizationHealthCheck> _logger;
        private readonly HttpClient _client;

        public AuthorizationHealthCheck(IHttpClientFactory httpClientFactory, ILogger<AuthorizationHealthCheck> logger)
        {
            this._logger = logger;
            this._client = httpClientFactory.CreateClient(nameof(AuthorizationHealthCheck));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initiating healtcheck authorization endpoint");

            var response = await this._client.GetAsync("https://localhost:5001/weatherforecast", cancellationToken);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Healthcheck failed with status code {response.StatusCode}");
                return await Task.FromResult(HealthCheckResult.Unhealthy());
            }

            _logger.LogInformation("Healthcheck completed, reporting healthy.");

            return await Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
