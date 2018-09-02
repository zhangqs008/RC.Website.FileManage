using System.Text.RegularExpressions;
using System.Web.Mvc;
using RC.Website.FileManage.Attribute.Authorize;

namespace RC.Website.FileManage.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileManage()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View("~/views/admin/AccountList.cshtml");
        }

        public ActionResult Document()
        {
            var url = "";
            if (ControllerContext.RequestContext.HttpContext.Request.Url != null)
            {
                url = ControllerContext.RequestContext.HttpContext.Request.Url.ToString();
                url = new Regex(
                    "(?i)http://([\\s\\S]*?)/",
                    RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                ).Match(url).Value;
            }

            ViewBag.Url = url;
            return View();
        }
    }
}