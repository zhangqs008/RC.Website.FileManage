using System;
using System.Web;
using System.Web.Mvc;
using RC.FileManage;

namespace RC.Website.FileManage.Attribute.Authorize
{
    /// <summary>
    ///     后台管理身份认证
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        ///     身份验证判断逻辑
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return HttpContext.Current.Session["CurrentUser"].ToStr() != "";
        }

        /// <summary>
        ///     验证失败时触发
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");
            filterContext.Result = new RedirectResult(string.Concat("/Home/Login",
                "?ReturnUrl=", filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.ToString())));
        }
    }
}