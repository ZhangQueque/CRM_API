using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
    public class PermissionCreateDto
    {
        public string Name { get; set; }

        public string Icon { get; set; }
        public int PID { get; set; }

        public string Url { get; set; }
    }
}
