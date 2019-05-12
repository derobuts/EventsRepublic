using Dapper;
using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{   
    public interface IRepository <T> where T : class
    {
        T get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> RunSqlCommand(Func<T, bool> predicate);
        void Add(T t);
        void AddRange(IEnumerable<T> entities);
        void Delete(T t);
        void DeleteId(int id);
        void GetId(int id);
        void Update(T t);
    
    }
}
