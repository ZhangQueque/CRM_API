using BT.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
   public  class Orders_ProductsCreateDto
    {
        public OrderCreateDto OrderCreateDto { get; set; }

        public List<ProductsCreateDto> ProductsCreateDtos { get; set; }
    }
}
