using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Interface
{
    interface IDatabaseInterface<T> where T :new ()
    {
        Task<int> InsertAsync();
        Task<int> UpdateAsync();
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsyncId(int id);
        void DeleteAsync();
    }

}
