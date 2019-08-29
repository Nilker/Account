using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
