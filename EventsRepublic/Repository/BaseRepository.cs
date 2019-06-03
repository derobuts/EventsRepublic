using Dapper;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public abstract class BaseRepository
    {
        private readonly string _ConnectionString = new DBCOntext().GetdbConnectionString();
        /* protected BaseRepository(string connectionstring)
         {
             _ConnectionString = connectionstring;
         }
         */
        protected async Task<C> WithConnection<C>(Func<IDbConnection, Task<C>> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                   // await connection.OpenAsync(); // Asynchronously open a connection to the database
                    return await getData(connection); // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        protected async Task WithConnection2(Func<IDbConnection, Task> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    //await connection.OpenAsync(); // Asynchronously open a connection to the database
                    await getData(connection); // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }
     
    }
}
