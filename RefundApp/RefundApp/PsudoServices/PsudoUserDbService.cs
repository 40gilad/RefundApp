using RefundApp.Models;
using RefundApp.Utils;
using System.Collections.Generic;

namespace RefundApp.PsudoServices
{
    public class PsudoUserDbService
    {
        private static Dictionary<string, UserModel> users;
        private static PsudoUserDbService instance;
        private static readonly object lockObj = new object();

        private PsudoUserDbService()
        {
            users = new Dictionary<string, UserModel>();

        }
        public static PsudoUserDbService Instance()
        {
            lock (lockObj)
            {
                if (instance == null)
                    instance = new PsudoUserDbService();
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

        public void Add(UserModel user)
        {
            if (user != null)
            {
                if (users.ContainsKey(user.UEmail))
                    throw new InvalidOperationException($"User Email {user.UEmail} already exists.");

                users.Add(user.UEmail, user);
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
