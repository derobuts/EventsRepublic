using Dapper;
using EventsRepublic.Context;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using EventsRepublic.Models;
using EventsRepublic.Models.Mpesa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EventsRepublic.Repository
{
    public class EventRespositoryv2 : BaseRepository<Event>
    {
        public async Task<Eventinfo> GeteventDetails(Guid eventid)
        {     
            var Eventinfo = await WithConnection(async c =>
            {
                var E = new DynamicParameters();
                E.Add("eventid", eventid, DbType.Guid);
                var Event = await c.QueryAsync<Eventinfo>(sql: "GeteventDetails"
                    , param: E,
                    commandType: CommandType.StoredProcedure
                    );
                return Event.FirstOrDefault();
            });
            if (Eventinfo.Recurring)
            {
                var constring = await GetCronstring(Eventinfo.IntId);
                Eventinfo.RecurrenceDates = new ShedulerRepository().GetNextEventOcurrences(constring);
                return Eventinfo;
            }
            return Eventinfo;
        }

        public async Task<string> GetCronstring(int id)
        {
            return await WithConnection(async c =>
            {
                var cronstring = await c.QueryAsync<string>(@"select Cron_string from Eventrecurrences where Event_id = @eventid
                                                            ", new { @eventid = id});
                return cronstring.FirstOrDefault();
            });
        }
        public async Task<IEnumerable<Eventinfo>> GetUserEvents(int userid, int status)
        {
           return await WithConnection(async c =>
            {
                var UserEvents = await c.QueryAsync<Eventinfo>(@"select Event_Id as Eventid,Name,ES.Status,E.Startdate,E.Enddate from Event E
                                                            inner join  EventStatus ES on ES.Id = E.Status
                                                            where User_Id = @userid and E.Status = @status
                                                            order by E.Created_On desc", new {@userid = userid,@status = status});
                return UserEvents;
            });
           
        }

        public async Task<IEnumerable<MonthlyStats>> GetMonthlyStats(int eventid,DateTime startdate,DateTime enddate)
        {
            return await WithConnection(async c =>
            {
                var args = new DynamicParameters();
                args.Add("@eventid",eventid, dbType: DbType.Int32);
                args.Add("@startdate",startdate, dbType: DbType.DateTime);
                args.Add("@enddate", startdate, dbType: DbType.DateTime);
                var monthlystats = await c.QueryAsync<MonthlyStats>("GetPopularEventsNearbyCat"
                    ,args,
                    commandType: CommandType.StoredProcedure
                    );
                return monthlystats;
            });

        }

        public async Task<IEnumerable<EventCategory>>EventCategories()
        {
            return await WithConnection(async c =>
            {
                var EventCategory = await c.QueryAsync<EventCategory>(@"select * from EventCategory", CommandType.Text);
                return EventCategory;
            });
        }

       


        public async Task<IEnumerable<EventSubinfo>> PopularByCategory(decimal latitude, decimal longitude, int pageno, int pagesize, int categoryid, int milestosearch)
        {
            return await WithConnection(async c =>
            {
                var args = new DynamicParameters();
                args.Add("@lat", latitude, dbType: DbType.Decimal);
                args.Add("@long", longitude, dbType: DbType.Decimal);
                args.Add("@categoryid", categoryid, dbType: DbType.Int32);
                args.Add("@pageNo", pageno, dbType: DbType.Int32);
                args.Add("@pageSize", pagesize, dbType: DbType.Int32);
                args.Add("@Miles", milestosearch, dbType: DbType.Int32);
                var Results = await c.QueryAsync<EventSubinfo>("GetPopularEventsNearbyCat"
                    , args,
                    commandType: CommandType.StoredProcedure
                    );

                return Results;
            });
        }

        public async Task<IEnumerable<EventSubinfo>> PopularNearby(decimal latitude, decimal longitude)
        {
            return await WithConnection(async c =>
            {
                var args = new DynamicParameters();
                args.Add("@lat", latitude, dbType: DbType.Decimal);
                args.Add("@long", longitude, dbType: DbType.Decimal);
                var Results = await c.QueryAsync<EventSubinfo>("GetPopularEventsNearby2"
                    , args,
                    commandType: CommandType.StoredProcedure
                    );

                return Results;
            });
        }

        public async Task<IEnumerable<T>> GeteventByName<T>(string searchword)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@word", searchword, DbType.String);
                //sqlparams.Add("@lastrecordno", lastrecordno, DbType.Int32);
                //sqlparams.Add("@noofrowsreturn", noofrowsreturn, DbType.Int32);
                //sqlparams.Add("@maxid", DbType.Int32, direction: ParameterDirection.Output);
                var Events = await c.QueryAsync<T>("SearchEventName2"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
               // int maxid = Events.Max(X => X.Eventid);
                return  Events;
            });
        }

        public async void AddEvent(Event eventz)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<Task> ticketclasses = new List<Task>();
                    int Eventid;
                    if (!eventz.venueispresent)
                    {
                        //add venue and get id 
                        VenueRepository venueRepository = new VenueRepository();
                        eventz.venueid = await venueRepository.AddVenue2(eventz.venue);
                    }
                    else
                    {
                        eventz.venueid = eventz.venue.id;
                    }

                    Eventid = await WithConnection(async c =>
                    {
                        var sqlparams = new DynamicParameters();
                        sqlparams.Add("@userid", eventz.user_id, DbType.Int32);
                        sqlparams.Add("@name", eventz.name, DbType.String);
                        sqlparams.Add("@categoryid", eventz.category, DbType.Int32);
                        sqlparams.Add("@visibility", eventz.visibility, DbType.Int32);
                        sqlparams.Add("@photo", eventz.photo, DbType.String);
                        sqlparams.Add("@description", eventz.description, DbType.String);
                        sqlparams.Add("@venueid", eventz.venueid, DbType.Int32);
                        sqlparams.Add("Startdate", eventz.startdate,DbType.DateTime);
                        sqlparams.Add("Enddate", eventz.enddate, DbType.DateTime);
                        sqlparams.Add("@output", DbType.Int32, direction: ParameterDirection.Output);
                        await c.ExecuteAsync("AddEvent",
                        sqlparams,
                        commandType: CommandType.StoredProcedure);
                        int eventid = sqlparams.Get<int>("@output");
                        return eventid;
                    });
                    if (eventz.recurring)
                    {
                        string k = "rec";
                        await AddEventRecurrencePattern(Eventid,k);
                    }
                    else
                    {
                        foreach (var ticketclass in eventz.ticketClasses)
                        {
                            await AddTicketClass2(Eventid, ticketclass);
                        }
                    }
                    
                    //Task.WaitAll(ticketclasses.ToArray());
                  
                    scope.Complete();
                }
             
            }
            catch (Exception ex)
            {
               
               var h = ex;
               
                throw;
            }          
        }
        
        public async Task AddEventRecurrencePattern(int eventid,string cronpattern)
        {
            await WithConnection2(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@event_id", eventid, DbType.Int32);
                sqlparams.Add("@recurrencepattern",cronpattern, DbType.String);
                sqlparams.Add("@endtime", cronpattern, DbType.Time);
                await c.ExecuteAsync("AddRecurrencePattern", sqlparams, commandType: CommandType.StoredProcedure);
            });
        }
        
  
        public async Task AddPerformerEvent(List<PerformerEvent> pe)
        {
            using (var connection = new SqlConnection(new DBCOntext().GetdbConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddPerformerToEvent", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                // GetEventShowingDates2<EventShowings>(eventShowings);
                SqlParameter param = new SqlParameter("@performerevent", PerformerEventDatatable(pe));
                cmd.Parameters.Add(param);
                connection.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

   
   
        //
        private DataTable PerformerEventDatatable(List<PerformerEvent> pe)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("performerid");
            dt.Columns.Add("eventid");
            foreach (var item in pe)
            {
                dt.Rows.Add(item.performerid, item.eventid);
            }
            return dt;
        }
        //ticketclasses

        //
        public async Task AddTicketClass2(int eventid, TicketClass ticketClass)
        {
            await WithConnection2(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@event_id", eventid, DbType.Int32);
                sqlparams.Add("@name", ticketClass.name, DbType.String);
                sqlparams.Add("@max_qtperodr", ticketClass.max_per_order, DbType.Int16);
                sqlparams.Add("@amount", ticketClass.amount, DbType.Decimal);
                sqlparams.Add("@type", ticketClass.type, DbType.Int16);
                sqlparams.Add("@tickettosell", ticketClass.ticketstosell, DbType.Int32);
                sqlparams.Add("@minperorder", ticketClass.min_per_order, DbType.Int16);
                sqlparams.Add("@visibility", ticketClass.visibility, DbType.Int16);
                sqlparams.Add("@startsale", ticketClass.startsale, DbType.DateTime);
                sqlparams.Add("@endsale", ticketClass.endsale, DbType.DateTime);
                sqlparams.Add("@feestype", ticketClass.feestype, DbType.Int32);
                sqlparams.Add("@netprice", ticketClass.netamount, DbType.Decimal);
                sqlparams.Add("@feecharge", ticketClass.fees, DbType.Decimal);
                sqlparams.Add("@additionalinfo", ticketClass.additionalinfo, DbType.String);
                await c.ExecuteAsync("AddTicketClass", sqlparams, commandType: CommandType.StoredProcedure);
            }
        );
        }

        public class PerformerEvent
        {
            public int performerid;
            public int eventid;
        }

        public class Eventinfo
        {
            public Guid Eventid { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public int IntId { get; set; }
            public DateTime Startdate { get; set; }
            public DateTime Enddate { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
            public string Photo { get; set; }
            public bool Recurring { get; set; }
            public IEnumerable<DateTime> RecurrenceDates { get; set; }
            public string PlaceAddress { get; set; }
            public string Timezone { get; set; }
        }

        public class EventHomePageInfo
        {
            public int Eventid { get; set; }
            public string Name { get; set; }
            public DateTime start_date { get; set; }
            public string ThumbnailPhoto { get; set; }
            public string placeaddress { get; set; }
            public string timezone { get; set; }
            public int Category_id { get; set; }
        }
        public class EventCategory
        {
            public int Category_Id { get; set; }
            public string Description { get; set; }
        }

        public class MonthlyStats
        {
            public string month { get; set; }
            public decimal revenue { get; set; }
            public int ticketsbought { get; set; }
            public int ordersbought { get; set; }
            public int prev_month_revenue { get; set; }
            public int prev_month_orders_bought { get; set; }
            public int prev_month_tickets_bought { get; set; }
            public int revenue_percentage_growth{ get; set; }
            public int ordersbought_percentage_growth { get; set; }
            public int ticketsbought_percentage_growth { get; set; }
        }
    }
}
