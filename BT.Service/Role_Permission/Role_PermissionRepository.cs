using BT.Core;
using BT.Data;
using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BT.Service.Role_Permission
{
    public class Role_PermissionRepository : BaseRepository<Roles_Permissions, int>, IRole_PermissionRepository
    {
        public Role_PermissionRepository(DbContext context) : base(context)
        {

        }
        /// <summary>
        /// 根据角色获取对应的权限记录
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public Task<IQueryable<Roles_Permissions>> GetRoles_PermissionsByRoleID(int roleId)
        {
            return Task.FromResult(context.Set<Roles_Permissions>().Where(m => m.RoleID == roleId &&m.IsDel==0));
        }
    }
}
