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

        public UserModel Get(string user_id)
        {
            if (!users.ContainsKey(user_id))
                throw new KeyNotFoundException($"No refund found with OrderId: {user_id}");
            return users[user_id];
        }

        public void Add(UserModel user)
        {
            if (user != null)
            {
                if (users.ContainsKey(user.Uid))
                    throw new InvalidOperationException($"User ID {user.Uid} already exists.");

                users.Add(user.Uid, user);
            }
        }

        public void Update(UserModel u)
        {
            if (u == null)
                throw new NullReferenceException();
            else users[u.Uid].UName = u.UName;
        }

        public void Remove(string user_id)
        {
            if (!users.ContainsKey(user_id))
                throw new KeyNotFoundException($"No user found with UserId: {user_id}");
            users.Remove(user_id);
        }
    }
}
