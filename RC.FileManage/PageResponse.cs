using System.Data;

namespace RC.FileManage
{
    /// <summary>
    ///     分页数据返回结果封装类
    /// </summary>
    public class PageResponse
    {
        /// <summary>
        ///     分页数据
        /// </summary>
        public DataTable Data { get; set; }

        /// <summary>
        ///     总数据量
        /// </summary>
        public long Total { get; set; }
    }

    public static class PageResponseExstion
    {
        /// <summary>
        /// 将分页数据转成ezUI DataGrid需要的json格式
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string ToJson(this PageResponse response)
        {
            long total = response.Total;
            string data = response.Data.ToJson();
            return string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        }
    }
}