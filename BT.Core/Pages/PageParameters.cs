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


        public int EmployeeIDByFind { get; set; } = 0;

        public int CustomerSegmentationIDByFind { get; set; } = 0;

        public string CustomerNameByFind { get; set; } = "";

        public DateTime? StartTimeByFind { get; set; }    
        public DateTime? EndTimeByFind { get; set; }

        public int RoleIDByFind { get; set; } = 0;
        public string RoleName { get; set; } = "";
        public DateTime? CreateTimeByFind { get; set; }

        public int Status { get; set; } = 0;

    }
}
