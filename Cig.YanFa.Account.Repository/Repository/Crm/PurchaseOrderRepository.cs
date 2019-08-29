using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cig.YanFa.Test;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Repository.Repository.Interface;
using Dapper;

namespace Cig.YanFa.Account.Repository.Repository
{
    public class PurchaseOrderRepository : DapperRepositoryBase<PurchaseOrder, CrmDBContext>, IPurchaseOrderRepository
    {

        private readonly CrmDBContext _context;

        public PurchaseOrderRepository(CrmDBContext context) : base(context)
        {
            _context = context;
        }

        public List<PurchaseOrder> GetEntities()
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            var sql = $@"
SELECT po.POCode,po.ProjectCode,po.CompanyCode,po.SupplierId,po.ContractType,pro.ProjectType,pro.CostDepartID
,pol.POLineID, pol.MaterialClassId,pol.MaterialId,pol.ExpenseItem
,poac.ConfirmationID, poac.POLineNum,poac.ActualPeriod,poac.ActualAmount
FROM PurchaseOrder po
LEFT JOIN dbo.Project pro ON pro.ProjectCode=po.ProjectCode AND pro.Status=0
LEFT JOIN dbo.PurchaseOrderLine pol ON pol.POCode=po.POCode AND pol.Status=0
LEFT JOIN dbo.POActualConfirmation poac ON poac.POCode = po.POCode AND pol.POLineNum=poac.POLineNum and poac.ConfirmationID!=0
WHERE poac.Status=0
";
            var lookUp = new Dictionary<string, PurchaseOrder>();
            var lookUp2 = new Dictionary<int, PurchaseOrderLine>();
            var entities = _connection.Query<PurchaseOrder, PurchaseOrderLine,POActualConfirmation, PurchaseOrder>(sql,
                   (po, pol,poac) =>
                   {
                       if (!lookUp.TryGetValue(po.POCode, out var tem))
                       {
                           lookUp.Add(po.POCode, tem = po);
                       }
                       if (!lookUp2.TryGetValue(pol.POLineID, out var tem2))
                       {
                           lookUp2.Add(pol.POLineID, tem2 = pol);
                       }
                       tem2.PoActualConfirmations.Add(poac);
                       tem.PurchaseOrderLines.Add(tem2);

                       tem.PurchaseOrderLines= tem.PurchaseOrderLines.Distinct().ToList();
                       return tem;
                   }, splitOn: "POLineID,ConfirmationID").Distinct().ToList();

            return entities;
        }
    }
}
