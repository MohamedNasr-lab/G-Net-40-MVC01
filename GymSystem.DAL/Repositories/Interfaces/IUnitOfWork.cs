using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystemG03.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        public Task<int> CompleteAsync();




    }
}