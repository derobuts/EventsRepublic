using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class Transactions : BaseRepository<Transactions>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }

       public async Task<IEnumerable<Transactions>> GetTransactions(int userid, DateTime startdate, DateTime enddate, int PageNo, int pageSize)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@userid", userid, DbType.Int32);
                sqlparams.Add("@startdate", startdate, DbType.DateTime2);
                sqlparams.Add("@enddate", enddate, DbType.DateTime2);
                sqlparams.Add("@pageNo", PageNo, DbType.Int32);
                sqlparams.Add("@pageSize", pageSize, DbType.Int32);
                var resorder = await c.QueryAsync<Transactions>("TxHistory",
                sqlparams,
                commandType: CommandType.StoredProcedure);
                return resorder;
            });
        }
    }
}
