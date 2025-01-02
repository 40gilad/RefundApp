using RefundApp.Data;
using RefundApp.Models;
using RefundApp.Utils;

namespace RefundApp.Services
{
    public class LoginService
    {
        private readonly RefundAppDbContext _context;
        public LoginService(RefundAppDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        public async Task Add(UserModel user)
        {
            if (user != null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserAuth(UserModel user)
        {
            UserModel storedUser = await _context.Users.FindAsync(user.Id);
            if (storedUser is null)
                return false;
            var hashedPassword = PasswordHasher.HashPassword(user.UPassword);

            return PasswordHasher.VerifyPassword(storedUser.UPassword, user.UPassword);
        }
    }
}
