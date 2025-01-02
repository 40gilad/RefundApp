using RefundApp.Models;
using RefundApp.Data;
using RefundApp.Utils;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RefundApp.PsudoServices
{
    public class PsudoUserDbService
    {
        private static Dictionary<string, UserModel> users;
        private static PsudoUserDbService instance;
        private static readonly object lockObj = new object();
        private readonly RefundAppDbContext _context;
        private PsudoUserDbService(RefundAppDbContext context)
        {
            users = new Dictionary<string, UserModel>();
            _context = context;
        }
        public static PsudoUserDbService Instance(RefundAppDbContext context=null)
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    if (context is null)
                        throw new ArgumentNullException(nameof(context));
                    instance = new PsudoUserDbService(context);
                }
                return instance;
            }
        }

        public Dictionary<string, UserModel> Get()
        {
            return users;
        }

        public UserModel Get(string user_mail)
        {
            if (!users.ContainsKey(user_mail))
                throw new KeyNotFoundException($"No user found with Email: {user_mail}");
            return users[user_mail];
        }

        public async Task Add(UserModel user)
        {
            if (user != null)
            {
                if (users.ContainsKey(user.UEmail))
                    throw new InvalidOperationException($"User Email {user.UEmail} already exists.");

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        public void Update(UserModel u)
        {
            if (u == null)
                throw new NullReferenceException();
            else users[u.UEmail].UName = u.UName;
        }

        public void Remove(string user_mail)
        {
            if (!users.ContainsKey(user_mail))
                throw new KeyNotFoundException($"No user found with Email: {users[user_mail].UEmail}");
            users.Remove(user_mail);
        }
    }
}

/*
 {
  "id": 11,
  "uName": "admin",
  "uEmail": "a@b.com",
  "uPassword": "123"
}
*/
