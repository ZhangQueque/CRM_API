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


namespace BT.Service.Permission
{
    public class PermissionRepository : BaseRepository<Permissions, int>, IPermissionRepository
    {
        public PermissionRepository(DbContext context) : base(context)
        {

        }
    }
}
