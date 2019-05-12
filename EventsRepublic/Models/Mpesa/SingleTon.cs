using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public sealed class Singleton
    {
        private static readonly Singleton instance = new Singleton();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }
    }
    }
