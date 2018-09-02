//--------------------------------------------------------------------------------
// 文件描述：请求辅助类
// 文件作者：全体开发人员
// 创建日期：2016-06-27
//--------------------------------------------------------------------------------

using System.Web;

namespace RC.FileManage
{
    /// <summary>
    ///     请求辅助类
    /// </summary>
    public class RequestHelper
    {
        #region  取得页面参数,兼容Get和Post两种方式

        /// <summary>
        ///     取得页面参数,兼容Get和Post两种方式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetParm(string name)
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }
            for (var i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                if (HttpContext.Current.Request.Form.Keys[i] != null)
                {
                    if (HttpContext.Current.Request.Form.Keys[i].ToLower() == name.ToLower())
                    {
                        return HttpContext.Current.Request.Form[i];
                    }
                }
            }
            for (var i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                if (HttpContext.Current.Request.QueryString.Keys[i] != null)
                {
                    if (HttpContext.Current.Request.QueryString.Keys[i].ToLower() == name.ToLower())
                    {
                        return HttpContext.Current.Request.QueryString[i];
                    }
                }
            }
            return string.Empty;
        }

        #endregion
    }
}