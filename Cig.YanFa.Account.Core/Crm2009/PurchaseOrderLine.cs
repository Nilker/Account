using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:05:08
/// </summary>
namespace  Cig.YanFa.Account.Core.Crm2009  
{
    /// <summary>
    ///  PurchaseOrderLine 描述：
    /// </summary>
    [Table("PurchaseOrderLine")]
    public class PurchaseOrderLine
    {

        public PurchaseOrderLine()
        {
            PoActualConfirmations=new List<POActualConfirmation>();
        }

            /// <summary>        
            ///
            /// </summary>
            [Key]
            public int POLineID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int POLineNum{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string POCode{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string ProjectCode{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string MaterialClassId{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string MaterialId{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int ExpenseItem{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string PurchaseType{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string SecondProduct{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Description{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string ConfirmStyle{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string BeginTime{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string EndTime{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Notes{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public decimal UnitPrice{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Quantity{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public decimal Amount{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Currency{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public decimal LocalCurrencyAmount{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string TaxCode{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public decimal NoTaxAmount{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public decimal TaxAmount{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string InvoiceType{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Status{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Unit{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int ErpLineID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int ERPLine{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string PROJECT_NUMBER_CODE{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string DEPARTMENT_CODE{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string CUSTOMER_CODE{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int PayType{ get;set; }
        public List<POActualConfirmation> PoActualConfirmations { get; set; }
    }
 }