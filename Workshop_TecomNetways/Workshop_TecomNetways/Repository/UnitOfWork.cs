using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Workshop_TecomNetways.Context;

namespace Workshop_TecomNetways.Repository
{
    public class UnitOfWork : IDisposable
    {
        private BRDContext Entities = new BRDContext();

        public Dictionary<Type, object> Repositories = new Dictionary<Type, object>();

        /// <summary>
        /// Creates a generic repository of type T if not exists.
        /// Returns this repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>() 
            where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)) == true)
            {
                return Repositories[typeof(T)] as IRepository<T>;
            }
            IRepository<T> repository = new Repository<T>(Entities);
            Repositories.Add(typeof(T), repository);
            return repository;
        }

        public void Save()
        {
            Entities.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Entities.SaveChangesAsync(); //***
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}