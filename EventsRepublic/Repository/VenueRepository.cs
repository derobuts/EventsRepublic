using Dapper;
using EventsRepublic.Database;
using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class VenueRepository : BaseRepository
    {
        
        //get event venue by name
        public async Task<IEnumerable<VenueSearch>> GetVenueEvents(string searchword, int lastrecordno, int noofrowsreturn)
        {
            return await WithConnection(async c => {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@word", searchword, DbType.String);
                sqlparams.Add("@lastrecordno", lastrecordno, DbType.Int32);
                sqlparams.Add("@noofrowsreturn", noofrowsreturn, DbType.Int32);
                var venuesearch = await c.QueryAsync<VenueSearch>("SearchVenueName"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return venuesearch;

            });
        }
        //
        public async Task<int> AddVenue2(Venue venue)
        {
            return await WithConnection<int>(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@latitude", venue.latitude, DbType.Decimal);
                sqlparams.Add("@longitude", venue.longitude, DbType.Decimal);
                sqlparams.Add("@city", venue.city, DbType.String);
                sqlparams.Add("@country", venue.country, DbType.String);
                sqlparams.Add("@timezone", venue.timezone, DbType.String);
                sqlparams.Add("@placeaddress", venue.placeaddress, DbType.String);
                sqlparams.Add("@output", DbType.Int32, direction: ParameterDirection.Output);
                await c.ExecuteAsync("AddVenue", sqlparams, commandType: CommandType.StoredProcedure);
                int OutPut = sqlparams.Get<int>("@output");
                return OutPut;
            }
            );
        }
        public async Task<int> AddVenue(Venue venue)
        {
            using (var connection = new SqlConnection(new DBCOntext().GetdbConnectionString()))
            {
                connection.Open();
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@latitude", venue.latitude, DbType.Decimal);
                sqlparams.Add("@longitude", venue.longitude, DbType.Decimal);
                sqlparams.Add("@city", venue.city, DbType.String);
                sqlparams.Add("@country", venue.country, DbType.String);
                sqlparams.Add("@timezone", venue.timezone, DbType.String);
                sqlparams.Add("@placeaddress", venue.placeaddress, DbType.String);
                sqlparams.Add("@output", DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("AddVenue",sqlparams, commandType: CommandType.StoredProcedure);
                int OutPut = sqlparams.Get<int>("@output");
                return OutPut;
            }
        }
        //get event venue by name
        public async Task<IEnumerable<T>> GetVenueByName<T>(string searchword, int lastrecordno, int noofrowsreturn)
       {
           return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@word", searchword, DbType.String);
                sqlparams.Add("@lastrecordno", lastrecordno, DbType.Int32);
                sqlparams.Add("@noofrowsreturn", noofrowsreturn, DbType.Int32);
                var venuesearch = await c.QueryAsync<T>("SearchVenueName"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return venuesearch;     
            });
        }
    }
    public class Venue
    {
     public int id { get; set; }
     public decimal latitude { get; set; }
     public decimal longitude { get; set; }
     public string country { get; set;}
     public string city { get; set; }
     public string placeaddress { get; set; }
     public bool venueispresent { get; set; }
     public string timezone { get; set; }
    }
}
