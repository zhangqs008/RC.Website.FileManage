using System;
using System.Web.UI;

namespace RC.Website.FileManage
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/index");
        }
    }
}