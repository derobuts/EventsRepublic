using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class LnmDictionary
    {
        //static readonly TimeSpan timeSpan;
        static readonly ConcurrentDictionary<string, int> lnm = new ConcurrentDictionary<string, int>(); 
        public int GetKey(string Key)
        {
            int value;
           
            if (lnm.ContainsKey(Key))
            {
                if (lnm.ContainsKey(Key))
                {
                    lnm.TryRemove(Key, out value);
                    return value;
                }
                return default(int);
            }
            return -1;
        }
    }
}
