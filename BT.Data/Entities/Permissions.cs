using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace BT.Data.Entities
{
    /// <summary>
    /// 权限实体类
    /// </summary>
    public class Permissions:BaseEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Leave { get; set; }

        public int Sort { get; set; }

        public string ShortDescribe { get; set; }

        /// <summary>
        /// 唯一code
        /// </summary>
        public string PathCode { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        public int PID { get; set; }


        public string ActiveName { get; set; }

    }
}
