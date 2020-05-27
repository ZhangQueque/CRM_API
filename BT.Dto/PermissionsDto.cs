using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BT.Dto
{
    public class PermissionsDto
    {

        public int ID { get; set; }
        public int IsDel { get; set; }

        public int CreateID { get; set; }

        public string CreateTime { get; set; }



        public string Name { get; set; }


        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        public int PID { get; set; }


        [JsonPropertyName("LAY_CHECKED")]
        public bool LAY_CHECKED { get; set; }

        public bool Open { get; set; } = true;
    }
}
