using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RC.FileManage
{
    public class SqlHelper
    {

        #region 数据库访问

        /// <summary>
        ///     连接字符串
        /// </summary>
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["sqlserver"].ConnectionString;
        public static DataTable Query(string conn, string sql, SqlParameter[] parms = null)
        {
            var table = new DataTable();
            using (var con = new SqlConnection(conn))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (parms != null)
                        foreach (var sqlParameter in parms)
                            cmd.Parameters.Add(sqlParameter);
                    using (var dr = cmd.ExecuteReader())
                    {
                        table.Load(dr);
                    }
                    cmd.Parameters.Clear();
                }
                con.Close();
            }
            return table;
        }
        public static DataTable Query(string sql, SqlParameter[] parms = null)
        {
            var table = new DataTable();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (parms != null)
                        foreach (var sqlParameter in parms)
                            cmd.Parameters.Add(sqlParameter);
                    using (var dr = cmd.ExecuteReader())
                    {
                        table.Load(dr);
                    }
                    cmd.Parameters.Clear();
                }
                con.Close();
            }
            return table;
        }
        public static int ExecuteNonQuery(string sql, SqlParameter[] parms = null)
        {
            int result;
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        if (parms != null)
                            foreach (var sqlParameter in parms)
                                cmd.Parameters.Add(sqlParameter);
                        result = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return result;
        }
        public static object ExecuteScalar(string sql, SqlParameter[] parms = null)
        {
            object result;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (parms != null)
                        foreach (var sqlParameter in parms)
                            cmd.Parameters.Add(sqlParameter);
                    result = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
                con.Close();
            }
            return result;
        }

        #endregion
    }
}