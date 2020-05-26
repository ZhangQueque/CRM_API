using BT.Data.Entities;
using BT.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Service.Log
{
    public  class LogRepository : BaseRepository<Logs, int>, ILogRepository
    {
        public LogRepository(DbContext context) : base(context)
        {
                
        }
    }
}
