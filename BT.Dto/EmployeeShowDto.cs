using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
     public  class EmployeeShowDto
    {
        public int ID { get; set; }
        public int IsDel { get; set; }

        public int CreateID { get; set; }

        public string CreateTime { get; set; }

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
