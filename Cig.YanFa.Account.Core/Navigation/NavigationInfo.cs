using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
 

namespace Cig.YanFa.Navigation
{
    /// <summary>
    /// 菜单栏
    /// </summary>
    [Table("moduleinfo")]
    public class NavigationInfo  
    {
        [Key]
        public int RecID { get; set; }
        public string ModuleID { get; set; }
        public string SysID { get; set; }
        public string ModuleName { get; set; }
        public string PID { get; set; }
        public int Level { get; set; }
        public string Intro { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string Links { get; set; }
        public int OrderNum { get; set; }
        public int TYPE { get; set; }
        public string CssClass { get; set; }
        [NotMapped]
        public string Domain { get; set; }
        public int Blank { get; set; }

        public string DomainCode { get; set; }

    }
}
