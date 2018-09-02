using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using RC.FileManage;
using RC.Website.FileManage.Attribute.Authorize;

namespace RC.Website.FileManage.Controllers
{
    [AdminAuthorize]
    public class AccountController : Controller
    {
        /// <summary>
        ///     编辑页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AccountEdit(int id = 0)
        {
            var dt = new DataTable();
            if (id > 0) dt = SqlHelper.Query("SELECT * FROM [RC_File_Account] WHERE id=" + id);
            ViewBag.Data = dt;
            return View("~/views/admin/AccountEdit.cshtml");
        }

        /// <summary>
        ///     列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountList()
        {
            return View("~/views/admin/AccountList.cshtml");
        }

        /// <summary>
        ///     分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="searchType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult AccountPageData(int page = 1, int rows = 20, string searchType = "", string keyword = "")
        {
            var conditions = new Dictionary<string, string>();
            if (searchType.IsNotEmpty() && keyword.IsNotEmpty()) conditions.Add(searchType, keyword);
            var request = new PageRequest
            {
                CurrentPage = page,
                PageSize = rows,
                Conditions = conditions,
                Sorts = new Dictionary<string, string> { { "CreateDate", "DESC" } }
            };
            var orderBy = new List<string>();
            foreach (var pair in request.Sorts) orderBy.Add(pair.Key + " " + pair.Value);

            var dataSql =
                string.Format(
                    "SELECT * FROM (SELECT *,ROW_NUMBER() OVER(ORDER BY {0}) AS RowNum FROM RC_File_Account WHERE 1=1 ",
                    string.Join(",", orderBy.ToArray()));
            const string countSql = "SELECT COUNT(Id) FROM RC_File_Account WHERE 1=1 ";
            var parms = new List<SqlParameter>();
            var whereSql = "";
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

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AccountDelete(int id)
        {
            var result = new AjaxResult();
            try
            {
                var flag = SqlHelper.ExecuteNonQuery("DELETE FROM RC_File_Account WHERE Id=" + id);
                result.IsSucess = flag > 0;
                result.Body = "";
            }
            catch (Exception ex)
            {
                result.IsSucess = false;
                result.Body = ex.Message;
            }

            return new ContentResult { Content = result.ToJson() };
        }

        /// <summary>
        ///     清空
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountClear()
        {
            var result = new AjaxResult();
            try
            {
                var tableName = "RC_File_Account";
                SqlHelper.ExecuteNonQuery("TRUNCATE TABLE " + tableName);
                SqlHelper.ExecuteNonQuery("DBCC CHECKIDENT ('" + tableName + "', RESEED, 1) ");

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

        /// <summary>
        ///     编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountForm()
        {
            var result = new AjaxResult();
            var id = RequestHelper.GetParm("Id");
            var gid = RequestHelper.GetParm("GId");
            var name = RequestHelper.GetParm("Name");
            var appKey = RequestHelper.GetParm("AppKey");
            var appSecret = RequestHelper.GetParm("AppSecret");
            var isEnable = RequestHelper.GetParm("IsEnable");
            var isLog = RequestHelper.GetParm("IsLog");
            var maxSize = RequestHelper.GetParm("MaxSize");
            var extension = RequestHelper.GetParm("Extension");
            var savePath = RequestHelper.GetParm("SavePath");

            if (name.IsEmpty())
            {
                result.IsSucess = false;
                result.Body = "抱歉，应用名称不能为空";
                return new ContentResult { Content = result.ToJson() };
            }

            if (appKey.IsEmpty())
            {
                result.IsSucess = false;
                result.Body = "抱歉，appKey不能为空";
                return new ContentResult { Content = result.ToJson() };
            }

            if (appSecret.IsEmpty())
            {
                result.IsSucess = false;
                result.Body = "抱歉，appSecret不能为空";
                return new ContentResult { Content = result.ToJson() };
            }

            if (savePath.IsEmpty())
            {
                result.IsSucess = false;
                result.Body = "抱歉，存放路径不能为空";
                return new ContentResult { Content = result.ToJson() };
            }

            if (extension.IsEmpty())
            {
                result.IsSucess = false;
                result.Body = "抱歉，允许上传文件后缀不能为空";
                return new ContentResult { Content = result.ToJson() };
            }

            if (maxSize.IsEmpty())
            {
                result.IsSucess = false;
                result.Body = "抱歉，允许上传文件大小不能为空";
                return new ContentResult { Content = result.ToJson() };
            }

            if (id.ToInt() == 0)
            {
                var sql =
                    @"INSERT  INTO [RC_File_Account] ( [Name], [AppKey], [AppSecret], [Count],[IsEnable], [IsLog], [IsDel], [Sort],[CreateDate], [CreateUser], [MaxSize],[Extension],[SavePath] )
                                VALUES  ( @Name, @AppKey, @AppSecret, 0, 1, 1, 0, 0, GETDATE(), 'system',@MaxSize,@Extension,@SavePath )";
                var parms = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "Name", Value = name},
                    new SqlParameter {ParameterName = "AppKey", Value = appKey},
                    new SqlParameter {ParameterName = "AppSecret", Value = appSecret},
                    new SqlParameter {ParameterName = "MaxSize", Value = maxSize},
                    new SqlParameter {ParameterName = "Extension", Value = extension},
                    new SqlParameter {ParameterName = "SavePath", Value = savePath}
                };

                SqlHelper.ExecuteNonQuery(sql, parms.ToArray());
                result.IsSucess = true;
                result.Body = "";
            }
            else
            {
                if (gid.IsEmpty())
                {
                    result.IsSucess = false;
                    result.Body = "抱歉，GId不能为空";
                    return new ContentResult { Content = result.ToJson() };
                }

                var sql = @"UPDATE  [RC_File_Account]
                                SET  [Name] = @Name, [AppKey] = @AppKey, [AppSecret] = @AppSecret, [IsEnable] = @IsEnable, [IsLog] = @IsLog,[MaxSize] = @MaxSize,[Extension] = @Extension,[SavePath] = @SavePath
                                WHERE   [GID] = @GID";
                var parms = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "GID", Value = gid},
                    new SqlParameter {ParameterName = "Name", Value = name},
                    new SqlParameter {ParameterName = "AppKey", Value = appKey},
                    new SqlParameter {ParameterName = "AppSecret", Value = appSecret},
                    new SqlParameter {ParameterName = "IsEnable", Value = isEnable.ToBoolean()},
                    new SqlParameter {ParameterName = "IsLog", Value = isLog.ToBoolean()},
                    new SqlParameter {ParameterName = "MaxSize", Value = maxSize},
                    new SqlParameter {ParameterName = "Extension", Value = extension},
                    new SqlParameter {ParameterName = "SavePath", Value = savePath}
                };

                if (SqlHelper.ExecuteNonQuery(sql, parms.ToArray()) > 0)
                {
                    result.IsSucess = true;
                    result.Body = "";
                }
                else
                {
                    result.IsSucess = false;
                    result.Body = "更新失败";
                }
            }

