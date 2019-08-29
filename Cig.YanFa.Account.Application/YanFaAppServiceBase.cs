using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Repository;
using Microsoft.Extensions.Configuration;


namespace Cig.YanFa.Account.Application
{
    public  class YanFaAppServiceBase
    {
     
        /// <summary>
        /// 原有Sql,加装分页信息；
        /// </summary>
        /// <param name="originalSql">原始Sql:YanFaFrom内部的sql</param>
        /// <param name="orderByStr">排序字段，需要是原始Sql中的一个字段</param>
        /// <param name="currentPage">当前页，第一页为：1</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public string GetPagerSql(string originalSql, string orderByStr, int currentPage, int pageSize)
        {
            //分页查询（通用型）
            var sql = $@"
                        select tem.* from ({originalSql}) as tem
                        order by tem.{orderByStr} 
                        offset {(currentPage - 1) * pageSize}  rows
                        fetch next {pageSize} rows only;
            ";
            return sql;
        }
    }
}
