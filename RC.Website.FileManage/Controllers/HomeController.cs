using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;
using RC.FileManage;

namespace RC.Website.FileManage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/views/Home/Login.cshtml");
        }

        public ActionResult Login()
        {
            return View("~/views/Home/Login.cshtml");
        }

        public ActionResult LoginForm()
        {
            var result = new AjaxResult();
            try
            {
                var userName = RequestHelper.GetParm("UserName");
                var password = RequestHelper.GetParm("Password");

                if (userName.IsEmpty() || password.IsEmpty())
                {
                    result.IsSucess = false;
                    result.Body = "抱歉，用户名和密码不能为空";
                    return new ContentResult {Content = result.ToJson()};
                }

                var dt = GetUserInfo(userName);
                if (dt.Rows.Count == 0)
                {
                    result.IsSucess = false;
                    result.Body = "抱歉，用户不存在！";
                    return new ContentResult {Content = result.ToJson()};
                }

                if (password.ToLower().Trim() != dt.Rows[0]["psw"].ToString().ToLower().Trim())
                {
                    result.IsSucess = false;
                    result.Body = "抱歉，密码不正确，登录失败";
                    return new ContentResult {Content = result.ToJson()};
                }

                var retunUrl = RequestHelper.GetParm("ReturnUrl");
                retunUrl = retunUrl.IsEmpty() ? "/Admin/Index" : retunUrl;
                Session["CurrentUser"] = userName;
                result.IsSucess = true;
                result.Body = HttpUtility.UrlDecode(retunUrl);
                return new ContentResult {Content = result.ToJson()};
            }
            catch (Exception ex)
            {
                result.IsSucess = false;
                result.Body = ex.Message;
            }

            return new ContentResult {Content = result.ToJson()};
        }

        public ActionResult Logout()
        {
            Session["CurrentUser"] = "";
            return Redirect("~/Home/Login");
        }

        protected DataTable GetUserInfo(string username)
        {
            //可以修改为数据库读取验证
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("username"));
            dt.Columns.Add(new DataColumn("psw"));
            var row = dt.NewRow();
            row["username"] = ConfigurationManager.AppSettings["userName"].ToStr();
            row["psw"] = ConfigurationManager.AppSettings["password"].ToStr();
            dt.Rows.Add(row);
            return dt;
        }
    }
}