using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Repository
{
    //public interface IUnitOfWorkFactory
    public interface IUnitOfWorkFactory<DapperXXContext> where DapperXXContext:IContext//,new()
    {
        IUnitOfWork Create();
    }
   
}
