using Dapper;
using EventsRepublic.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class DashboardRepository : BaseRepository<T>
    {
     

     public async Task<GrossNet> GrossNetPerEvent(int userid,int eventid)
     {
            return await WithConnection<dynamic>(async c =>
            {
                var grossnet = await c.QueryAsync<GrossNet>(@"select Top 1 NetAmount,GrossAmount, from AggregateGrossNetSalesPerEvent where UserId = @userid", new { @userid = userid });
                return grossnet.FirstOrDefault();
            });                 
     }

        public async Task<GrossNet> GrossNet(int userid)
        {
            return await WithConnection<dynamic>(async c =>
            {
                var grossnet = await c.QueryAsync<GrossNet>(@"select Top 1 NetAmount,GrossAmount from UserTransactionAggregateLog where UserId = @userid", new { @userid = userid });
                return grossnet.FirstOrDefault();
            });
        }

        public async Task<UserBalance> Balance(int userid)
        {
            return await WithConnection<dynamic>(async c =>
            {
                var balance  = await c.QueryAsync<UserBalance>(@"select Balance from OrganiserBalance where UserId = @userid", new { @userid = userid });
                var Balance = balance.FirstOrDefault();
                return Balance;
            });
        }

        public async Task<IEnumerable<dynamic>> GetUserEvents(int userid)
        {
            try
            {
                using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                {
                    var events = await con.QueryAsync<dynamic>(@"SELECT Name,Startdate,Enddate from Event where User_Id = @userid", new { userid });
                    return events;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<IEnumerable<AggregateOrderTicketSales>> AggregateOrderticketsalesPerEvent(int userid, DateTime startdate, DateTime enddate, int eventid)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid", userid);
                sqlparams.Add("@startdate", startdate);
                sqlparams.Add("@enddate", enddate);
                sqlparams.Add("@eventid", eventid);
                var orderBought = await c.QueryAsync<AggregateOrderTicketSales>("AgggregateOrderticketvaluePerEvent", sqlparams, commandType: CommandType.StoredProcedure);

                return orderBought == null ? Enumerable.Empty<AggregateOrderTicketSales>():orderBought;
            });
            }

        public async Task<IEnumerable<AggregateOrderTicketSales>>AggregateOrderticketsales(int userid, DateTime startdate, DateTime enddate)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid", userid);
                sqlparams.Add("@startdate", startdate);
                sqlparams.Add("@enddate", enddate);
                var orderBought = await c.QueryAsync<AggregateOrderTicketSales>("AgggregateOrderticketvalue", sqlparams, commandType: CommandType.StoredProcedure);
                return orderBought == null ? Enumerable.Empty<AggregateOrderTicketSales>() : orderBought;
            });
        }
        public async Task<IEnumerable<Tx>> LatestTransactions(int eventid, int quantity)
        {
            try
            {
                using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                {
                    var sqlparams = new DynamicParameters();
                    sqlparams.Add("@eventid",eventid);                  
                    sqlparams.Add("@quantity",quantity);
                    return await con.QueryAsync<Tx>("GetEventLatestTx", sqlparams, commandType: CommandType.StoredProcedure);
                }              
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<OrderTicketBoughtGrouped>> OrderTicketSalegrouped(int userid, DateTime startdate, DateTime enddate)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid", userid);
                sqlparams.Add("@startdate", startdate);
                sqlparams.Add("@enddate", enddate);
                var totalticketsbought = await c.QueryAsync<OrderTicketBoughtGrouped>("OrdersTicketsBoughtGrouped", sqlparams, commandType: CommandType.StoredProcedure);
                return totalticketsbought;
            });
        }

        public async Task<IEnumerable<OrderTicketBoughtGrouped>>OrdeTicketSalegroupedPerEvent(int userid, DateTime startdate, DateTime enddate, int eventid)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid", userid);
                sqlparams.Add("@eventid", eventid);
                sqlparams.Add("@startdate", startdate);
                sqlparams.Add("@enddate", enddate);
                var totalticketsbought = await c.QueryAsync<OrderTicketBoughtGrouped>("OrdersTicketsBoughtPerEventGrouped", sqlparams, commandType: CommandType.StoredProcedure);
                return totalticketsbought;
            });
                }
            }

    public class UserBalance
    {
        public decimal Balance { get; set; }
    }

    public class GrossNetBalance
    {
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        
    }
}

    public class T
    {
    }

    public class GrossNet
    {
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
    }
public class AggregateOrderTicketSales
{
    public int Orders { get; set; }
    public int Ticketssold { get; set; }
    public decimal Avgticketsperorder { get; set; }
    public decimal Avgorderamount { get; set; }
}
    public class OrderTicketBoughtGrouped
    {
        public DateTime PaidDate { get; set; }
        public int OrderSold { get; set; }              
    }
    public class TicketsSoldPerTicketclass
    {
        public string Name { get; set; }
        public int TicketsToSell { get; set; }
        public int TicketsSold { get; set; }
    }
    public class Tx
    {
        public DateTime Date { get; set; }
        public string Txcode { get; set; }
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }
    }


