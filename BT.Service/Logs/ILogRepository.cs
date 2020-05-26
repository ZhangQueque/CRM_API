using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BT.Service.Log
{
    /// <summary>
    /// 日志仓储接口
    /// </summary>
    public interface ILogRepository : IBaseRepositoryT<Logs>, IBaseRepositoryTID<Logs, int>
    {
    }
}
