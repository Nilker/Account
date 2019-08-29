using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Repository
{
    //public class UnitOfWorkFactory : IUnitOfWorkFactory
    //{
    //private readonly DapperDBContext _context;
    //public UnitOfWorkFactory(DapperDBContext context)
    //{
    //    _context = context;
    //}
    //public IUnitOfWork Create()
    //{
    //    return new UnitOfWork(_context);
    //}
    //}

    public class UnitOfWorkFactory<DapperXXContext> : IUnitOfWorkFactory<DapperXXContext>
            where DapperXXContext : IContext//, new()
    {
        private readonly DapperXXContext _context;
        public UnitOfWorkFactory(DapperXXContext context)
        {
            _context = context;
        }
        public IUnitOfWork Create()
        {
            return new UnitOfWork(_context);
        }
    }
}
