using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BT.Data.Entities
{

    /// <summary>
    /// 日志实体表
    /// </summary>
    public class Logs : BaseEntity
    {
      

        public string Content { get; set; }

        /// <summary>
        /// 日志类别外键 
        /// </summary>
        public int CategoryID { get; set; }
    }
}
