using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace BT.Service.LogCategory
{
    /// <summary>
    /// 日志类别仓储接口
    /// </summary>
    public interface ILogCategoryRepository : IBaseRepositoryT<LogCategories>, IBaseRepositoryTID<LogCategories, int>
    {
    }
}
