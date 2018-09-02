using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace RC.FileManage
{
    public class AccountService
    {
        /// <summary>
        ///     取得接口调用应用
        /// </summary>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public static DataTable GetApiAccount(string appkey)
        {
            var sql = "SELECT * FROM RC_File_Account WHERE AppKey=@AppKey";
            var parms = new[]
            {
                new SqlParameter {Value = appkey, ParameterName = "AppKey"}
            };
            var dt = SqlHelper.Query(sql, parms);
            return dt;
        }


        /// <summary>
        ///     接口签名算法
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <param name="secret">由清山.NET市场信息部颁发</param>
        /// <returns></returns>
        public static string SignRequest(IDictionary<string, string> parameters, string secret)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams =
                new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            var dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            var query = new StringBuilder();

            while (dem.MoveNext())
            {
                var key = dem.Current.Key;
                var value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            query.Append(secret);
            var md5 = MD5.Create();
            bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));


            // 第四步：把二进制转化为大写的十六进制
            var result = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString("X2"));

            return result.ToString();
        }
    }
}