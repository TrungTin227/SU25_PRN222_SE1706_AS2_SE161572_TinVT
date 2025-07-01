using SMMS.Repositories.TinVT.Models;
using BCrypt.Net;

namespace SMMS.Services.TinVT
{
    public interface IAuthenticationService
    {
        Task<UserAccount?> AuthenticateAsync(string userName, string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        bool IsValidPassword(string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserAccountService _userAccountService;
        private readonly Dictionary<string, List<DateTime>> _loginAttempts = new();
        private readonly object _lockObject = new();

        public AuthenticationService(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public async Task<UserAccount?> AuthenticateAsync(string userName, string password)
        {
            // Check rate limiting
            if (IsRateLimited(userName))
            {
                return null;
            }

            // Get user from database
            var user = await _userAccountService.GetUserAccount(userName, password);
            
            // If user not found with plain password, try to find by username and verify hashed password
            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
                user = await _userAccountService.GetUserByUserName(userName);
                if (user == null || string.IsNullOrEmpty(user.UserName) || !user.IsActive)
                {
                    RecordFailedAttempt(userName);
                    return null;
                }

                // Check if password matches (either plain text for backward compatibility or hashed)
                if (!VerifyPassword(password, user.Password))
                {
                    RecordFailedAttempt(userName);
                    return null;
                }
            }
            else if (!user.IsActive)
            {
                RecordFailedAttempt(userName);
                return null;
            }

            // Authentication successful - clear failed attempts
            ClearFailedAttempts(userName);
            return user;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Handle backward compatibility - if password is not hashed, compare directly
            if (!hashedPassword.StartsWith("$2") && !hashedPassword.StartsWith("$2a") && 
                !hashedPassword.StartsWith("$2b") && !hashedPassword.StartsWith("$2y"))
            {
                return password == hashedPassword;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                // If verification fails, try direct comparison for backward compatibility
                return password == hashedPassword;
            }
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Basic password policy: at least 6 characters
            return password.Length >= 6;
        }

        private bool IsRateLimited(string userName)
        {
            lock (_lockObject)
            {
                if (!_loginAttempts.ContainsKey(userName))
                    return false;

                var attempts = _loginAttempts[userName];
                var cutoffTime = DateTime.UtcNow.AddMinutes(-15); // 15-minute window

                // Remove old attempts
                attempts.RemoveAll(attempt => attempt < cutoffTime);

                // Check if too many attempts (5 attempts in 15 minutes)
                return attempts.Count >= 5;
            }
        }

        private void RecordFailedAttempt(string userName)
        {
            lock (_lockObject)
            {
                if (!_loginAttempts.ContainsKey(userName))
                {
                    _loginAttempts[userName] = new List<DateTime>();
                }

                _loginAttempts[userName].Add(DateTime.UtcNow);
            }
        }

        private void ClearFailedAttempts(string userName)
        {
            lock (_lockObject)
            {
                if (_loginAttempts.ContainsKey(userName))
                {
                    _loginAttempts[userName].Clear();
                }
            }
        }
    }
}