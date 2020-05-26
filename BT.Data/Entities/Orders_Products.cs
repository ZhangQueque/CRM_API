using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data.Entities
{
    public class Orders_Products : BaseEntity
    {
        public int OrderID { get; set; }


        public int ProductID { get; set; }
        public string ProductTitle { get; set; }

        public int Num { get; set; }

     }
}
