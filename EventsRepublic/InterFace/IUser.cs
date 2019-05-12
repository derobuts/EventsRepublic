using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
    public interface IUser 
    {
        Task<int> AddUser(UserInfo User);
        IEnumerable<UserInfo> GetUsers();
        Task<bool> Login(UserInfo User);
        Task<bool> ISEmailAddress(UserInfo User);
        Task<int> GetUserID(int User);
        Task<string> GetRole(int User);
        Task<int> UpdateUser(int User);
        void Save();
    }
}
