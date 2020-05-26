using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
    public class OrderCreateDto
    {
        public int CustomerID { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
