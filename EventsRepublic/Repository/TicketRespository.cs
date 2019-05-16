using Dapper;
using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class TicketRespository : BaseRepository<TicketClass>
    {        
        public async Task<IEnumerable<TicketClassSubinfo>> GeteventTicketClass(Guid eventid)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@eventid",eventid, DbType.Guid);
                var TicketClass = await c.QueryAsync<TicketClassSubinfo>("GetEventTicketClasses"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return TicketClass;
            });
        }
        public async Task<IEnumerable<TicketClassSubinfo>> GetEventRecurringTicketClasses(Guid eventid,DateTime recurrencekey)
        {
            await CheckIfRecurrenceDateExists(recurrencekey, eventid);
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@eventid", eventid, DbType.Guid);
                sqlparams.Add("@recurrencekey",recurrencekey, DbType.DateTime);
                var TicketClass = await c.QueryAsync<TicketClassSubinfo>("GetEventRecurringTicketClasses"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return TicketClass;
            });
        }

        public async Task CheckIfRecurrenceDateExists(DateTime date, Guid eventid)
        {
            var ticketid = await WithConnection(async c =>
            {
                return c.Query<int>(@"select Top 1 Id from Ticket where RecurrenceKey = @eventdate", new { @eventdate = date }).FirstOrDefault();
            });
            if (ticketid <= 0)
            {
                await WithConnection2(async c =>
                {
                    var sqlparams = new DynamicParameters();
                    sqlparams.Add("@eventid", eventid, DbType.Guid);
                    sqlparams.Add("@@recurrencekey", date, DbType.DateTime);
                    var TicketClass = await c.QueryAsync<TicketClassSubinfo>("AddTicketsRecurrence"
                        , sqlparams,
                        commandType: CommandType.StoredProcedure
                        );
                });
            }
        }

        public async Task AddTicketClass(int eventid, TicketClass ticketClass)
        {
            await WithConnection2(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@event_id", eventid, DbType.Int32);
                sqlparams.Add("@name", ticketClass.name, DbType.String);
                sqlparams.Add("@max_qtperodr", ticketClass.max_per_order, DbType.Int16);
                sqlparams.Add("@price", ticketClass.amount, DbType.Decimal);
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
        public async Task AddRecurringTicketClass(int eventid, TicketClass ticketClass)
        {
            await WithConnection2(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@event_id", eventid, DbType.Int32);
                sqlparams.Add("@name", ticketClass.name, DbType.String);
                sqlparams.Add("@max_qtperodr", ticketClass.max_per_order, DbType.Int16);
                sqlparams.Add("@price", ticketClass.amount, DbType.Decimal);
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
                await c.ExecuteAsync("AddRecurringTicketClass", sqlparams, commandType: CommandType.StoredProcedure);
            }
        );
        }

        public async Task<bool> ReserveTickets(int eventid, List<TicketsToReserve> ticketsToReserve)
        {

            List<Task> ticketclasstasks = new List<Task>();
            var reservationKey = Guid.NewGuid();
            var unixtime = this.UnixTimeNow();          
            foreach (var item in ticketsToReserve)
            {
                ticketclasstasks.Add(ReserveTicket(item,reservationKey,unixtime,eventid));
            }
            Task result = Task.WhenAll(ticketclasstasks);
            try
            {
                await result;
                return true;
            }
            catch (Exception EX)
            {
                var H = 5;

                return false;
            }
        }
        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
        public async Task ReserveTicket(TicketsToReserve ticketsToReserve, Guid ReservationKey, long ExpiryTs, int eventid)
        {
            await WithConnection2(
               async c =>
               {
                   var sqlparams = new DynamicParameters();
                   sqlparams.Add("@ticketclassid", ticketsToReserve.ticketclassid, DbType.Int32);
                   sqlparams.Add("@eventid", eventid, DbType.Int32);
                   sqlparams.Add("@ticketquantity", ticketsToReserve.ticketsselected, DbType.Int32);
                   sqlparams.Add("@reservationkey", ReservationKey, DbType.Guid);
                   sqlparams.Add("@expirytime", ExpiryTs, DbType.Int64);
                   await c.ExecuteAsync("ReserveTicket", sqlparams, commandType: CommandType.StoredProcedure);
               }
              );
        }

        public async void ConfirmTicketsBought(int orderid)
        {
            await WithConnection2(
            async c => {
                await c.ExecuteAsync(@"UPDATE Ticket SET Status = 102 WHERE Order_Id = @orderid", new { @orderid = orderid });
          });
        }

    }
    public class TicketClassSubinfo
    {
        public int TicketClassId { get; set; }
        public bool TicketSaleClosed { get; set; }
        public bool TicketSaleStarted { get; set; }
        public int Event_Id { get; set; }
        public string Name{ get; set; }
        public decimal Price { get; set; }
        public int Max_Qt_Per_Order { get; set; }
        public bool FewTicketsWarning { get; set; }
        public int MinTicketPerOrder { get; set; }
        public bool IsSoldOut{ get; set; }
        public DateTime Requestdate { get; set; }
    }
}
