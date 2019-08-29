using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cig.YanFa.Account.Web.Core.Common
{
    /// <summary>
    /// 返回json
    /// </summary>
    public class JsonFlag
    {
        /// <summary>
        /// 失败---异常消息
        /// </summary>
        /// <param name="ex"></param>
        public JsonFlag(Exception ex)
            : this(ex, null)
        { }

        /// <summary>
        /// 失败--异常消息和数据
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="data"></param>
        public JsonFlag(Exception ex, object data)
            : this(0, WebUtility.UrlEncode(ex.Message), data)
        { }

        /// <summary>
        /// 成功返回--消息和数据
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        public JsonFlag(string msg, object data)
            : this(1, msg, data)
        { }

        /// <summary>
        /// 返回操作消息
        /// </summary>
        /// <param name="flag">1 成功， 0 失败</param>
        /// <param name="msg">消息内容</param>
        public JsonFlag(int flag, string msg)
            : this(flag, msg, null)
        { }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <param name="data">数据或列表</param>
        public JsonFlag(object data) : this(1, null, data) { }

        /// <summary>
        /// 返回Json格式化对象
        /// </summary>
        /// <param name="flag">1 成功， 0 失败</param>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">json 格式对象</param>
        public JsonFlag(int flag, string msg, object data)
        {
            this.Flag = flag;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// 1 成功， 0 失败
        /// </summary>
        public int Flag { get; set; }

        /// <summary>
        /// json 格式对象
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Msg { get; set; }

    }
}
