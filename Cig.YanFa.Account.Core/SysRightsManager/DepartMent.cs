using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:06:45
/// </summary>
namespace Cig.YanFa.Account.Core.SysRightsManager
{
    /// <summary>
    ///  DepartMent 描述：
    /// </summary>
    public class DepartMent
    {


        [Key]
        [Identity]
        public int RecID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string DepartID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Pid{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string HRDepartID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string HRDepartPid{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string DepartName{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string AbbrName{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Level{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Status{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public DateTime CreateTime{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string BusinessLineID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string DistrictID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Gid{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Address{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string PostCode{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Tele{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Fax{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Remark{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int DepartType{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int DepartKind{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string DepartPath{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string StopTime{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public byte[] TIMESTAMP{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int DepartCityType{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string NamePath{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int BussinessCategory{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int ManagerID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int BudgetManagerID{ get;set; }
     }
 }