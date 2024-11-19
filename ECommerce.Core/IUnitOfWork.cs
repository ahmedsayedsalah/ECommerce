using ECommerce.Core.Entities;
using ECommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T: BaseEntity;
        Task<int> CompleteAsync();
    }
}
