using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.UnTest;
using NUnit.Framework;

namespace Tests
{
    public class Tests:BaseTest
    {
        private IPurchaseOrderService _purchaseOrderService;
        public Tests(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }


        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}