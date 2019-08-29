using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.HeSuan.Dto;
using Cig.YanFa.Account.Application.Dto;

namespace Cig.YanFa.HeSuan
{
    public interface ISaleService
    {
        PagedResultDto<SaleAchievementListOutPut> GePageList(SaleQueryInput query);
    }
}
