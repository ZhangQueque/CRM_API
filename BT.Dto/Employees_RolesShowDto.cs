using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
    /// <summary>
    /// 员工角色实体关联表
    /// </summary>
    public class Employees_RolesShowDto
    {
        public int ID { get; set; }
        public int IsDel { get; set; }

        public int CreateID { get; set; }

        public string CreateTime { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public int RoleID { get; set; }

        public string RoleName { get; set; }
    }
}
