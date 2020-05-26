using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 订单表
    /// </summary>
    public class Orders:BaseEntity
    {

        public string OrderNo { get; set; }

        public int CustomerID { get; set; }

        public decimal TotalPrice { get; set; }

        public int Status { get; set; }
    }
}
