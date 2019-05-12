using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Serializers
{
    public class JsonSerializerDeserializer
    {
        public string ToJson(T t)
        {
            try
            {
                string jsonstr = JsonConvert.SerializeObject(t);
                return jsonstr;
            }
            catch (Exception ex)
            {
                var h = ex;              
            }
            return "error";
        }
        public T ToObject<T>(string t)
        {
            try
            {
                T d = JsonConvert.DeserializeObject<T>(t);
                return d;
            }
            catch (Exception ex)
            {
                var h = ex;
                throw;
            }
          
        }
    }
}
