using EventsRepublic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
    public interface IJwtTokenService
    {
        Token CreateToken(string Id);
    }
}
