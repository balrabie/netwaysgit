using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dotnetcore.Data
{
    public class Repository<T> : IRepository<T>
        where T : class // model class
    {
        private readonly Context context;

        public Repository(Context context)
        {
            this.context = context;
        }
            
        public void Add(T item)
        {
            context.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            context.Set<T>().Remove(item);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }


        public T Get(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public void ModifyEntryState(T item, EntityState state)
        {
            context.Entry(item).State = state;
        }
    }
}
