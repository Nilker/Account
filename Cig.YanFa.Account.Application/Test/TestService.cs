using System;
using Cig.YanFa.Account.Application;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using Cig.YanFa.Account.Repository.Repository.Interface;
using Microsoft.Extensions.Configuration;

namespace Cig.YanFa.Test
{
    public class TestService : YanFaAppServiceBase, ITestService
    {

        public readonly IUnitOfWorkFactory<SysRightsManagerDBContext> _uowSysRightsManager;
        public readonly IUnitOfWorkFactory<CrmDBContext> _uowCrm;
        public readonly IUnitOfWorkFactory<AcDBContext> _uowAc;
        public readonly IUnitOfWorkFactory<BudgetDBContext> _uowBudget;


        private readonly ITest2Repository _test2Repository;
        private readonly IDapperRepositoryBase<Test1, CrmDBContext> _test1Repository;

        public TestService(ITest2Repository test2Repository
            , IDapperRepositoryBase<Test1, CrmDBContext> test1Repository
            , IUnitOfWorkFactory<SysRightsManagerDBContext> uowSysRightsManager
            , IUnitOfWorkFactory<CrmDBContext> uowCrm
            , IUnitOfWorkFactory<AcDBContext> uowAc
            , IUnitOfWorkFactory<BudgetDBContext> uowBudget)
        {
            _test2Repository = test2Repository;
            _test1Repository = test1Repository;

            _uowSysRightsManager = uowSysRightsManager;
            _uowCrm = uowCrm;
            _uowAc = uowAc;
            _uowBudget = uowBudget;
        }


        public void AddBoth()
        {
            #region 工作单元 开启业务层事物

            using (var uow = _uowAc.Create())
            {
                var n1 = _test1Repository.Add(new Test1 { Age = 22, Name = "张三" });
                var n2 = _test2Repository.Add(new Test2 { Content = DateTime.Now.ToString() });
                throw new NotImplementedException("没有实现");
                uow.SaveChanges();
            }

            #endregion

            #region 无工作单元

            var n11 = _test1Repository.Add(new Test1 { Age = 1, Name = $"张三-{DateTime.Now.ToString()}" });
            var n22 = _test2Repository.Add(new Test2 { Content = DateTime.Now.ToLongTimeString() });

            #endregion
        }
    }
}