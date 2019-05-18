using Dapper;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EventsRepublic.Models.JsonWebToken;

namespace EventsRepublic.Models
{
    public class JsonWebToken : BaseRepository<UsersRoles>
    {
        //will use sha256
        //will add other algorithim later/google/facebook/
        //Mostly will Use RS256 and HS256
        public enum JAlgorithm
        {
            RS256,HS256, RS384
        }
        public struct Header
        {
            public string alg { get; set; }
         public string typ { get; set; }
        }

      

        // base64URL header + payload 
        public static string GetToken(object payload)
        {
            Header head = new Header();
            string Header = Convert.ToBase64String(Encoding.UTF8.GetBytes((JsonConvert.SerializeObject(head))));
            string Payload = Convert.ToBase64String((Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload))));
            string Headerpayload = Header + "." + Payload;
            // string ServerSecret = Base64UrlEncode(serversecret);
            var JToken = Hmac(SecretKey(), Encoding.UTF8.GetBytes(Headerpayload));
            return Headerpayload + "." + JToken;
        }

        public static Tuple<bool,string> ValidateToken(string token,bool verify)
        {
            
                var s = token;
                var header = s.Split('.')[0];// get header
                var body = s.Split('.')[1]; //get body
                var signature = s.Split('.')[2]; //get token

                string DecodedToken = Hmac(SecretKey(), Encoding.UTF8.GetBytes(header + "." + body));
                if (signature == DecodedToken)//check if token still valid
                {
                    var Payload = JsonConvert.DeserializeObject<Payload>(Encoding.UTF8.GetString(Base64URLDecode(body)));
                    if (Payload.Expiry < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    {
                        return new Tuple<bool, string>(false, "expired");

                    }
                    return new Tuple<bool, string>(true,Encoding.UTF8.GetString(Base64URLDecode(body)));
                }

                return new Tuple<bool, string>(false, "invalid token");
            }

        public async Task<Guid> GetRefreshTokenAsync(int userid)
        {
            Guid newRefeshToken = Guid.NewGuid();
           int Affectedrows = await WithConnection(async c =>
            {         
                var affectedrows = await c.ExecuteAsync(@"insert into RefreshToken(RefreshToken)
                                                    values(@refreshtoken)", new {@refreshtoken = newRefeshToken});
                return affectedrows;
            });
            return newRefeshToken;
        }
                      
        //refresh jwt 

       public async Task<Tuple<bool,string>>TokenRenew(Guid RefreshToken,string Token)
       {
            //check if token has expired and use refresh token to renew
           var isTokenvalid = ValidateToken(Token, true);
           // bool refreshtoken = false;
            Guid dbRefreshTokenGuid = Guid.NewGuid();
           
            if (isTokenvalid.Item2 == "expired")
            {
                dbRefreshTokenGuid = await WithConnection<Guid>(c =>
                {
                    var dbrefreshToken = c.Query<Guid>(@"SELECT RefreshToken  FROM RefreshToken where RefreshToken = @refreshtoken", new { @refreshtoken = RefreshToken }).FirstOrDefault();
                    return Task.FromResult(dbrefreshToken);
                }
           );
            }
            if (RefreshToken != dbRefreshTokenGuid)
            {
                return new Tuple<bool, string>(false, "invalid refreshtoken");
            }
            var newJwtToken = GetToken(Token.Split('.')[1]);
            var newRefreshToken =
            //delete and save new refresh guid
            await WithConnection<Guid>(c =>
            {
                var dbrefreshToken = c.Query<Guid>(@"update RefreshToken set RefreshToken = newid()
                                                     OUTPUT inserted.RefreshToken
                                                     where RefreshToken = @refreshtoken", new { @refreshtoken = RefreshToken }).FirstOrDefault();
                return Task.FromResult(dbrefreshToken);
            });
            return new Tuple<bool, string>(true, newJwtToken + '&' + newRefreshToken);
        }


        public async Task<IEnumerable<string>> GetRoles(Payload userpayload)
        {
            return await WithConnection(async c =>
              {
                  var userroles = await c.QueryAsync<string>(@"select R.Name from Roles R
                                                                 inner join UserRoles UR
                                                                 on R.RolesId = UR.RoleId
                                                                 WHERE UR.Userid = @userid ", new { @userid = userpayload.UserId });
                  return userroles;
              }
              );
        }


        // Base64URL we must remove certain special chars
        //
        private static string Base64UrlEncode(byte[] input)
        {
            string base64Url = Convert.ToBase64String(input);
            base64Url = base64Url.Split('=')[0];
            base64Url = base64Url.Replace('+', '-');
            base64Url = base64Url.Replace('/', '_');
            return base64Url;
        }
        public static byte[] SecretKey()
        {
          
            return Encoding.UTF8.GetBytes("dbutoyezIqP6uFTrUGGKzFsNMGlECQhuiYnpLZ3TRN7dcsYJjZKFJLfufESjGgXpJ1W4APUtLFzQdU6yBjhicPHaHt9k5A==");
        }
     
        public static string Hmac(byte[] Key, byte[] payload)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Key))
            {
                byte[] hashmessage = hmac.ComputeHash(payload);
                return Base64UrlEncode(hashmessage);
            }
        }
        public static byte[] Base64URLDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("You shall not pass base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }

}

