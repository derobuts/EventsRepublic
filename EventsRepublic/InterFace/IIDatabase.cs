using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
    public interface IIDatabase<T>
    {
        Task<T> Get { get; }
        Task<bool> Update();
        Task<T> GetId(int t);
        Task<bool> DeleteId(int t);
    }
}
