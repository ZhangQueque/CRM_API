using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Core.Pages
{
    public class PageParameters
    {

        //private int pageIndex=1;

        //public int PageIndex
        //{
        //    get { return pageIndex; }
        //    set { pageIndex = value; }
        //}

        //private int pageSize=10;

        //public int PageSize
        //{
        //    get { return pageSize; }
        //    set { pageSize = value>300? 300 : value; }
        //}


        private int page = 1;

        public int Page
        {
            get { return page; }
            set { page = value; }
        }

        private int limit = 10;

        public int Limit
        {
            get { return limit; }
            set { limit = value > 300 ? 300 : value; }
        }

        //客户查询字段
        public int EmployeeIDByFind { get; set; } = 0;

        public int CustomerSegmentationIDByFind { get; set; } = 0;

        public string CustomerNameByFind { get; set; } = "";


        //员工查询字段
        //public int EmployeeRoleIDByFind { get; set; } = 0;

        //public int EmployeeeSegmentationIDByFind { get; set; } = 0;

        public string EmployeeNameByFind { get; set; } = "";

        public DateTime? StartTimeByFind { get; set; }    
        public DateTime? EndTimeByFind { get; set; }


        public int Status { get; set; } = 0;

    }
}
