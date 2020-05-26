using BT.Core;
using BT.Data;
using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using BT.Service.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BT.Service.Role
{
    public  class RoleRepository :BaseRepository<Roles, int>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {

        }
    }
}
