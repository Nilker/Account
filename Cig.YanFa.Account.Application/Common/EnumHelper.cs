using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cig.YanFa.Account.Application.Common
{
    public class EnumHelper
    {
        public static Dictionary<int, string> GetValueAndDesc<T>()
        {

            Dictionary<int, string> dic = new Dictionary<int, string>();
            try
            {
                foreach (FieldInfo item in typeof(T).GetFields())
                {
                    if (item.FieldType.IsEnum)
                    {
                        int key = (int)typeof(T).InvokeMember(item.Name, BindingFlags.GetField, null, null, null);
                        DescriptionAttribute[] flags = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (!dic.ContainsKey(key))
                        {
                            dic.Add(key, flags[0].Description);
                        }
                    }
                }

                return dic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
