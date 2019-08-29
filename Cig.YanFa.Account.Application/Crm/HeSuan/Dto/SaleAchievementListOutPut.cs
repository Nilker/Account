using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.HeSuan.Dto
{
    public class SaleAchievementListOutPut: SaleAchievement
    {
        /*
         * ,ui.TrueName as SaleManName,ui1.TrueName as 
                           CreateUserName,ui2.TrueName as LastUpdateUserName,
                           ci.CustName, p.ProjectName, a.FileName, a.FilePath
         */

        public string SaleManName { get; set; }
        public string CreateUserName { get; set; }
        public string LastUpdateUserName { get; set; }
        public string CustName { get; set; }
        public string ProjectName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
