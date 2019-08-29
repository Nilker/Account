using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cig.YanFa.Account.Application.Dto;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cig.YanFa.Account.Web.Core.Components
{
    public class PagerTagHelper : TagHelper
    {
        public CrmPagerOption PagerOption { get; set; }

        public bool IsUp { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            //output.TagName = "div";

            if (PagerOption.PageSize <= 0) { PagerOption.PageSize = 10; }
            if (PagerOption.CurrentPage <= 0) { PagerOption.CurrentPage = 1; }
            if (PagerOption.Total <= 0) { return; }

            //总页数
            var totalPage = PagerOption.Total / PagerOption.PageSize + (PagerOption.Total % PagerOption.PageSize > 0 ? 1 : 0);
            if (totalPage <= 0) { return; }
            //当前路由地址
            //if (string.IsNullOrEmpty(PagerOption.RouteUrl))
            //{

            //    //PagerOption.RouteUrl = helper.ViewContext.HttpContext.Request.RawUrl;
            //    if (!string.IsNullOrEmpty(PagerOption.RouteUrl))
            //    {

            //        var lastIndex = PagerOption.RouteUrl.LastIndexOf("/");
            //        PagerOption.RouteUrl = PagerOption.RouteUrl.Substring(0, lastIndex);
            //    }
            //}
            //PagerOption.RouteUrl = PagerOption.RouteUrl.TrimEnd('/');

            //构造分页样式
            var sbPage = new StringBuilder(string.Empty);

            if (IsUp)
            {
                sbPage.Append(" <div class='pages right'>");
                sbPage.Append("     <ul>");
                sbPage.Append("         <li>");
                sbPage.Append($"             <span class='total aorou'>共{PagerOption.Total }条</span>");
                sbPage.Append("         </li>");
                sbPage.Append("         <li>");
                sbPage.Append($"             <span class='aorou'>{PagerOption.CurrentPage}/{totalPage}</span>");
                sbPage.Append("         </li>");
                sbPage.Append("         <li>");
                sbPage.Append(PagerOption.CurrentPage==1? "<span class='pre_ye'>上一页</span>":$" <a href='javascript:Pager({PagerOption.CurrentPage-1})'>上一页</a>");
                sbPage.Append("         </li>");
                sbPage.Append("         <li>");
                sbPage.Append(PagerOption.CurrentPage == totalPage ? "<span class='pre_ye'>下一页</span>" : $" <a href='javascript:Pager({PagerOption.CurrentPage + 1})'>下一页</a>");
                sbPage.Append("         </li>");
                sbPage.Append("     </ul>");
                sbPage.Append(" </div>");
            }
            else

            {
                #region 默认样式
                sbPage.Append("<div class='pages right'>");
                sbPage.Append(" <div class='m-pagination'>");
                sbPage.Append("     <div class='inner'>");
                sbPage.Append(PagerOption.CurrentPage == 1 ? "<span>上一页</span>" : $" <a class='btn prev'  href='javascript:Pager({PagerOption.CurrentPage - 1})'>上一页</a>");

                var i1 = PagerOption.CurrentPage / 5;
                if (PagerOption.CurrentPage % 5 == 0)
                {
                    i1 = i1 - 1;
                }

                if (PagerOption.CurrentPage > 5)
                {
                    sbPage.Append($" <span><a class='btn' href='javascript:Pager({(i1 -1) * 5 + 1})' title='前5页'>...</a></span>");
                }
               
                for (int i = 1; i < 6; i++)
                {
                    var i2 = i1 * 5 + i;
                    if (totalPage >= i2)
                    {
                        sbPage.Append(PagerOption.CurrentPage == i2
                            ? $"       <span class='active'>{i2}</span> "
                            : $"       <a class='btn' href='javascript:Pager({i2})' title='第{i2}页'>{i2}</a>");
                    }
                }

                if ((i1 + 1) * 5 + 1 <= totalPage)
                {
                    sbPage.Append($" <span><a class='btn' href='javascript:Pager({(i1 + 1) * 5 + 1})' title='后5页'>...</a></span>");
                }
               
                sbPage.Append(PagerOption.CurrentPage == totalPage? "<span class='prev btn-disabled gray'><span>下一页</span></span>" : $" <a class='btn next' href='javascript:Pager({PagerOption.CurrentPage + 1})'>下一页</a>");
                sbPage.Append($"       <span class='text'>共{totalPage}页</span> ");
                sbPage.Append("       <span class='text'>到第</span><input type = 'text' class='num' id='goindex' onkeydown='return goKeyDown(event)'><span class='text'>页</span>");
                sbPage.Append("       <a class='submit' href=''>GO</a>");
                sbPage.Append("      </div>");
                sbPage.Append("  </div>");
                sbPage.Append("</div>");

                #endregion
            }



            output.Content.SetHtmlContent(sbPage.ToString());
            //output.TagMode = TagMode.SelfClosing;
            //return base.ProcessAsync(context, output);
        }
    }
}
