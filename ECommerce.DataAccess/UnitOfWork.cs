using ECommerce.Core;
using ECommerce.Core.Entities;
using ECommerce.Core.Repositories;
using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Reposioties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;
        private readonly Hashtable repositories;
        //private readonly Dictionary<string, GenericRepository<BaseEntity>> repositories;

        public UnitOfWork(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            repositories= new Hashtable();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type= typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repository= new GenericRepository<T>(dbContext);

                repositories.Add(type, repository); // Implicit Casting to <string, GenericRepository<BaseEntity>
            }

            return repositories[type] as IGenericRepository<T>;
        }
        public async Task<int> CompleteAsync()
            => await dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await dbContext.DisposeAsync();

    }
}
