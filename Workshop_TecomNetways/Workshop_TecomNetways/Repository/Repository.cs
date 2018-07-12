using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Workshop_TecomNetways.Context;

namespace Workshop_TecomNetways.Repository
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private BRDContext Entities = null;

        public Repository(BRDContext entities)
        {
            Entities = entities;
        }

        public void ModifyEntityState(T item)
        {
            Entities.Entry(item).State = EntityState.Modified;
        }

        public IQueryable<T> GetAll()
        {
            return Entities.Set<T>().AsQueryable();
        }


        public T GetItem(Expression<Func<T, bool>> predicate)
        {
            return Entities.Set<T>().FirstOrDefault(predicate);
        }

        public Task<T> GetItemAsycn(Expression<Func<T, bool>> predicate)
        {
            return Entities.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public void Insert(T item)
        {
            Entities.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            Entities.Set<T>().Remove(item);
        }
    }
}