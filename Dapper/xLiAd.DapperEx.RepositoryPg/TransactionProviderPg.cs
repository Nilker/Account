﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.RepositoryPg
{
    public class TransactionProviderPg : TransactionProviderBase
    {
        /// <summary>
        /// 不让随便 new
        /// </summary>
        /// <param name="_con"></param>
        internal TransactionProviderPg(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_con, repoXmlProvider, exceptionHandler, throws)
        {

        }
        /// <summary>
        /// 获取事务仓储对象
        /// </summary>
        /// <typeparam name="T">请确认类对应的数据表在此仓储里</typeparam>
        /// <returns></returns>
        public override IRepository<T> GetRepository<T>()
        {
            return new RepositoryPg<T>(Connection, this.RepoXmlProvider, ExExceptionHandler, Throws, Transaction);
        }
    }
}
