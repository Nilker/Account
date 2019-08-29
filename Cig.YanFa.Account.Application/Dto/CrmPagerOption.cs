using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Application.Dto
{
    /// <summary>
    /// 分页option属性
    /// </summary>
    public class CrmPagerOption: DataTableParameter
    {
        public CrmPagerOption()
        {
            PageSize = 10;
        }
        /// <summary>
        /// 当前页  必传
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总条数  必传
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 分页记录数（每页条数 默认每页15条）
        /// </summary>
        public int PageSize { get; set; }

        
    }



    public class DataTableParameter
    {
        /// <summary>
        /// DataTable Draw记录  前端--》后端---》前端 不会发生
        /// </summary>
        public int Draw { get; set; }
        //public int draw { get; set; }
        //public int length { get; set; }
        //public int start { get; set; }
        public List<columm> columns { get; set; }
    }
    public struct columm
    {
        public string data;
        public string name;
        public Boolean searchable;
        public Boolean orderable;
       // public searchValue Search;
    }
    public struct searchValue
    {
        public string value;
        public Boolean regex;
    }
}
