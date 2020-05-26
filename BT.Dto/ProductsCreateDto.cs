using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
   public class ProductsCreateDto
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ShortDescribe { get; set; }

        public string Picture { get; set; }

        public int Number { get; set; }


        public decimal TotalPrice { get; set; }
    }
}
