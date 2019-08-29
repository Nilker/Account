using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cig.YanFa.Account.Application.Crm.Expense.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Core.Crm2009;

namespace Cig.YanFa.Account.Application.Common
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PurchaseOrderLineConfirmHistory, PurchaseOrderLineConfirmHistoryExportDto>().ReverseMap();
            CreateMap<PurchaseRequirementDeptShareRatioVersion, PurchaseRequirementDeptShareRatioVersionExportDto>().ReverseMap();
            CreateMap<CostAccount, CostAccountExportDto>().ReverseMap();
            CreateMap<ExpenseAccount,ExpenseLineConfirmHistory>()
                //.ForMember(dest=>dest.Id,op=>op.UseDestinationValue())
                //.ForMember(s=>s.LastUpdateTime,op=>op.Ignore());
                //.ForPath(s=>s.LastUpdateTime,op=>op.Ignore())
                .ReverseMap();

            CreateMap<ExpenseAccount, ExpenseOut>()
                //.ForMember(des=>des.ExpenseCode,o=>o.MapFrom(s=>s.ExpenseCode))
                //.ForMember(des=>des.ExpenseLineNum,o=>o.MapFrom(s=>s.ExpenseLineNum))
                //.ForMember(des=>des.ExpenseCode,o=>o.MapFrom(s=>s.POCode))
                .ReverseMap();
            CreateMap<ExpenseAccount, ExpenseAccountExportDto>().ReverseMap();
            //CreateMap<PurchaseOrderLineConfirmHistory, CostAccount>()
            //    .ForMember(s=>s.Id,op=>op.Ignore())
            //    .ForMember(s=>s.DepathNamePath,d=>d.MapFrom(s=>s.DepartNamePath))
            //    .
            
            CreateMap<ExpenseLineConfirmHistory, ExpenseLineConfirmHistoryExportDto>().ReverseMap();
            CreateMap<ExpenseOut, ExpenseLineConfirmHistory>().ReverseMap();
            CreateMap<ExpenseAccount, ExpenseLineConfirmHistoryExportDto>().ReverseMap();
            CreateMap<v_AchievementBusinessDepartMappingVersion, v_AchievementBusinessDepartMappingVersionExportDto>().ReverseMap();
        }
    }
}
