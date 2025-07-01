using SMMS.Services.TinVT;

namespace SMMS.Services.TinVT.Utilities
{
    public static class PasswordUtility
    {
        /// <summary>
        /// Utility method to hash existing plain text passwords
        /// Call this method from a controller or console app to migrate existing passwords
        /// </summary>
        public static string HashPassword(string plainTextPassword)
        {
            var authService = new AuthenticationService(new UserAccountService());
            return authService.HashPassword(plainTextPassword);
        }

        /// <summary>
        /// Utility method to verify if a password matches a hash
        /// </summary>
        public static bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            var authService = new AuthenticationService(new UserAccountService());
            return authService.VerifyPassword(plainTextPassword, hashedPassword);
        }
    }
}