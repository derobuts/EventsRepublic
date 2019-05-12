using EventsRepublic.Database;
using EventsRepublic.InterFace;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using EventsRepublic.Database;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using EventsRepublic.Models;
using EventsRepublic.Models.Interface;

namespace EventsRepublic.Repository
{  //OneClass To Rule THem all
    public class DBContext
    { 
        private DBCOntext dbcontext;
        public int outputparam;
        public bool outputparampresent = false;
        private String storedproc;
        private SqlParameter[] parameters;

        public DBContext(DBCOntext dBContext, string StoredProc, params SqlParameter[] param)
        {
            this.dbcontext = dBContext;
            this.storedproc = StoredProc;
            this.parameters = param;
        }

        public async void DeleteAsync()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbcontext.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter outparameter = new SqlParameter();
                        //open con
                        await conn.OpenAsync();
                        int x = await cmd.ExecuteNonQueryAsync();
                        if (outputparampresent)
                        {
                            outputparam = int.Parse(cmd.Parameters["@output"].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<T> GetOneObjectAsync<T>()
        {
            var k = typeof(T);

            var props = k.GetProperties();
            try
            {
                var myObj = Activator.CreateInstance<T>(); //create instance of the given type
                using (SqlConnection conn = new SqlConnection(dbcontext.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter outparameter = new SqlParameter();
                        cmd.Parameters.AddRange(parameters);
                        //cmd.Parameters.Add("@eventid", System.Data.SqlDbType.Int);
                        //open con
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            int size = reader.FieldCount;

                            while (await reader.ReadAsync())
                            {
                                for (int i = 0; i < size; i++)
                                {
                                    if (String.Equals(props[i].Name, reader.GetName(i), StringComparison.OrdinalIgnoreCase))
                                    {
                                        props[i].SetValue(myObj, Convert.ChangeType(reader[props[i].Name], reader.GetFieldType(i), null));
                                    }
                                }

                            }
                        }

                        return myObj;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<T> GetWithID<T>()
        {
            //int number = -1;
            var k = typeof(T);
           
            var props = k.GetProperties();
            try
            {
                var myObj = Activator.CreateInstance<T>(); //create instance of the given type
                using (SqlConnection conn = new SqlConnection(dbcontext.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter outparameter = new SqlParameter();
                        cmd.Parameters.AddRange(parameters);
                        //cmd.Parameters.Add("@eventid", System.Data.SqlDbType.Int);
                        //open con
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            int size = reader.FieldCount;

                            while (await reader.ReadAsync())
                            {
                       
                                for (int i = 0; i < size; i++)
                                {
                                    if (String.Equals(props[i].Name, reader.GetName(i), StringComparison.OrdinalIgnoreCase))
                                    {
                                        props[i].SetValue(myObj, Convert.ChangeType(reader[props[i].Name], reader.GetFieldType(i), null));
                                    }
                                }
                                
                            }
                        }

                        return myObj; 
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public  async Task<IEnumerable<T>> GetAsync<T>()
        {
            //int number = -1;
            List<T> myObjects  = new List<T>();
            try
            {
                var k = typeof(T);
                var props = k.GetProperties();
               
                using (SqlConnection conn = new SqlConnection(dbcontext.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter outparameter = new SqlParameter();
                        cmd.Parameters.AddRange(parameters);
                        //open con
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {

                            int size = reader.FieldCount;

                            while (await reader.ReadAsync())
                            {
                                var myObj = Activator.CreateInstance<T>(); //create instance of the given type
                                
                                for (int i = 0; i < size; i++)
                                {
                                    if (String.Equals(props[i].Name,reader.GetName(i), StringComparison.OrdinalIgnoreCase))
                                    {
                                        props[i].SetValue(myObj, Convert.ChangeType(reader[props[i].Name],reader.GetFieldType(i),null));
                                    }
                                }
                                myObjects.Add(myObj);
                            }
                        }
                    }
                }
                return myObjects;
            }
            catch (SqlException ex)
            {
                //Log any error to database

                throw;
            }
        }
    
        public async Task InsertAsync()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbcontext.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //SqlParameter outparameter = new SqlParameter();
                        
                        cmd.Parameters.AddRange(parameters);
                        //open con
                        await conn.OpenAsync();
                        int x = await cmd.ExecuteNonQueryAsync();
                      
                        //outputparam = (Int32)outparameter.Value;
                        if (outputparampresent)
                        {
                            outputparam = int.Parse(cmd.Parameters["@output"].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> UpdateAsync()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbcontext.GetdbConnectionString()))

                {
                    using (SqlCommand cmd = new SqlCommand(storedproc, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);
                        //open con
                        await conn.OpenAsync();
                        int x = await cmd.ExecuteNonQueryAsync();
                        if (outputparampresent)
                        {
                            outputparam = int.Parse(cmd.Parameters["@output"].Value.ToString());
                        }

                        return x;
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                var b = 10;
                throw;
            }
        }
    }

    }

