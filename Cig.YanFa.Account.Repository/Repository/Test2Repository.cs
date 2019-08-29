using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cig.YanFa.Test;
using Cig.YanFa.Account.Repository.Repository.Interface;

namespace Cig.YanFa.Account.Repository.Repository
{
    public class Test2Repository : DapperRepositoryBase<Test2, SysRightsManagerDBContext>, ITest2Repository
    {

        private readonly SysRightsManagerDBContext _context;

        public Test2Repository(SysRightsManagerDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
