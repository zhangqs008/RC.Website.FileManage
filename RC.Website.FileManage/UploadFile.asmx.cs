using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Services;
using RC.FileManage;
using WebGrease.Css;

namespace RC.Website.FileManage
{
    /// <summary>
    ///     UploadFile 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class UploadFile : WebService
    {
        /// <summary>
        ///     上传文件到远程服务器
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="fileBytes"></param>
        /// <param name="fileName"></param>
        /// <param name="appkey"></param>
        /// <param name="timestamp"></param>
        /// <param name="tablename"></param>
        /// <param name="tablekeyid"></param>
        /// <returns></returns>
        [WebMethod(Description = "上传文件")]
        public string Upload(string appkey, string timestamp, string sign, byte[] fileBytes, string fileName = "", string tablename = "", string tablekeyid = "")
        {
            var response = new ApiResponse();
            try
            {
                #region 1.验证参数是否为空

                if (string.IsNullOrEmpty(appkey) ||
                    string.IsNullOrEmpty(timestamp) ||
                    string.IsNullOrEmpty(sign))
                {
                    response.IsSuccess = false;
                    response.Body = "抱歉,appkey,method,sign不能为空";
                    return response.ToJson();
                }

                #endregion

                #region 2.验证appkey是否有效

                var dt = AccountService.GetApiAccount(appkey);
                if (dt.Rows.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Body = "抱歉,appkey无效";
                    return response.ToJson();
                }

                if (!dt.Rows[0]["IsEnable"].ToBoolean())
                {
                    response.IsSuccess = false;
                    response.Body = "抱歉,应用已被禁用";
                    return response.ToJson();
                }

                #endregion

                #region 3.验证接口签名是否有效

                var appsecret = dt.Rows[0]["AppSecret"].ToString();
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("appkey", appkey);
                parameters.Add("timestamp", timestamp);
                var vsign = AccountService.SignRequest(parameters, appsecret);

                if (sign.ToLower() != vsign.ToLower())
                {
                    response.IsSuccess = false;
                    response.Body = "抱歉,接口签名错误";
                    return response.ToJson();
                }

                #endregion

                #region 4.文件大小限制

                var filesize = dt.Rows[0]["MaxSize"].ToLong() * 1024;
                if (fileBytes.Length > filesize)
                {
                    response.IsSuccess = false;
                    response.Body = "抱歉,上传文件大小不能超过：" + FileHelper.GetFileSize(filesize);
                    return response.ToJson();
                }

                #endregion

                #region 5.文件类型限制

                var extension = dt.Rows[0]["Extension"].ToStr().ToLower()
                    .Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                var list = new List<string>();
                list.AddRange(extension);
                if (Path.GetExtension(fileName) != null)
                    if (!list.Contains(Path.GetExtension(fileName).ToLower()))
                    {
                        response.IsSuccess = false;
                        response.Body = "抱歉,仅能上传后缀为：" + dt.Rows[0]["Extension"].ToStr().Replace("|", ",") + " 的文件";
                        return response.ToJson();
                    }

                #endregion

                #region 6.文件保存到硬盘

                var saveDir = dt.Rows[0]["SavePath"].ToString();

                #region 初始化存放目录

                saveDir = string.IsNullOrEmpty(saveDir) ? Server.MapPath(".") + "\\Uploadfiles" : saveDir;
                saveDir = saveDir.CombinePath(DateTime.Now.ToString("yyyy-MM"));
                if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
                var guid = SqlHelper.ExecuteScalar(
                    "SELECT TOP 1 [guid] FROM [RC_File_FileUpload] WHERE fullpath=@fullpath",
                    new[] { new SqlParameter { ParameterName = "fullpath", Value = saveDir } });
                if (guid.IsEmpty())
                {
                    var sql =
                        @"INSERT INTO [RC_File_FileUpload] ( [guid], [type], [name], [fullpath],[parentpath], [extension],[length], [creationtime],[level],[isroot])
                              VALUES(@guid, @type, @name, @fullpath,@parentpath, @extension, @length, @creationtime,@level,0);";
                    var i = 0;
                    var parms = new SqlParameter[9];
                    parms[i] = new SqlParameter { ParameterName = "guid", Value = Guid.NewGuid().ToString("N") };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "type", Value = "dir" };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "name", Value = new DirectoryInfo(saveDir).Name };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "fullpath", Value = saveDir };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "extension", Value = "" };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "length", Value = 0 };
                    i++;
                    parms[i] = new SqlParameter
                    {
                        ParameterName = "creationtime",
                        Value = new DirectoryInfo(saveDir).CreationTime
                    };
                    i++;
                    var parent = new DirectoryInfo(saveDir).Parent;
                    if (parent != null)
                        parms[i] = new SqlParameter
                        {
                            ParameterName = "parentpath",
                            Value = parent.FullName
                        };
                    i++;
                    parms[i] = new SqlParameter
                    {
                        ParameterName = "level",
                        Value = Regex.Matches(new DirectoryInfo(saveDir).FullName, @"\\").Count
                    };
                    SqlHelper.ExecuteNonQuery(sql, parms);
                }

                #endregion

                string name;
                var exstion = fileName.IsEmpty() ? ".jpg" : Path.GetExtension(fileName);
                //如果传了数据表和表主键，则用表名+主键命名
                if (tablename.IsNotEmpty() && tablekeyid.IsNotEmpty())
                {
                    name = tablename + "_" + tablekeyid + exstion;
                }
                //如果传了文件名，则用上传者的名字
                else if (fileName.IsNotEmpty())
                {
                    name = fileName;
                }
                //如果都没传，则自动命名
                else
                {
                    name = DateTime.Now.Ticks + exstion;
                }
                fileName = name;
                var filePath = Path.Combine(saveDir, fileName);
                try
                {
                    if (File.Exists(filePath)) File.Delete(filePath);
                }
                catch (Exception ex)
                {
                }

                using (var stream = new MemoryStream(fileBytes))
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    stream.WriteTo(fileStream);
                    stream.Close();
                    fileStream.Close();
                }

                #endregion

                #region 7.写入数据库

                guid = SqlHelper.ExecuteScalar("SELECT TOP 1 [guid] FROM [RC_File_FileUpload] WHERE fullpath=@fullpath",
                    new[] { new SqlParameter { ParameterName = "fullpath", Value = filePath } });
                if (guid.IsEmpty())
                {
                    var sql =
                        @"INSERT INTO [RC_File_FileUpload] ( [guid], [type], [name], [fullpath],[parentpath], [extension],[length], [creationtime],[level],[isroot],[tablename],[tablekeyid])
                              VALUES(@guid, @type, @name, @fullpath,@parentpath, @extension, @length, @creationtime,@level,0,@tablename,@tablekeyid);";
                    var i = 0;
                    var parms = new SqlParameter[11];
                    parms[i] = new SqlParameter { ParameterName = "guid", Value = Guid.NewGuid().ToString("N") };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "type", Value = "file" };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "name", Value = new FileInfo(filePath).Name };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "fullpath", Value = filePath };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "extension", Value = Path.GetExtension(filePath) };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "length", Value = new FileInfo(filePath).Length };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "tablename", Value = tablename };
                    i++;
                    parms[i] = new SqlParameter { ParameterName = "tablekeyid", Value = tablekeyid };
                    i++;
                    parms[i] = new SqlParameter
                    {
                        ParameterName = "creationtime",
                        Value = new FileInfo(filePath).CreationTime.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    i++;
                    parms[i] = new SqlParameter
                    {
                        ParameterName = "parentpath",
                        Value = new FileInfo(filePath).Directory.FullName
                    };
                    i++;
                    parms[i] = new SqlParameter
                    {
                        ParameterName = "level",
                        Value = Regex.Matches(new FileInfo(filePath).FullName, @"\\").Count
                    };
                    SqlHelper.ExecuteNonQuery(sql, parms);
                }

                #endregion

                #region 8.返回文件GUID

                response.IsSuccess = true;
                response.Body =
                    SqlHelper.ExecuteScalar("SELECT TOP 1 [guid] FROM [RC_File_FileUpload] WHERE fullpath=@fullpath",
                        new[] { new SqlParameter { ParameterName = "fullpath", Value = filePath } });

                #endregion

                #region 9.记录调用次数

                SqlHelper.ExecuteNonQuery("UPDATE RC_File_Account SET Count=Count+1 WHERE AppKey=@AppKey",
                    new[] { new SqlParameter { ParameterName = "AppKey", Value = appkey } });

                #endregion

                return response.ToJson();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}