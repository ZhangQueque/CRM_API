using BT.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{

    /// <summary>
    /// 员工信息Dto
    /// </summary>
    public class EmployeeInfoDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public string HeadImage { get; set; }

        public List<Permissions> Permissions { get; set; } = new List<Permissions>();

    }
}
