using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Application.Crm.CommonData.Dto
{
    public  class ProductCategoryDto
    {
        public string ProductCatID { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int ParentID { get; set; }

        public string Type { get; set; }
    }
}
