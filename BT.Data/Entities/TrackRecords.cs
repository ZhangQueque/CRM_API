using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data.Entities
{
    public class TrackRecords:BaseEntity
    {

        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
