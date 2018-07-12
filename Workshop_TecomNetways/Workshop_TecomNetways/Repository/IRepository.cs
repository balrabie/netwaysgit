using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Workshop_TecomNetways.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetItem(Expression<Func<T, bool>> predicate);
        Task<T> GetItemAsycn(Expression<Func<T, bool>> predicate);
        void Insert(T item);
        void Delete(T item);
        void ModifyEntityState(T item);
    }
}
