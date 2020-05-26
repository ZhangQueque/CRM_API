using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BT.Data.Repository
{

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <typeparam name="TId">id</typeparam>
    public interface IBaseRepositoryTID<T, TId>
         where T : class
    {
        /// <summary>
        /// 根据id主键获取对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(TId id);

        /// <summary>
        /// 检测对象是否存在
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<bool> IsExistAsync(TId id);
    }
}
