using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 商品表
    /// </summary>
    public class Products:BaseEntity
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ShortDescribe { get; set; }

        public string Picture { get; set; }
    }
}
