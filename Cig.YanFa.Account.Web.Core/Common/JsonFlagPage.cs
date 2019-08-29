using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cig.YanFa.Account.Web.Core.Common
{
    public class JsonFlagPage
    {
        public JsonFlagPage(int Draw,int Total,object Data)
        {
            draw = Draw;
            recordsTotal = Total;
            recordsFiltered = Total;
            data = Data;
        }

        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object data { get; set; }
    }


}
