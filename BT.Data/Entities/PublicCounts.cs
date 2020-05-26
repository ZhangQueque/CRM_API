using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 公共计数表
    /// </summary>
   public class PublicCounts
    {
        [Key]
        public int ID { get; set; }

        public string Type { get; set; }

        public  int Value { get; set; }

        public int IsDel { get; set; }
    }
}
