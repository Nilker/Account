using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:05:08
/// </summary>
namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    ///  PurchaseOrder 描述：
    /// </summary>
    [Table("PurchaseOrder")]
    public class PurchaseOrder
    {

        public PurchaseOrder()
        {
            PurchaseOrderLines = new List<PurchaseOrderLine>();
        }

        /// <summary>        
        ///
        /// </summary>
        [Key]
        public string POCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string OrderBatchCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string OrderTypeName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int ApproveStatus { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ApproveStatusName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ErpProjectCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ErpProjectName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Address { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CreateDepartId { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int UpdateUserId { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string OrderUserID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string StampUserId { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string StampTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string SaveUserID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string SaveTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public decimal PurchaseAmount { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Notes { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int OrderEnum { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int DutyUserID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ContractSigner { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CustID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ErpContractCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ContractType { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string PoFrame { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Currency { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public decimal RMBAmount { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int SyncStatus { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public decimal ContractAmonut { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ApplyArea { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string BusinessLine { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string SaveUserName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string StampUserName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public bool IsFinacialFinalApprove { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int IsFinalAcceptance { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>        
        ///是否停止付款（0：否，1：是）
        /// </summary>
        public int StopPayment { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int NCState { get; set; }
        public List<PurchaseOrderLine> PurchaseOrderLines { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        [NotMapped]
        public int ProjectType { get; set; }

        /// <summary>
        /// 成本计入部门（取项目）
        /// CostDepartID:非媒介成本计入部门ID
        /// MediaCostDepartID:媒介成本计入部门ID
        /// </summary>
        [NotMapped]
        public string CostDepartID { get; set; }
    }
}