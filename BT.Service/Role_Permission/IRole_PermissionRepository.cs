using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BT.Service.Role_Permission
{
    public interface IRole_PermissionRepository : IBaseRepositoryT<Roles_Permissions>, IBaseRepositoryTID<Roles_Permissions, int>
    {
        /// <summary>
        /// 根据角色获取对应的权限记录
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        Task<IQueryable<Roles_Permissions>> GetRoles_PermissionsByRoleID(int roleId);
    }
}
