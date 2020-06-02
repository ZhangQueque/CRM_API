using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
   public  class EmployeeExcelDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage { get; set; }
    }
}
