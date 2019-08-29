using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Cig.YanFa.HeSuan
{
    [Table("SaleAchievement")]
    public class SaleAchievement 
    {
       [Key]
        public  string ID { get; set; }

       
        public  string SaleManID { get; set; }

       
        public  string CustID { get; set; }

       
        public  string ProjectID { get; set; }

       
        public  string FileID { get; set; }

        public  decimal? AchievementAmount { get; set; }

       
        public  DateTime AchievementStartTime { get; set; }

       
        public  DateTime AchievementEndTime { get; set; }

       
        public  DateTime CreateTime { get; set; }

       
        public  int CreateUserID { get; set; }

       
        public  DateTime LastUpdateTime { get; set; }

       
        public  int LastUpdateUserID { get; set; }


    }
}
