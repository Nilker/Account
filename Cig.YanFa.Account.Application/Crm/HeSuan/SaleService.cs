using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cig.YanFa.HeSuan.Dto;
using Cig.YanFa.Test;
using Cig.YanFa.Account.Application;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;

namespace Cig.YanFa.HeSuan
{
    public class SaleService : YanFaAppServiceBase, ISaleService
    {
        private readonly IDapperRepositoryBase<SaleAchievement,CrmDBContext> _saleAchDapperRepository;
        public SaleService(IDapperRepositoryBase<SaleAchievement,CrmDBContext> saleAchDapperRepository)
        {
            _saleAchDapperRepository = saleAchDapperRepository;
        }


        public PagedResultDto<SaleAchievementListOutPut> GePageList(SaleQueryInput query)
        {
            //var dt = DateTime.Now;
            //var cou = _saleAchDapperRepository.Count(s => s.CreateTime < dt);
            //var saleAchievements = _saleAchDapperRepository.GetAllPaged(s => s.CreateTime < dt, query.CurrentPage - 1, query.PageSize, false, t => t.CreateTime);
            //return new PagedResultDto<SaleAchievement>(Convert.ToInt32(cou), saleAchievements.ToList());

            #region 原有Sql
            var originalSql = @"
                           select s.*,ui.TrueName as SaleManName,ui1.TrueName as 
                           CreateUserName,ui2.TrueName as LastUpdateUserName,
                           ci.CustName, p.ProjectName, a.FileName, a.FilePath   
                           from SaleAchievement s 
                           LEFT JOIN v_CustsAndFriends ci on ci.CustID=s.CustID
                           LEFT JOIN Project p on p.ProjectCode =s.ProjectID 
                           left join AttachFile a on s.FileID = a.RecID 
                           LEFT JOIN v_userinfo ui on ui.UserID=s.SaleManID 
                           LEFT JOIN v_userinfo ui1 on ui1.UserID=s.CreateUserID 
                           LEFT JOIN v_userinfo ui2 on ui2.UserID=s.[LastUpdateUserID] 
                           where s.ID like '%'+'" + IsNullorEmpty(query.ID) +
                                            "'+'%' and s.SaleManID like '" + IsNullorEmpty(query.SaleManID) +
                                            "' and s.CustID like '" + IsNullorEmpty(query.CustID) +
                                            "' and s.ProjectID like '" + IsNullorEmpty(query.ProjectID) +
                                            "' and ui1.TrueName like '%'+'" + IsNullorEmpty(query.CreateUserName) +
                                            "'+'%' and ui2.TrueName like '%'+'" + IsNullorEmpty(query.LastUpdateUserName) +
                                            "'+'%'";
            if (!String.IsNullOrEmpty(query.AchievementStartTime))
            {
                originalSql += " and s.AchievementStartTime = '" + IsNullorEmpty(query.AchievementStartTime) +
                          "' ";
            }

            if (!String.IsNullOrEmpty(query.AchievementEndTime))
            {
                originalSql += " and s.AchievementEndTime = '" + IsNullorEmpty(query.AchievementEndTime) +
                          "' ";
            } 
            #endregion

            var count = _saleAchDapperRepository.QueryBySql<int>($"select count(1) as count from ({originalSql}) tem ",null).FirstOrDefault();
            var pagerSql = GetPagerSql(originalSql, "CustID", query.CurrentPage, query.PageSize);
            var models = _saleAchDapperRepository.QueryBySql<SaleAchievementListOutPut>(pagerSql,null);

            return new PagedResultDto<SaleAchievementListOutPut>(Convert.ToInt32(count), models.ToList());
        }


        private string IsNullorEmpty(string value)
        {
            if (value == null | value == "" | value == "-1")
            {
                value = "%";
            }

            return value;
        }
    }
}
