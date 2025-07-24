using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace HealthCheckSession2TinVT.BlazorWebApp
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var isLoggedInResult = await _sessionStorage.GetAsync<bool>("isLoggedIn");
                var usernameResult = await _sessionStorage.GetAsync<string>("username");

                if (isLoggedInResult.Success && isLoggedInResult.Value && usernameResult.Success)
                {
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usernameResult.Value ?? ""),
                    new Claim(ClaimTypes.Role, "User")
                }, "CustomAuth"));

                    return new AuthenticationState(claimsPrincipal);
                }
            }
            catch (Exception ex)
            {
                // Log exception nếu cần
                Console.WriteLine($"Auth error: {ex.Message}");
            }

            return new AuthenticationState(_anonymous);
        }

        public async Task MarkUserAsAuthenticated(string username)
        {
            await _sessionStorage.SetAsync("username", username);
            await _sessionStorage.SetAsync("isLoggedIn", true);

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "User")
        }, "CustomAuth"));

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _sessionStorage.DeleteAsync("username");
            await _sessionStorage.DeleteAsync("isLoggedIn");

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
