using Dapper;
using EventsRepublic.Models;
using EventsRepublic.Models.Mpesa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class EventRespisotory
    {
        /**
         * 100 - Perfomance and concerts
         * 101 - Arts$Theatre
         * 102 - Sports
         * 103 - Movie
         * 104 - Workshop and classes
         * 105 - Food
         * 106 - Nature and Parks
         * 107- Parties and NightLife
         * 108 - Attractions
         * 109 - Fashion**/
        public async Task<string> GetHomepageTopSellingEvents(decimal latitude, decimal longitude)
        {
            int miles = 250;
            //List<EventSubinfo> Homepagedata = new List<EventSubinfo>();
            HomePageData homePageData = new HomePageData();
            
            var Event = new EventRespositoryv2();
            var Perfomanceconcert = Event.PopularByCategory(latitude, longitude, 0, 4, 100, 230);
            var ArtTheatre = Event.PopularByCategory(latitude, longitude, 0, 4, 101, 230);
            var Sports = Event.PopularByCategory(latitude, longitude, 0, 4, 102, 230);
            var Workshopclasses = Event.PopularByCategory(latitude, longitude, 0, 4, 104, 230);

            var homepagedata = await Task.WhenAll(Perfomanceconcert, ArtTheatre, Sports, Workshopclasses);

            return JsonConvert.SerializeObject(homepagedata);
           
        }
     
       
        //lat/long
        public async Task<List<EventSubinfo>> PopularEventsnearby(decimal latitude,decimal longitude,int pageno,int pagesize,int category)
        {
            var Lat = new SqlParameter("@lat", SqlDbType.Decimal);
            Lat.Value = latitude;
            var Longitude = new SqlParameter("@long", SqlDbType.Decimal);
            Longitude.Value = longitude;

            var PageNo = new SqlParameter("@pageNo", SqlDbType.Int);
            PageNo.Value = pageno;

            var pageSize = new SqlParameter("@pageSize", SqlDbType.Int);
            pageSize.Value = pagesize;

            var _context = new DBContext(new Database.DBCOntext(), "GetPopularEventsNearbyv2", Lat, Longitude,PageNo,pageSize);

            var result =  await _context.GetAsync<EventSubinfo>();
            
            return result.ToList();
        }
        //
        public async Task<string> Getevent(int eventid)
        {
            var Id = new SqlParameter("@eventid", SqlDbType.Int);
            Id.Value = eventid;
            Task[] tasks = new Task[2];
            var task = this.GetEventdetails(eventid);
            var task1 = this.GetEventDates(eventid);
            await Task.WhenAll(task,task1);
            var eventdetail =  task.Result;
            eventdetail.eventDates =  task1.Result;
            return JsonConvert.SerializeObject(eventdetail);
           
        }
        public async Task<EventDetails> GetEventdetails(int eventid)
        {
            var Id = new SqlParameter("@eventid",SqlDbType.Int);
            Id.Value = eventid;
            var _context = new DBContext(new Database.DBCOntext(),"GetEvent",Id);
            var result = await _context.GetWithID<EventDetails>();
            return result;
        }
        //
        public async Task<IEnumerable<EventSubinfo>> PopularByCategory(decimal latitude,decimal longitude,int pageno,int pagesize,int categoryid,int milestosearch)
        {
            var Latitude = new SqlParameter("@lat", SqlDbType.Decimal);
            Latitude.Value = latitude;
            var Longitude = new SqlParameter("@long", SqlDbType.Decimal);
            Longitude.Value = longitude;
            var Categoryid = new SqlParameter("@categoryid", SqlDbType.Int);
            Categoryid.Value = categoryid;
            var PageNo = new SqlParameter("@pageNo", SqlDbType.Int);
            PageNo.Value = pageno;
            var PageSize = new SqlParameter("@PageSize", SqlDbType.Int);
            PageSize.Value = pagesize;
            var MilesToSearch = new SqlParameter("@Miles", SqlDbType.Int);
            MilesToSearch.Value = milestosearch;//default 230 but user can pre-define a smaller search area
            var _context = new DBContext(new Database.DBCOntext(), "GetPopularEventsNearbyCat",Latitude,Longitude,PageNo,PageSize,Categoryid,MilesToSearch);
            var result = await _context.GetAsync<EventSubinfo>();
            return result;
        }
        //
        public void UpdateEvent(int id)
        {

        }
        //
        public async Task<IEnumerable<EventSubinfo>> HappeningThisWeekend(decimal latitude,decimal longitude,int pageno,int pagesize)
        {
            var Lat = new SqlParameter("@lat", SqlDbType.Decimal);
            Lat.Value = latitude;

            var Longitude = new SqlParameter("@long", SqlDbType.Decimal);
            Longitude.Value = longitude;

            var PageNo = new SqlParameter("@pageNo", SqlDbType.Int);
            PageNo.Value = 10;

            var PageSize = new SqlParameter("@PageSize", SqlDbType.Int);
            PageSize.Value = pagesize;

            var _context = new DBContext(new Database.DBCOntext(), "HappeningThisWeekend", Lat, Longitude, PageNo, PageSize);
            var result = await _context.GetAsync<EventSubinfo>();
            return result;
        }
        //
        public int deleteEventAsync(int id)
        {

            var Eventid = new SqlParameter("@id", SqlDbType.Int);
            Eventid.Value = id;

            var Output = new SqlParameter("@output", SqlDbType.Int);
            Output.Direction = ParameterDirection.Output;

            var _context = new DBContext(new Database.DBCOntext(), "DeleteEvent", Eventid, Output);
            _context.outputparampresent = true;
           _context.DeleteAsync();
            return _context.outputparam;
        }

        //
       
        //
        public void SaveImage(int id)
        {

        }
        //
        public void DeleteEventImage(int id)
        {

        }
        //
        public async Task<int> LockTicketsAsync(int id,int noofticketstoreserve)
        {
            var EventId = new SqlParameter("@eventid", SqlDbType.Int);
            EventId.Value = id;

            var NoTicketToLock = new SqlParameter("@noticketstolock", SqlDbType.Int);
            NoTicketToLock.Value = noofticketstoreserve;

            var _context = new DBContext(new Database.DBCOntext(), "TicketsToLock",NoTicketToLock,EventId);
            await _context.UpdateAsync();
            _context.outputparampresent = true;
            return _context.outputparam;
        }

      
        public async Task AddEventShowingsDates(int eventid,EventShowings eventShowings)
        {/*
            try
            {
                var Eventid = new SqlParameter("@event_id", SqlDbType.Int);
                Eventid.Value = eventid;
                var startdate = new SqlParameter("@start_date", SqlDbType.DateTime);
                startdate.Value = eventShowings.start_date;
                var enddate = new SqlParameter("@end_date", SqlDbType.DateTime);
                enddate.Value = eventShowings.end_date;

                var country = new SqlParameter("@country", SqlDbType.NChar, 3);
                country.Value = eventShowings.country;

                var city = new SqlParameter("@city", SqlDbType.VarChar, 30);
                city.Value = eventShowings.city;

                var latitude = new SqlParameter("@latitude", SqlDbType.Decimal);
                latitude.Value = eventShowings.latitude;

                var longitude = new SqlParameter("@longitude", SqlDbType.Decimal);
                longitude.Value = eventShowings.longitude;

                var timezone = new SqlParameter("@timezone", SqlDbType.VarChar, 50);
                timezone.Value = eventShowings.tz;

                var Menu = new SqlParameter("@menu", SqlDbType.Int);
                Menu.Value = 1;

                var output = new SqlParameter("@output", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;

                var _context = new DBContext(new Database.DBCOntext(), "AddEventShowings", Eventid, startdate,enddate,latitude,longitude,city,country,timezone,Menu,output);
                _context.outputparampresent = true;
                await _context.InsertAsync();
                var evshowingsid = _context.outputparam;
                Task[] addticketclass = new Task[eventShowings.ticketClasses.Length];
                for (int i = 0; i < eventShowings.ticketClasses.Length; i++)
                {
                    addticketclass[i] = AddTicketClass(eventid, evshowingsid, eventShowings.ticketClasses[i]);
                }
            }
            catch (Exception ex)
            {

                throw;
            }*/
            
        }
        //get recurring event dates
        public async Task<IEnumerable<Eventdates>> GetEventDates(int eventid)
        {
            var EventId = new SqlParameter("@eventid", SqlDbType.Int);
            EventId.Value = eventid;
            var _context = new DBContext(new Database.DBCOntext(), "GetEventDates", EventId);
            var eventdates = await _context.GetAsync<Eventdates>();
            return eventdates;
        }

        public async Task<EventShowings> GetEventshowing(int eventid,int eventshowingid)
        {
            var EventId = new SqlParameter("@eventid", SqlDbType.Int);
            EventId.Value = eventid;
            var EventShowingId = new SqlParameter("@eventshowingid", SqlDbType.DateTime);
            EventShowingId.Value = eventid;
            var _context = new DBContext(new Database.DBCOntext(), " GetSingleEventShowingDate", EventId,EventShowingId);
            var eventdates = await _context.GetOneObjectAsync<EventShowings>();
            return eventdates;
        }

        static string localtime(DateTime time,string tz)
       {
            var date = Time.GetLocalTime(time, tz);
            var timeofday = date.TimeOfDay.ToString();
            return timeofday;
       }

        static string Datetimestr(DateTime time, string tz)
        {
            var date = Time.GetLocalTime(time, tz);
            var localdate = date.Date.ToString();
            return localdate
                ;
        }
        public class Eventdates
        {
            public int eventshowings_id { get; set; }
            public int event_id { get; set; }
            public DateTime start_date { get; set; }
        }
    }
}
