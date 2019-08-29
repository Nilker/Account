using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cig.YanFa.Test;
using Cig.YanFa.Account.Repository.Repository.Interface;

namespace Cig.YanFa.Account.Repository.Repository
{
    public class Test1Repository : DapperRepositoryBase<Test1>, ITest1Repository
    {
        private readonly DapperDBContext _context;
        public Test1Repository(DapperDBContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> AddAsync(Test1 prod)
        {
            throw new NotImplementedException();
        }
    }
}
