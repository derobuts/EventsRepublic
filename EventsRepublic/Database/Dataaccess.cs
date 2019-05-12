using EventsRepublic.InterFace;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Database
{   //CRUD
    public class Dataaccess<T>
    {
        private DBCOntext db;
        public int outputparam;
        private String storedproc;
        private SqlParameter[] parameters;
        public Dataaccess(DBCOntext dB,string StoredProc, params SqlParameter[]param)
        {
            this.db = dB;
            this.storedproc = StoredProc;
            this.parameters = param;
        }
        public async Task<T> GetAsync(T t)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(db.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter();
                        cmd.Parameters.AddRange(parameters);
                        //open con
                        await conn.OpenAsync();
                        var read = (T)cmd.ExecuteScalar();
                        return read;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<bool> DeleteId(int t)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetIdAsync(int t)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(db.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter();
                        cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int);
                        cmd.Parameters["@ID"].Value = t;
                        //open con
                        await conn.OpenAsync();
                        var read = (T)cmd.ExecuteScalar();
                        return read;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> InsertDeleteUPdate()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(db.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter outparameter = new SqlParameter();
                        outparameter.ParameterName = "@output";
                        outparameter.SqlDbType = System.Data.SqlDbType.Int;
                        outparameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.AddRange(parameters);
                        cmd.Parameters.Add(outparameter);
                        //open con
                        await conn.OpenAsync();
                        int x = await cmd.ExecuteNonQueryAsync();
                        outputparam=(Int32)outparameter.Value;
                        if (x==0)
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Task<bool> Insert(T t)
        {
            throw new NotImplementedException();
        }
    }
}
