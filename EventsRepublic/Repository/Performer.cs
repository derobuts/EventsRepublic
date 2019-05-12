using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class PerformerRepository : BaseRepository<Performer>
    {
        //get event venue by name
        public async Task<IEnumerable<Performer>> GetPerformer(string searchword, int lastrecordno, int noofrowsreturn)
        {
            return await WithConnection(async c => {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@word", searchword, DbType.String);
                sqlparams.Add("@lastrecordno", lastrecordno, DbType.Int32);
                sqlparams.Add("@noofrowsreturn", noofrowsreturn, DbType.Int32);
                var venuesearch = await c.QueryAsync<Performer>("SearchPerformerName"
                    , sqlparams,
                    commandType: CommandType.StoredProcedure
                    );
                return venuesearch;

            });
        }
    }
    public class Performer
    {
        public int Performer_Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } = "performer";

    }
}
