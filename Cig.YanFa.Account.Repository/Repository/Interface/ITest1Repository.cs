using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cig.YanFa.Test;

namespace Cig.YanFa.Account.Repository.Repository.Interface
{
    public interface ITest1Repository : IDapperRepositoryBase<Test1>
    {
        Task<bool> AddAsync(Test1 prod);

    }
}
