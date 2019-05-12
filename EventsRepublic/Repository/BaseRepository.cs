using Dapper;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public abstract class BaseRepository<T> : IRepository<T>  where T : class
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
                    await connection.OpenAsync(); // Asynchronously open a connection to the database
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

        protected async Task WithConnection2(Func<IDbConnection,Task>getData)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    await connection.OpenAsync(); // Asynchronously open a connection to the database
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
       

        public void Add(T t,params DynamicParameters[] dynamicParams)
        {
       
        }

        public void AddRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T t)
        {
            throw new NotImplementedException();
        }

        public void DeleteId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> RunSqlCommand(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public T get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void GetId(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(T t)
        {
            throw new NotImplementedException();
        }

        public void Add(T t)
        {
            throw new NotImplementedException();
        }
    }
}
