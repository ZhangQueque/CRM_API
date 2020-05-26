using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 员工角色关联实体表
    /// </summary>
    public class Employees_Roles : BaseEntity
    {
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }
        public int RoleID { get; set; }

        public string RoleName { get; set; }
    }
}
