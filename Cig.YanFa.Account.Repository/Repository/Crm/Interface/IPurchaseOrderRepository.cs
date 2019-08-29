using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cig.YanFa.Test;
using Cig.YanFa.Account.Core.Crm2009;

namespace Cig.YanFa.Account.Repository.Repository.Interface
{
    public interface IPurchaseOrderRepository : IDapperRepositoryBase<PurchaseOrder, DapperDBContext>
    {
        List<PurchaseOrder> GetEntities();
    }
}
