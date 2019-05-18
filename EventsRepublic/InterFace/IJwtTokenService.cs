using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
    public interface IJwtTokenService
    {
        string CreateToken(string email);
    }
}