            if (savePath.IsNotEmpty())
            {
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

                savePath = savePath.Replace("\\\\", "\\");

                var exsit = SqlHelper
                    .ExecuteScalar(
                        "SELECT COUNT(*) FROM RC_File_FileUpload WHERE fullpath=@fullpath AND type='dir' AND isroot=1",
                        new[] { new SqlParameter { ParameterName = "fullpath", Value = savePath } }).ToInt();
                if (exsit == 0)
                {
                    var sql =
                        @"INSERT  INTO [RC_File_FileUpload]( [guid] ,[type] ,[name] ,[fullpath] ,[parentpath] ,[extension] ,[length] ,[creationtime] ,[level] ,[isroot] )
VALUES  ( @guid ,@type ,@name ,@fullpath ,@parentpath ,@extension ,@length ,@creationtime ,@level ,@isroot  )";

                    var dirInfo = new DirectoryInfo(savePath);
                    var parms = new List<SqlParameter>
                    {
                        new SqlParameter {ParameterName = "guid", Value = Guid.NewGuid()},
                        new SqlParameter {ParameterName = "type", Value = "dir"},
                        new SqlParameter {ParameterName = "name", Value = dirInfo.Name},
                        new SqlParameter {ParameterName = "fullpath", Value = dirInfo.FullName},
                        new SqlParameter {ParameterName = "parentpath", Value = dirInfo.Parent.FullName},
                        new SqlParameter {ParameterName = "extension", Value = ""},
                        new SqlParameter {ParameterName = "length", Value = ""},
                        new SqlParameter {ParameterName = "creationtime", Value = dirInfo.CreationTime},
                        new SqlParameter {ParameterName = "level", Value = Regex.Matches(savePath, @"\\").Count},
                        new SqlParameter {ParameterName = "isroot", Value = true} 
                    };
                    SqlHelper.ExecuteNonQuery(sql, parms.ToArray());
                }
            }

            return new ContentResult { Content = result.ToJson() };
        }

        public ActionResult GetGuid()
        {
            var result = new AjaxResult();
            result.IsSucess = true;
            result.Body = Guid.NewGuid().ToString("N");
            return new ContentResult { Content = result.ToJson() };
        }
    }
}