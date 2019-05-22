using EventsRepublic.Models;
using EventsRepublic.Repository;
using EventsRepublic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
   public interface IOrderRepository
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();
        Task<int> CreatOrder(int eventid, int userid, List<TicketsToReserve> ticketsToReserve, bool recurring, int noofticketsinorder, DateTime orderstartdate, DateTime orderenddate);
        Task<OrderSummary> GetUserOrder(int orderid,int userid);
        Task<IEnumerable<OrderItem>> GetItemsinOrder(int orderid);
    }
}
