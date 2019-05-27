using Dapper;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using EventsRepublic.Models;
using EventsRepublic.Models.Interface;
using EventsRepublic.Models.Mpesa;
using EventsRepublic.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QRCoder;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class OrderRespository  : BaseRepository,IOrderRepository
    {       
        /** **/
        public async Task<IEnumerable<OrderSummary>> GetOrderReserved(int orderid)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@orderid", orderid, DbType.Int32);
                //sqlparams.Add("@eventid", eventid, DbType.Int32);
                var resorder = await c.QueryAsync<OrderSummary>("GetOrderReserved",
                sqlparams,
                commandType: CommandType.StoredProcedure);
                return resorder;
            });
        }
      
        /** **/
        public async Task<int> ValidateOrderC2B(int ordermobilerefno, decimal amount)
        {
            return await WithConnection(async c =>
            {
                var sqlparams = new DynamicParameters();
                sqlparams.Add("@ordermobilerefno", ordermobilerefno, DbType.Int32);
                sqlparams.Add("@amount", amount, DbType.Decimal);
                var mobileref = await c.ExecuteScalarAsync<Int32>("ValidateOrder",
                sqlparams,
                commandType: CommandType.StoredProcedure);
                return mobileref;
            });
        }

        /** **/
        public async Task<OrderSummary> GetUserOrder(int orderid,int userid) => await WithConnection(async c =>
                                                                              {
                                                                                  return c.Query<OrderSummary>(@"select Top 1 Expiryts,Status,Amount,(case when NoOfTicketsReserved = NoOfTicketsRequested
                                                                                  then 1 else 0 end)as AllItemsReserved
                                                                                  from Orders where OrdersId = @orderid and UserId = @userid", new { @orderid = orderid, @userid = userid }).FirstOrDefault();
                                                                              }
        );      
        /** **/
        public async Task<int> CreatOrder(int eventid,int userid, List<TicketsToReserve> ticketsToReserve,bool recurring,int noofticketsinorder,DateTime orderstartdate, DateTime orderenddate)
        {
           // List<Task> ticketclasstasks = new List<Task>();
            DateTime Expirytime = DateTime.Now.AddMinutes(15).ToUniversalTime();
            //OrderSummary orderSummary = new OrderSummary();
             var orderid = await WithConnection(async c =>
             {
                 return c.Query<int>("AddOrderv32", new { eventid = eventid,userid = userid,noofticketstoreserve = noofticketsinorder, startorderdate = orderstartdate, endorderdate  = orderenddate,expiry = Expirytime}, commandType: CommandType.StoredProcedure).Single();
             });
            if (recurring)
            {
                foreach (var item in ticketsToReserve)
                {
                    await AddRecurringTicketToOrder(item,Expirytime,orderid, eventid,orderstartdate.ToUniversalTime());
                }
            }
            else
            {
                foreach (var item in ticketsToReserve)
                {
                    await AddTicketToOrder(item,Expirytime,orderid, eventid);                 
                }
            }
            return orderid;
        }
        public async Task<IEnumerable<OrderItem>> GetItemsinOrder(int orderid) => await WithConnection(async c =>
        {
            return await c.QueryAsync<OrderItem>(@"select Tc.Name,Tc.Price,quantity.Quantity from
                                      (
                                      SELECT Class_Id, COUNT(Id) AS Quantity FROM Ticket T
                                      WHERE Order_Id = @orderid
                                      group by Class_Id
                                      ) as quantity
                                      inner join TicketClass Tc on Tc.TicketId = quantity.Class_Id", new { @orderid = orderid });
        }
       );
        /** **/
        public async Task AddTicketToOrder(TicketsToReserve ticketsToReserve,DateTime expiryttime,int orderid,int eventid)
        {
            await WithConnection2(
               async c =>
               {
                   var sqlparams = new DynamicParameters();
                   sqlparams.Add("@ticketclassid", ticketsToReserve.ticketclassid, DbType.Int32);
                   sqlparams.Add("@eventid", eventid, DbType.Int32);
                   sqlparams.Add("@ticketquantity", ticketsToReserve.ticketsselected, DbType.Int32);
                   sqlparams.Add("@orderid",orderid, DbType.Int32);
                   sqlparams.Add("@expirytime", expiryttime, DbType.DateTime);
                   await c.ExecuteAsync("AddTicketToOrderv32", sqlparams, commandType: CommandType.StoredProcedure);
               }
              );
        }
        public async Task AddRecurringTicketToOrder(TicketsToReserve ticketsToReserve, DateTime expiryttime, int orderid, int eventid,DateTime recurrencekey)
        {
            await WithConnection2(
               async c =>
               {
                   var sqlparams = new DynamicParameters();
                   sqlparams.Add("@ticketclassid", ticketsToReserve.ticketclassid, DbType.Int32);
                   sqlparams.Add("@eventid", eventid, DbType.Int32);
                   sqlparams.Add("@ticketquantity", ticketsToReserve.ticketsselected, DbType.Int32);
                   sqlparams.Add("@orderid", orderid, DbType.Int32);
                   sqlparams.Add("@expirytime", expiryttime, DbType.DateTime);
                   sqlparams.Add("@recurrencekey", recurrencekey, DbType.DateTime);
                   await c.ExecuteAsync("AddRecurringTicketToOrderv32", sqlparams, commandType: CommandType.StoredProcedure);
               }
              );
        }
        public async void UpdateConfirmedOrder (int orderid, string txcode)
        {
            await WithConnection2(
                    async c =>
                    {
                        await c.ExecuteAsync(@"UPDATE OrderTransaction
                                          SET Status = @status,paidts = GETUTCDATE()
                                          where OrdersId = @orderid", new { @status = 101, @orderid = orderid });
                    }
                     );
        }

        public async Task<IEnumerable<LatestOrdersBought>>GetLatestOrders()
        {
           return await WithConnection(
                async c =>
                {
                    return await c.QueryAsync<LatestOrdersBought>("GetLatestOrders",commandType: CommandType.StoredProcedure);
                });
        }

        public async void ConfirmOrder(int orderid,int Paymentmethod,int userid,decimal amount,string txcode)
        {
            async Task confirmorder()
            {
                await WithConnection2(
                   async c =>
                   {
                       var sqlparams = new DynamicParameters();
                       sqlparams.Add("@orderid", orderid, DbType.Int32);
                       sqlparams.Add("@amount", amount, DbType.Decimal);
                       sqlparams.Add("@txcode", amount, DbType.String);
                       sqlparams.Add("@paymentmethod", Paymentmethod, DbType.Int16);
                       await c.ExecuteAsync("ConfirmOrder", new { @status = 101, @orderid = orderid });
                   }
                    );

            }
            async Task updateTicketsInOrder()
            {
                await WithConnection2(
                   async c =>
                   {                     
                       await c.ExecuteAsync(@"UPDATE Ticket SET Status = 102 WHERE Order_Id = @orderid", new { @status = 101, @orderid = orderid });
                   }
                    );

            }
            List<Task> tasks = new List<Task> { confirmorder()};
            Task result = Task.WhenAll(tasks);
            try
            {
               await result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async void ConfirmTicketsBought(int orderid)
        {
            await WithConnection2(
            async c => {
                await c.ExecuteAsync(@"UPDATE Ticket SET Status = 102 WHERE Order_Id = @orderid", new { @orderid = orderid });
            });
        }

        //public async void UpdateOrderBought
        public async Task<Decimal> GetOrderPrice(int orderid, int eventid)
        {
          var Amount =   await WithConnection(
               async c =>
               {
                 var amount = await c.QueryAsync<decimal>(@"select Top 1 Amount from OrderTransaction where OrdersId = @orderid and Event_Id = @eventid", new { @orderid = orderid, @eventid = eventid });
                   return amount.FirstOrDefault();
               }
              );
            return Amount;
        }

     

        public async void SendOrderEmail(int orderid)
        {
            try
            {
                
               IEnumerable <BoughtTickets> Orderinfo;
                using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                {

                    Orderinfo = con.Query <BoughtTickets> (@"SELECT Tc.Name as TicketClassName,T.Barcode,E.Name as EventName,E.Startdate,V.PlaceAddress FROM OrderTransaction OT
                                                     inner join Ticket T on T.Order_Id = OT.OrdersId
                                                     inner join Event E on E.Event_Id = OT.Event_Id
                                                     inner join Venue V on V.id = E.venueid
                                                     inner join TicketClass Tc on Tc.Event_Id = E.Event_Id  WHERE OT.OrdersId = @OrdersId", new { @OrdersId = orderid });             
                }
              
                foreach (var boughtticket in Orderinfo)
                {
                    boughtticket.Barcodeimage = QRCodegenerator();
                }
             

                var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                var client = new SendGridClient("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");
                var msg = new SendGridMessage();
                msg.SetFrom(new EmailAddress("test@example.com", "Example User"));
                msg.AddTo(new EmailAddress("derobuts@outlook.com", "Example User"));
                msg.SetTemplateId("d-1a64faeb0b174e8bbe1c88d2a7e93ff9");
                var dynamicTemplateData = new 
                {
                    TicketsBought = new
                    {
                        Orderinfo
                    }
                    
                };
                var h = JsonConvert.SerializeObject(dynamicTemplateData);
                msg.SetTemplateData(dynamicTemplateData);
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    //POST TO AZURE FUNCTION
                }

            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        private string QRCodegenerator()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qrGenerator.CreateQrCode("derob", QRCodeGenerator.ECCLevel.M);
            //create byte/raw bitmap qr code
            BitmapByteQRCode qrCodeBmp = new BitmapByteQRCode(qRCodeData);
            byte[] qrCodeImageBmp = qrCodeBmp.GetGraphic(4);
            var h = Convert.ToBase64String(qrCodeImageBmp);
            return h;
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ListAllAsync()
        {
            throw new NotImplementedException();
        }


        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        private class BoughtTickets
        {
            public string TicketClassName { get; set; }
            public Guid Barcode { get; set; }
            public string Barcodeimage { get; set; }
            public string Attendee { get; set; }
            public string EventName { get; set; }
            public string Startdate { get; set; }
            public string PlaceAddress { get; set; }
        }

        public class LatestOrdersBought
        {
            public string TransactionId { get; set; }
            public Guid Id { get; set; }
            public decimal Amount { get; set; }
            public int  NoofTickets { get; set; }
            public string Status { get; set; }
        }
    }

        public class Ordercreated
        {
           public int OrdersId { get; set; }
           public Guid Unique_Id { get; set; }
           public DateTime Expiry { get; set; }
    }
        public class TicketClassesReserved
        {
            public string TicketClassName { get; set; }
            public decimal TicketClassPrice { get; set; }
            public int QuantityReserved { get; set; }
        }

    public class TicketsToReserve
    {
        public int ticketclassid { get; set; }
        public string ticketclassname { get; set; }
        public int ticketsselected { get; set; }
        public int ticketprice { get; set; }
    }

    public class OrderPayloads
    {
        public int ticketclassid { get; set; }
        public string ticketclassname { get; set; }
        public int ticketsselected { get; set; }
        public int ticketprice { get; set; }
    }

    public class OrdertoBeReserved
    {
        public List<TicketsToReserve> TicketsToReserve { get; set; }
        public int Eventid { get; set; }
        public bool Recurring { get; set; }
        public DateTime OrderStartDate { get; set; }
        public DateTime OrderEndDate { get; set; }
        public int NoofTicketsInOrder { get; set; }
    }
}

