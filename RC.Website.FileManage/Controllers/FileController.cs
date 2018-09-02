using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using RC.FileManage;

namespace RC.Website.FileManage.Controllers
{
    public class FileController : Controller
    {
        /// <summary>
        ///     预览文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Preview(string id)
        {
            if (string.IsNullOrEmpty(id)) return Content("抱歉，参数错误");

            var file = SqlHelper.Query("SELECT [fullpath],extension  FROM  [RC_File_FileUpload] WHERE guid=@guid",new[] {new SqlParameter {ParameterName = "guid", Value = id}});
            if (file.Rows.Count == 0) return Content("抱歉，文件记录不存在");

            var filepath = file.Rows[0]["fullpath"].ToStr();
            var extension = file.Rows[0]["extension"].ToStr();
            if (!System.IO.File.Exists(filepath)) return Content("抱歉，文件不存在");

            switch (extension)
            {
                #region 脚本文件

                case ".js":
                    return JavaScript(System.IO.File.ReadAllText(filepath));

                #endregion

                #region 文本文件

                case ".config":
                case ".cs":
                case ".css":
                case ".txt":
                case ".suo":
                case ".sln":
                case ".asmx":
                case ".aspx":
                    return File(filepath, "text/plain");

                #endregion

                #region 图片预览

                case ".gif":
                case ".jpg":
                case ".png":
                    return File(filepath, "image/jpeg");

                #endregion

                default:
                    return File(filepath, "text/plain", Path.GetFileName(filepath));
            }
        }
    }
}