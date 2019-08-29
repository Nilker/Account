using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;

namespace Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto
{
    public  class CostAccountConfigOut: CostAccountConfigInput
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string OrderTypeName { get; set; }
        public string ContentTypeName { get; set; }
        public string ProductCat1Name { get; set; }
        public string ProductCat2Name { get; set; }
        public string PayOutType1Name { get; set; }
        public string PayOutType2Name { get; set; }
    }
}
