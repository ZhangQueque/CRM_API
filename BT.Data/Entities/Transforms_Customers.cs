using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 客户转换记录表
    /// </summary>
    public class Transforms_Customers:BaseEntity
    {
        public int FromEmployeesID { get; set; }

        public int ToEmployeesID { get; set; }

        /// <summary>
        /// 0 正在转换中，1转换成功
        /// </summary>
        public int Status { get; set; }
    }
}
