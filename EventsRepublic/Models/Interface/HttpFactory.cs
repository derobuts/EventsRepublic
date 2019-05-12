using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Interface
{
    public interface HttpFactory<H>
    {
        Task<H> GetAsync();
        Task<H> PostAsync();
      
    }
}
