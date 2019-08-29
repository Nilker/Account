using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cig.YanFa.Account.Application.Crm.CommonData.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using Cig.YanFa.Account.Repository.Repository.Interface;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace Cig.YanFa.Account.Application.Crm.PurchaseOrder
{
    public class CostAccountConfigService : ICostAccountConfigService
    {

        #region Injection
        private readonly IDapperRepositoryBase<CostAccountConfig, CrmDBContext> _costAccountConfigRepository;
        private readonly ICommonDataService _commonDataService;



        public CostAccountConfigService(
                 IDapperRepositoryBase<CostAccountConfig, CrmDBContext> costAccountConfigRepository
                , ICommonDataService commonDataService
                )
        {

            _costAccountConfigRepository = costAccountConfigRepository;
            _commonDataService = commonDataService;

        }
        #endregion

        /// <summary>
        /// 添加 成本支出核算科目配置表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCostAccountConfig(CostAccountConfigInput model)
        {
            var expression = Expression(model);
            var count = _costAccountConfigRepository.Count(expression);
            if (count > 0)
            {
                return false;
            }

            var n = _costAccountConfigRepository.Add(new CostAccountConfig()
            {
                CompanyCode = model.CompanyCode,
                OrderType = model.OrderType,
                ContentType = model.ContentType,
                ProductCat1 = model.ProductCat1,
                ProductCat2 = model.ProductCat2,
                PayOutType1 = model.PayOutType1,
                PayOutType2 = model.PayOutType2
            });
            return n > 0;
        }

        public PagedResultDto<CostAccountConfigOut> GetAccountConfigsByPage(CostAccountConfigInput queryModel)
        {
            #region expression表达式
            var expression = Expression(queryModel);

            #endregion

            //var count = _costAccountConfigRepository.Count(expression);
            var model = _costAccountConfigRepository.PageListSelect(expression, s => new CostAccountConfig()
            {
                Id = s.Id,
                CompanyCode = s.CompanyCode,
                OrderType = s.OrderType,
                ContentType = s.ContentType,
                ProductCat1 = s.ProductCat1,
                ProductCat2 = s.ProductCat2,
                PayOutType1 = s.PayOutType1,
                PayOutType2 = s.PayOutType2
            }, queryModel.CurrentPage, queryModel.PageSize, new Tuple<Expression<Func<CostAccountConfig, object>>, SortOrder>(s => s.Id, SortOrder.Descending));

            //签约公司
            var companyInfos = _commonDataService.CompanyInfos();
            //订单类型
            var relevanceInfos = _commonDataService.GetRelevanceInfos(5);
            //采购类型
            var contentTypes = _commonDataService.GetDicInfos(701);
            //物料分类
            var productCat1s = _commonDataService.GetProductCategorys(level: 1, type: "MaterialClass");
            //物料
            var productCat2s = _commonDataService.GetProductCategorys(level: 2, type: "Material");
            //支出类型1
            var payOutType1s = _commonDataService.GetDicInfos(723);
            var payOutType2s = new List<DicInfoDto>();
            payOutType1s.ForEach(s =>
            {
                payOutType2s.AddRange(_commonDataService.GetDicInfos(s.DictId));
            });

            var models = new List<CostAccountConfigOut>();
            foreach (var item in model.Items)
            {
                var tem = new CostAccountConfigOut()
                {
                    Id = item.Id,
                    CompanyCode = item.CompanyCode,
                    CompanyName = item.CompanyCode == "-99" ? "缺省" : companyInfos.Where(s => s.CompanyCode == item.CompanyCode.ToString()).Select(s => s.CompanyName).FirstOrDefault(),
                    OrderType = item.OrderType,
                    OrderTypeName = relevanceInfos.Where(s => s.Key == item.OrderType.ToString()).Select(s => s.Value).FirstOrDefault(),
                    ContentType = item.ContentType,
                    ContentTypeName = item.ContentType == -99 ? "缺省" : contentTypes.Where(s => s.DictId == item.ContentType).Select(s => s.DictName).FirstOrDefault(),
                    ProductCat1 = item.ProductCat1,
                    ProductCat1Name = productCat1s.Where(s => s.ProductCatID == item.ProductCat1.ToString()).Select(s => s.Name).FirstOrDefault(),
                    ProductCat2 = item.ProductCat2,
                    ProductCat2Name = item.ProductCat2 == "-99" ? "缺省" : productCat2s.Where(s => s.ProductCatID == item.ProductCat2.ToString()).Select(s => s.Name).FirstOrDefault(),
                    PayOutType1 = item.PayOutType1,
                    PayOutType1Name = payOutType1s.Where(s => s.DictId == item.PayOutType1).Select(s => s.DictName).FirstOrDefault(),
                    PayOutType2 = item.PayOutType2,
                    PayOutType2Name = item.PayOutType2 == -99 ? "缺省" : payOutType2s.Where(s => s.DictId == item.PayOutType2).Select(s => s.DictName).FirstOrDefault(),
                };
                models.Add(tem);
            }


            return new PagedResultDto<CostAccountConfigOut>(model.Total, models);
        }


        public bool DeleteCostAccountConfig(int id, int userId)
        {
            var num = _costAccountConfigRepository.UpdateWhere(s => s.Id == id, new CostAccountConfig()
            {
                Status = 1,
                LastUpdateTime = DateTime.Now,
                LastUpdateUserID = userId
            }, s => s.Status, s => s.LastUpdateTime, s => s.LastUpdateUserID);

            return num > 0;
        }



        private static Expression<Func<CostAccountConfig, bool>> Expression(CostAccountConfigInput queryModel)
        {
            Expression<Func<CostAccountConfig, bool>> expression = s => s.Status == 0;
            if (!string.IsNullOrEmpty(queryModel.CompanyCode))
            {
                expression = expression.And(s => s.CompanyCode == queryModel.CompanyCode);
            }

            if (queryModel.OrderType != 0)
            {
                expression = expression.And(s => s.OrderType == queryModel.OrderType);
            }

            if (queryModel.ContentType != 0)
            {
                expression = expression.And(s => s.ContentType == queryModel.ContentType);
            }

            if (!string.IsNullOrEmpty(queryModel.ProductCat1))
            {
                expression = expression.And(s => s.ProductCat1 == queryModel.ProductCat1);
            }

            if (!string.IsNullOrEmpty(queryModel.ProductCat2))
            {
                expression = expression.And(s => s.ProductCat2 == queryModel.ProductCat2);
            }

            if (queryModel.PayOutType1 != 0)
            {
                expression = expression.And(s => s.PayOutType1 == queryModel.PayOutType1);
            }

            if (queryModel.PayOutType2 != 0)
            {
                expression = expression.And(s => s.PayOutType2 == queryModel.PayOutType2);
            }

            return expression;
        }

    }
}
