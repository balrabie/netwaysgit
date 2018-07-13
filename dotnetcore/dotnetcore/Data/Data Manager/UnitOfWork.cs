
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Data
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed;

        private Context entities;

        private Dictionary<Type, object> repositories;


        public UnitOfWork() // ****************************** how to use the connection string from the .json file
        {
            disposed = false;

            entities = new Context(
            new DbContextOptionsBuilder<Context>()
            .UseSqlServer(Startup.CONNECTION_STRING)
            .Options);

            repositories = new Dictionary<Type, object>();
        }

        public UnitOfWork(Context entities)
        {
            disposed = false;

            this.entities = entities;

            repositories = new Dictionary<Type, object>();
        }


        public IRepository<T> GetRepository<T>()
            where T : class
        {
            if (repositories.Keys.Contains(typeof(T)))
            {
                return repositories[typeof(T)] as IRepository<T>;
            }

            else
            {
                IRepository<T> repository = new Repository<T>(entities);
                repositories.Add(typeof(T), repository);
                return repositories[typeof(T)] as IRepository<T>;
            }

        }
        public void Save()
        {
            entities.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await entities.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
