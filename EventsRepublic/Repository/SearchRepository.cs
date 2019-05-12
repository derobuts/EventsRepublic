using Dapper;
using EventsRepublic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class SearchRepository
    {
        //add constructor overloading later to enable different serach params
        public async Task<IEnumerable<EventSubinfo>> GetSearchQuery(string query)
        {

            var EventTaskSearch = await new EventRespositoryv2().GeteventByName(query);

            return EventTaskSearch;

        }       
    }
    //check if ienum null or empty
    public static class IenumerableCheck
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
