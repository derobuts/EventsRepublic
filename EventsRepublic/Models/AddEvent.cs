using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class AddEvent
    {
            public int user_id;
            public string name;
            public int category_id;
            public DateTime startdate;
            public DateTime enddate;
            public int status;
            public int visibility;
            public string largephoto;
            public string thumbnailphoto;
            public string description;
            public EventShowings[] eventshowings;
            
    }
}
