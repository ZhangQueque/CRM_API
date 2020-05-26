using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BT.Data.Entities
{

    /// <summary>
    /// 员工实体对象
    /// </summary>
    public class Employees: BaseEntity
    {
      
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 唯一code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 父类id
        /// </summary>
        public int EmployeePID { get; set; }

      

    }
}
