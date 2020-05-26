using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 日志类别实体类
    /// </summary>
    public class LogCategories : BaseEntity
    {
        /// <summary>
        /// 日志类别名称
        /// </summary>
        public string Title { get; set; }
    }
}
