using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cig.YanFa.Account.Application.Crm.CommonData.Dto;
using Cig.YanFa.Navigation;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;
using Cig.YanFa.Account.Core.Ac2010;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Core.SysRightsManager;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using Cig.YanFa.Account.Repository.Repository.Interface;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace Cig.YanFa.Account.Application.PurchaseOrder
{
    public class CommonDataService : YanFaAppServiceBase, ICommonDataService
    {
        #region Injection

        private readonly IDapperRepositoryBase<CompanyInfo, CrmDBContext> _companyInfoRepository;
        private readonly IDapperRepositoryBase<DictInfo, CrmDBContext> _dicRepository;
        private readonly IDapperRepositoryBase<Relevance, CrmDBContext> _relevanceRepository;
        private readonly IDapperRepositoryBase<v_ProductCategory, AcDBContext> _v_ProductCategoryDtoRepository;
        private readonly IDapperRepositoryBase<DepartMent, SysRightsManagerDBContext> _departMentRepository;
        private readonly IDapperRepositoryBase<DepartMentVersion, CrmDBContext> _departVersionRepository;

        public CommonDataService(IDapperRepositoryBase<CompanyInfo, CrmDBContext> companyInfoRepository
        , IDapperRepositoryBase<DictInfo, CrmDBContext> dicRepository
        , IDapperRepositoryBase<Relevance, CrmDBContext> relevanceRepository
        , IDapperRepositoryBase<v_ProductCategory, AcDBContext> v_ProductCategoryDtoRepository
        , IDapperRepositoryBase<DepartMent, SysRightsManagerDBContext> departMentRepository
        , IDapperRepositoryBase<DepartMentVersion, CrmDBContext> departVersionRepository
                )
        {
            _companyInfoRepository = companyInfoRepository;
            _dicRepository = dicRepository;
            _relevanceRepository = relevanceRepository;
            _v_ProductCategoryDtoRepository = v_ProductCategoryDtoRepository;
            _departMentRepository = departMentRepository;
            _departVersionRepository = departVersionRepository;
        }
        #endregion


        public List<CompanyInfoDto> CompanyInfos()
        {
            var companyInfoDtos = _companyInfoRepository.WhereOrderSelect(s => s.Status == 0, s => s.ID, s => new CompanyInfoDto()
            {
                CompanyCode = s.CompanyCode,
                CompanyName = s.CompanyName,
                CompanyType = s.CompanyType
            }).ToList();
            return companyInfoDtos;
        }

        public List<DepartMent> GetDepartMents()
        {
            var tem = _departMentRepository.Where(s=>s.Status==0).ToList();
            return tem;
        }

        public List<DepartMentVersion> GetDepartMentsByVersion(string period)
        {
            var departMentVersions = _departVersionRepository.Where(s=>s.Period==period).ToList();
            return departMentVersions;
        }

        public List<DicInfoDto> GetDicInfos(int dicType = -1)
        {
            Expression<Func<DictInfo, bool>> expression = s => s.Status == 0;
            if (dicType != -1)
            {
                expression = expression.And(s => s.DictType == dicType);
            }

            var models = _dicRepository.WhereOrderSelect(expression, s => s.OrderNum, s => new DicInfoDto()
            {
                DictId = s.DictID,
                DictName = s.DictName,
                DictType = s.DictType
            }).OrderBy(s => s.DictId).ToList();

            return models;
        }

        public List<RelevanceDto> GetRelevanceInfos(int type, int mainType = 0)
        {
            Expression<Func<Relevance, bool>> expression = s => s.Status == 0;
            if (type != 0)
            {
                expression = expression.And(s => s.Type == type);
            }

            if (mainType != 0)
            {
                var tem = mainType.ToString();
                expression = expression.And(s => s.MainType == tem);
            }

            var models = _relevanceRepository.WhereOrderSelect(expression, s => s.Type, s => new RelevanceDto()
            {
                Key = s.SubType,
                Value = s.Remark,
                Type = s.Type
            }).OrderBy(s => s.Key).ToList();

            return models;
        }

        public List<ProductCategoryDto> GetProductCategorys(int level = 0, int parentId = 0, string type = null)
        {
            Expression<Func<v_ProductCategory, bool>> expression = s => s.Status == 1;
            if (level != 0)
            {
                expression = expression.And(s => s.Level == level);
            }

            if (parentId != 0)
            {
                expression = expression.And(s => s.ParentID == parentId);
            }

            if (!string.IsNullOrEmpty(type))
            {
                expression = expression.And(s => s.Type == type);
            }


            var models = _v_ProductCategoryDtoRepository.WhereSelect(expression, s => new ProductCategoryDto()
            {
                ProductCatID = s.ProductCatID,
                Name = s.Name,
                Level = s.Level,
                ParentID = s.ParentID,
                Type = s.Type
            }).ToList();

            return models;
        }
    }
}
