using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Core.Pages
{
    /// <summary>
    /// 分页帮助类
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    public   class PageList<T>
        where T:class
    {

        public PageList(IQueryable<T> source,int pageIndex,int pageSize,int count)
        {
            Source = source;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            PageTotal = (int)Math.Ceiling((decimal)count / pageSize); //取于有的话就+1
        }

        public IQueryable<T> Source { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int Count { get; }
        public int PageTotal { get; set; }

        public static  Task< PageList<T>>CreatePageList(IQueryable<T> source,int pageIndex,int pageSize)
        {
            int count = source.Count();

            source = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var pagelist = new PageList<T>(source, pageIndex, pageSize, count);

            return Task.FromResult(pagelist);
        }
 
    }
}
