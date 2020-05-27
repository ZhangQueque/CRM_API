using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
    public class RolesShowDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int IsDel { get; set; }

        public int CreateID { get; set; }

        public string CreateTime { get; set; }
    }
}
