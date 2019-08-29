using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Dto;

namespace Cig.YanFa.HeSuan.Dto
{
    public class SaleQueryInput : CrmPagerOption
    {
        public string SaleManID { get; set; }
        public string CustID { get; set; }
        public string CreateUserName { get; set; }
        public string ProjectID { get; set; }
        public string ID { get; set; }
        public string LastUpdateUserName { get; set; }
        public string AchievementStartTime { get; set; }
        public string AchievementEndTime { get; set; }
    }
}
