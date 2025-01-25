using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrsl.Data.GenerateShortUrls.DAL.Contracts
{
    public interface IUrlRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Delete(TEntity entity);

        IQueryable<TEntity> AsQueryable();
        Task<TEntity?> GetByIdAsync(Guid id);

        Task<int> SaveChangesAsync();
    }
}
