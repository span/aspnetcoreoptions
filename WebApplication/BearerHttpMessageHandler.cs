using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

namespace WebApplication
{
    /// <summary>
    /// Creates an authenticated HttpMessageHandler
    /// </summary>
    public class BearerHttpMessageHandler : DelegatingHandler
    {
        private readonly string scope;
        private readonly ITokenAcquisition tokenAcquisition;

        public BearerHttpMessageHandler(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            this.scope = configuration["Scope"];
            this.tokenAcquisition = tokenAcquisition;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await tokenAcquisition.GetAccessTokenForAppAsync(this.scope, authenticationScheme: "Bearer");

            request.Headers.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
