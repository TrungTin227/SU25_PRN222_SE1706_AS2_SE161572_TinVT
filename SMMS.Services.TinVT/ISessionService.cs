using Microsoft.AspNetCore.Http;

namespace SMMS.Services.TinVT
{
    public interface ISessionService
    {
        bool IsLoggedIn();
        string GetUserId();
        string GetUserName();
        string GetFullName();
        void SetUserSession(string userId, string userName, string fullName = "");
        void ClearSession();
    }

    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggedIn() => _httpContextAccessor.HttpContext?.Session?.GetString("IsLoggedIn") == "true";
        public string GetUserId() => _httpContextAccessor.HttpContext?.Session?.GetString("UserId") ?? string.Empty;
        public string GetUserName() => _httpContextAccessor.HttpContext?.Session?.GetString("UserName") ?? string.Empty;
        public string GetFullName() => _httpContextAccessor.HttpContext?.Session?.GetString("FullName") ?? string.Empty;

        public void SetUserSession(string userId, string userName, string fullName = "")
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString("UserId", userId);
            session?.SetString("UserName", userName);
            session?.SetString("IsLoggedIn", "true");
            session?.SetString("LoginTime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(fullName))
                session?.SetString("FullName", fullName);
        }

        public void ClearSession() => _httpContextAccessor.HttpContext?.Session?.Clear();
    }
}
