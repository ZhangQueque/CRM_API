using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Data.Repository
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    public interface IBaseRepositoryT<T>
         where T : class
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <returns></returns>
        Task AddAsync(T t);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <returns></returns>
        Task DeleteAsync(T t);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <returns></returns>
        Task UpdateAsync(T t);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<T>> GetAllAsync();



        /// <summary>
        /// 保存到数据库
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
