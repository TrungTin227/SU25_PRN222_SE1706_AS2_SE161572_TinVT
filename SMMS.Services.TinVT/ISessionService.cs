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
        void ExtendSession();
    }

    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggedIn()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session?.GetString("IsLoggedIn") != "true")
                return false;

            // Check if session has expired (30 minutes from login time)
            var loginTimeStr = session.GetString("LoginTime");
            if (DateTime.TryParse(loginTimeStr, out var loginTime))
            {
                if (DateTime.UtcNow.Subtract(loginTime).TotalMinutes > 30)
                {
                    // Session expired, clear it
                    ClearSession();
                    return false;
                }
            }

            return true;
        }

        public string GetUserId()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("UserId") ?? string.Empty;
        }

        public string GetUserName()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("UserName") ?? string.Empty;
        }

        public string GetFullName()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("FullName") ?? string.Empty;
        }

        public void SetUserSession(string userId, string userName, string fullName = "")
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetString("UserId", userId);
                session.SetString("UserName", userName);
                session.SetString("IsLoggedIn", "true");
                session.SetString("LoginTime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                if (!string.IsNullOrEmpty(fullName))
                    session.SetString("FullName", fullName);
            }
        }

        public void ClearSession()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Clear();
        }

        public void ExtendSession()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null && IsLoggedIn())
            {
                session.SetString("LoginTime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}