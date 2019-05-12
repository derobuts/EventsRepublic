using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class Reports : BaseRepository<Reports>
    {
        public async Task<IEnumerable<OrderReports>> GetOrderReports(int userid,DateTime startdate, DateTime enddate)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid",userid, DbType.String);
                sqlparams.Add("@startdate",startdate, DbType.DateTime);
                sqlparams.Add("@enddate",enddate, DbType.DateTime);
               
                var orderReports = await c.QueryAsync<OrderReports>("TotalOrdersBoughtReports"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return orderReports;
            });
        }

        public async Task<IEnumerable<OrderReports>> GetOrderReportsByEvent(int userid,DateTime startdate, DateTime enddate, int eventid)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid", userid, DbType.String);
                sqlparams.Add("@startdate", startdate, DbType.DateTime);
                sqlparams.Add("@enddate", enddate, DbType.DateTime);

                var orderReports = await c.QueryAsync<OrderReports>("TotalOrdersBoughtReportsPerEvent"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return orderReports;
            });
        }

        public async Task<IEnumerable<TransactionReport>> GetTransactionHistory(int userid, DateTime startdate, DateTime enddate)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid", userid, DbType.String);
                sqlparams.Add("@startdate", startdate, DbType.DateTime);
                sqlparams.Add("@enddate", enddate, DbType.DateTime);

                var txReports = await c.QueryAsync<TransactionReport>("TransactionsHistory"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return txReports;
            });
        }



    }

    public class TransactionReport
    {
        public string Txcode { get; set; }
        public int Orderid { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime Txdate { get; set; }


    }

    public class OrderReports
    {
        public int Orderid { get; set; }
        public decimal Amount { get; set; }
        public decimal NetAmount { get; set; }
        public string Delivery_Status { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
      
    }
}
