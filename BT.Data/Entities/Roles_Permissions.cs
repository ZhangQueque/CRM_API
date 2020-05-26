using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 角色_权限关系实体类
    /// </summary>
   public class Roles_Permissions:BaseEntity
    {
        public int RoleID { get; set; }

        public string RoleName { get; set; }
        public int PermissionID { get; set; }

        public string PermissionName { get; set; }


    }
}
