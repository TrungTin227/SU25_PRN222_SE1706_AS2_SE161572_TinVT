using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
namespace HealthCheckSession2TinVT.BlazorWebApp
{
    public class CustomAuthenticationSchemeOptions : AuthenticationSchemeOptions { }

    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationSchemeOptions>
    {
        public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Không xử lý ở đây vì chúng ta dùng AuthenticationStateProvider
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
