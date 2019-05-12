using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
   public interface IIAuthentication
    {
        Task<int> AddUser();
        Task<bool> LoginUser();
        Task<bool> ISEmailAddres();
        Task<int> GetUserID(int User);
        Task<int> UpdateUser(int User);
    }
}
