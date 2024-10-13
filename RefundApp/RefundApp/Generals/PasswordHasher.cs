using Microsoft.AspNetCore.Identity;

namespace RefundApp.Utils
{
    public static class PasswordHasher
    {
        private static readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        public static string HashPassword(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
