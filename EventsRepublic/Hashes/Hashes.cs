using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EventsRepublic.Hashes
{
    public static class Hashes
    {
        private static byte[] Key()
        {
            var key = new byte[64];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            return key;
        }
       

    }
}
