using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Data.Repository
{
    /// <summary>
    /// 仓储父类
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <typeparam name="TID">操作主键</typeparam>
    public class BaseRepository<T, TID> : IBaseRepositoryT<T>, IBaseRepositoryTID<T, TID>
        where T:class
    {
        protected readonly DbContext context;

        public BaseRepository(DbContext context)
        {
            this.context = context;
        }

 
 
        public async Task AddAsync(T t)
        {
            await context.Set<T>().AddAsync(t);
        }

        public Task DeleteAsync(T t)
        {
            context.Set<T>().Remove(t);

            return Task.CompletedTask;

        }

        public Task<IQueryable<T>> GetAllAsync()
        {
            return Task.FromResult(context.Set<T>().AsQueryable());
        }

        public async Task<T> GetByIdAsync(TID id)
        {
           return await context.Set<T>().FindAsync(id);
        }

        public async Task<bool> IsExistAsync(TID id)
        {
            return await context.Set<T>().FindAsync(id)!=null;
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync()>0;
        }

        public Task UpdateAsync(T t)
        {
            context.Set<T>().Update(t);

            return Task.CompletedTask;
        }
    }
}
