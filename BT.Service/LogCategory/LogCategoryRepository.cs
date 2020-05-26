using BT.Core;
using BT.Data;
using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace BT.Service.LogCategory
{
    public  class LogCategoryRepository : BaseRepository<LogCategories, int>, ILogCategoryRepository
    {
        public LogCategoryRepository(DbContext context) : base(context)
        {

        }

    }
}
