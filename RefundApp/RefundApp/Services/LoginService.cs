using Microsoft.EntityFrameworkCore;
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

        public async Task<List<UserModel>?> Get()
        {
            return await _context.Users.ToListAsync();
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

        public async Task<bool> Delete(int id)
        {
            UserModel? u = await Get(id);
            if (u is null)
                return false;
            return await Delete(u);
        }

        public async Task<bool> Delete(UserModel user)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> Update(UserModel user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
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
