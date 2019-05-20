using EventsRepublic.InterFace;
using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class User : IUser
    {
        public Task<int> AddUser(UserInfo User)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRole(int User)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUserID(int User)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserInfo> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ISEmailAddress(UserInfo User)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Login(UserInfo User)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateUser(int User)
        {
            throw new NotImplementedException();
        }
    }
}
