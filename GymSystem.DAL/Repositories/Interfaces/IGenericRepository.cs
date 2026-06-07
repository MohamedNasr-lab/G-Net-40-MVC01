using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemG03.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default);

        Task<TEntity?> GetById(int id, CancellationToken ct = default);

        void Add(TEntity item);

        void Update(TEntity item);

        void Delete(int id);

        Task<int> CompleteAsync();
        Task GetById();

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,bool isTracked=false,CancellationToken ct=default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken ct = default);
    }
}
