using Dapper;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using EventsRepublic.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EventsRepublic.Repository
{
    public class UserRespirotory : BaseRepository
    {
        public async Task<int> AddGuestCheckoutUser(UserInfo User)
        {
          
            return await WithConnection(async c =>
              {
                  var sqlparams = new DynamicParameters();
                  sqlparams.Add("@email", User.Email, DbType.String);
                  sqlparams.Add("@phoneno", User.Phoneno, DbType.String);
                  sqlparams.Add("@role",2, DbType.Int32);
                  sqlparams.Add("@reguserid", DbType.Int32, direction: ParameterDirection.Output);

                  await c.ExecuteAsync("AddGuestUser", sqlparams, commandType: CommandType.StoredProcedure);
                  int userid = sqlparams.Get<int>("@reguserid");
                  return userid;
              });
            /**
            //add guestuser to order
            await WithConnection2(async c =>
             {
                 var sqlparams = new DynamicParameters();
                    // sqlparams.Add("@user_id",UserId, DbType.String);
                    await c.ExecuteAsync($"update OrderTransaction set Userid = {UserId}  where OrdersId = {User.Orderid}", commandType: CommandType.Text);
             });**/
            
        }

        public async Task<int> UpdateGuestCheckoutUserInfo(UserInfo userinfo)
        {

            return await WithConnection(async c =>
            {
                return await c.ExecuteAsync("update UsersTable set Email = @Email,PhoneNumber = @PhoneNumber  where Userid = @Userid",new {@Email=userinfo.Email,@PhoneNumber=userinfo.Phoneno,@Userid=userinfo.UserId });
            });
            /**
            //add guestuser to order
            await WithConnection2(async c =>
             {
                 var sqlparams = new DynamicParameters();
                    // sqlparams.Add("@user_id",UserId, DbType.String);
                    await c.ExecuteAsync($"update OrderTransaction set Userid = {UserId}  where OrdersId = {User.Orderid}", commandType: CommandType.Text);
             });**/

        }


        public async Task<Tuple<bool, string>> AddUser(UserInfo User)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (await checkifemailexists(User.Email))
                    {
                        return new Tuple<bool, string>(false, "Email exists");
                    }

                    var reguserid = await WithConnection(async c =>
                    {
                        var sqlparams = new DynamicParameters();
                        sqlparams.Add("@name", User.UserName, DbType.String);
                        sqlparams.Add("@email", User.Email, DbType.String);
                        //  sqlparams.Add("@role",2, DbType.Int32);
                        sqlparams.Add("@hashpwd", HashPassword(User.Pwd), DbType.String);
                        sqlparams.Add("@userid", 0, DbType.Int32, ParameterDirection.Output);
                        await c.ExecuteAsync("AddUser", sqlparams, commandType: CommandType.StoredProcedure);
                        int Userid = sqlparams.Get<int>("@userid");
                        return Userid;
                    });

                    await WithConnection2(async c =>
                    {
                        var sqlparams = new DynamicParameters();
                        sqlparams.Add("@userid", reguserid, DbType.Int32);
                        sqlparams.Add("@roleid",1, DbType.Int32);
                        await c.ExecuteAsync("AddUserRole", sqlparams, commandType: CommandType.StoredProcedure);
                    });
                    //createjwt
                    Payload payload = new Payload();
                    payload.Email = User.Email;
                    payload.Role = 2;
                    payload.Expiry = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
                    payload.UserId = reguserid;

                    Header header = new Header();
                    header.alg = "hmac";
                    header.typ = "none";
                    JsonWebToken jwt = new JsonWebToken();
                    var Refreshtoken = await jwt.GetRefreshTokenAsync(reguserid);
                    string jwttoken = JsonWebToken.GetToken(payload);
                    scope.Complete();
                    return new Tuple<bool, string>(true, JsonConvert.SerializeObject(new { RefreshToken = Refreshtoken, token = jwttoken }));
                   
                }
                catch (Exception ex)
                {
                    var h = ex;
                    throw;
                }
                
            }
        }
        //
        public async Task<Tuple<bool,string>>Login(UserInfo user)
        {
            try
            {
                string jwttoken = null;
              var Dbuser = await WithConnection(c =>
                {
                    var dbuser = c.Query<UserInfo>(@"SELECT top(1)Email,PasswdHash,UserId FROM Users where Email = @email", new { @email = user.Email }).FirstOrDefault();
                    return Task.FromResult(dbuser);
                });
                if (Dbuser != null)
                {
                 var IsPasswordvalid =  VerifyHashedPassword(Dbuser.Passwdhash,user.Pwd);
                    if (IsPasswordvalid)
                    {
                        //createjwt
                        Payload payload = new Payload();
                        payload.Email = Dbuser.Email;
                        payload.Role = 1;
                        payload.Expiry = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
                        payload.UserId = Dbuser.UserId;

                        Header header = new Header();
                        header.alg = "hmac";
                        header.typ = "none";
                        JsonWebToken jwt = new JsonWebToken();
                      //  var Refreshtoken = await jwt.GetRefreshTokenAsync(reguserid);
                        string token = JsonWebToken.GetToken(payload);
                        //jwttoken = JsonWebToken.GetToken(payload, header);
                        return new Tuple<bool, string>(true,token);
                    }
                    return new Tuple<bool, string>(false, "wrong pasword");
                }
                return new Tuple<bool, string>(false, "user not found")
;            }
            catch (Exception ex)
            {
                 return new Tuple<bool, string>(false, "error");
            }
        }
        public async void ConfirmTicketsBought(int orderid)
        {
            await WithConnection2(
            async c => {
                await c.ExecuteAsync(@"UPDATE Ticket SET Status = 102 WHERE Order_Id = @orderid", new { @orderid = orderid });
            });
        }

        public string GetUserBalance(int userid)
        {
            decimal Balance;
            using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
            {

                Balance = con.Query<Int32>(@"SELECT(SUM(CASE WHEN txoperation = 100 THEN t.Amount END),0)  - coalesce(SUM(CASE WHEN txoperation = 100 THEN t.Amount END),0) * 0.07  - coalesce(SUM(CASE WHEN txoperation = 101 THEN t.Amount END),0) as Balance
                                               from TxTable t where userid = @Userid", new { @Userid = userid }).FirstOrDefault();
            }
            return JsonConvert.SerializeObject(Balance);
        }

        public async Task<bool> checkifemailexists(string Email)
        {
          var emailint =  await WithConnection<int>(async c =>
                {
                    const string sql = "SELECT count(1) FROM Users where Email = @email";
                    int exists = await c.ExecuteScalarAsync<int>(sql, new { @email = Email });
                    return exists;
                });
            var emailexists = emailint == 0 ? false : true;
            return emailexists;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRole(int User)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUserID(int User)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserInfo> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ISEmailAddress(UserInfo User)
        {
            throw new NotImplementedException();
        }

     


        public void Save()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateUser(int User)
        {
            throw new NotImplementedException();
        }
        //
        public string HashPassword(string password)
        {
            var prf = KeyDerivationPrf.HMACSHA256;
            var rng = RandomNumberGenerator.Create();
            const int iterCount = 10000;
            const int saltSize = 128 / 8;
            const int numBytesRequested = 256 / 8;

            // Produce a version 3 (see comment above) text hash.
            var salt = new byte[saltSize];
            rng.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, iterCount);
            WriteNetworkByteOrder(outputBytes, 9, saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return Convert.ToBase64String(outputBytes);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // Wrong version
            if (decodedHashedPassword[0] != 0x01)
                return false;

            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
            var iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);
            var saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            var salt = new byte[saltLength];
            Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

            // Read the subkey (the rest of the payload): must be >= 128 bits
            var subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;
            if (subkeyLength < 128 / 8)
            {
                return false;
            }
            var expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            var actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterCount, subkeyLength);
            return actualSubkey.SequenceEqual(expectedSubkey);
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }
    }
    public struct Payload
    {
        public string Email { get; set; }
        public int Role { get; set; }
        public Guid UseruniqueId { get; set; }
        public int UserId { get; set; }
        public long Iat { get; set; }
        public long Expiry { get; set; }
        //public Ordercreated UserOrder { get; set; }
      //  public byte[] rowversion { get; set; }
       // public int OrderId { get; set; }
        public bool Isregistered { get; set; }
    }
   
    public struct Header
    {
        public string alg { get; set; }
        public string typ { get; set; }
    }
}
