using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using RC.FileManage;
using RC.Website.FileManage.Attribute.Authorize;

namespace RC.Website.FileManage.Controllers
{
    [AdminAuthorize]
    public class FileManageController : Controller
    {
        public ActionResult GetRoot()
        {
            var dt = SqlHelper.Query(@"SELECT  guid,name,fullpath
            FROM   [RC_File_FileUpload]
            WHERE   isroot = 1 Order By creationtime DESC");
            var list = new List<string>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                list.Add("{\"id\":\"" + row["guid"] + "\",\"text\":\"" + row["name"] +"\",\"children\":[],\"state\":\"closed\"}");
            } 
            return new ContentResult
            {
                Content = "[{\"id\":1,\"text\":\"所有目录\",\"children\":[" + string.Join(",", list.ToArray()) + "]}]"
            };
        }

        public ActionResult GetChildren()
        {
            var id = RequestHelper.GetParm("id");
            var dt = SqlHelper.Query(
                @" SELECT guid, name,(SELECT COUNT(*) FROM RC_File_FileUpload WHERE parentpath=u.fullpath)cnt
                                                     FROM   RC_File_FileUpload u
                                                     WHERE  parentpath IN ( SELECT  fullpath FROM    RC_File_FileUpload WHERE   guid = @guid )
                                                     AND Type='dir'
                                                     ORDER BY creationtime DESC",
                new[] { new SqlParameter { ParameterName = "guid", Value = id } });
            var list = new List<string>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                if (row["cnt"].ToInt() > 0)
                    list.Add("{\"id\":\"" + row["guid"] + "\",\"text\":\"" + row["name"] +
                             "\",\"children\":[],\"state\":\"closed\",\"iconCls\":\"tree-folder\"}");
                else
                    list.Add("{\"id\":\"" + row["guid"] + "\",\"text\":\"" + row["name"] +
                             "\",\"children\":[],\"state\":\"open\",\"iconCls\":\"tree-folder\"}");
            }

            return new ContentResult { Content = "[" + string.Join(",", list.ToArray()) + "]" };
        }

        public ActionResult GetFile(string guid, int page = 1, int rows = 20, string searchType = "", string keyword = "")
        {
            var conditions = new Dictionary<string, string>();
            if (searchType.IsNotEmpty() && keyword.IsNotEmpty()) conditions.Add(searchType, keyword);
            var request = new PageRequest
            {
                CurrentPage = page,
                PageSize = rows,
                Conditions = conditions,
                Sorts = new Dictionary<string, string> { { "creationtime", "DESC" } }
            };
            var orderBy = new List<string>();
            foreach (var pair in request.Sorts) orderBy.Add(pair.Key + " " + pair.Value);

            var dataSql = string.Format(
                @"SELECT * FROM (SELECT *,ROW_NUMBER() OVER(ORDER BY {0}) AS RowNum FROM RC_File_FileUpload WHERE parentpath IN ( SELECT  fullpath
            FROM    RC_File_FileUpload
            WHERE   guid = @guid)  AND type='file' ", string.Join(",", orderBy.ToArray()));

            const string countSql = @"SELECT COUNT(*) FROM RC_File_FileUpload WHERE  parentpath IN ( SELECT  fullpath
            FROM    RC_File_FileUpload
            WHERE   guid = @guid) AND type='file' ";
            var parms = new List<SqlParameter>();
            var whereSql = "";
            parms.Add(new SqlParameter { ParameterName = "guid", Value = guid });

            if (request.Conditions.Count > 0)
                foreach (var condition in request.Conditions)
                    switch (condition.Key)
                    {
                        default:
                            if (!condition.Value.IsEmpty())
                            {
                                whereSql += string.Format(" AND {0} like '%'+@{0}+'%' ", condition.Key);
                                parms.Add(new SqlParameter { ParameterName = condition.Key, Value = condition.Value });
                            }

                            break;
                    }
            dataSql += string.Format("{0})temp WHERE RowNum BETWEEN {1} AND {2}  ORDER BY temp.RowNum ASC ", whereSql,
                request.Begin, request.End);

            var response = new PageResponse
            {
                Data = SqlHelper.Query(dataSql, parms.ToArray()),
                Total = SqlHelper.ExecuteScalar(countSql + whereSql, parms.ToArray()).ToInt()
            };
            return new ContentResult { Content = response.ToJson() };
        }

        public ActionResult DelFile(string id)
        {
            var result = new AjaxResult();
            try
            {
                SqlHelper.ExecuteNonQuery("DELETE FROM [RC_File_FileUpload] WHERE Guid=@Guid", new[] { new SqlParameter { ParameterName = "Guid", Value = id } });
                result.IsSucess = true;
                result.Body = "";
            }
            catch (Exception ex)
            {
                result.IsSucess = false;
                result.Body = ex.Message;
            }

            return new ContentResult { Content = result.ToJson() };
        }
    }
}