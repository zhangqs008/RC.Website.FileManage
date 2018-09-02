using System;
using System.Collections.Generic;

namespace RC.FileManage
{
    /// <summary>
    /// 分页数据请求参数封装类
    /// </summary>
    public class PageRequest
    {
        private int _currentPage = 1;
        /// <summary>
        ///     当前页码
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage <= 0 ? 1 : _currentPage; }
            set { _currentPage = value; }
        }

        /// <summary>
        ///     页数
        /// </summary>
        public int PageSize { get; set; }

        Dictionary<string, string> _conditions = new Dictionary<string, string>();

        /// <summary>
        ///     查询参数
        /// </summary>
        public Dictionary<string, string> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }

        /// <summary>
        ///     排序
        /// </summary>
        public Dictionary<string, string> Sorts { get; set; }

        /// <summary>
        ///     分页开始
        /// </summary>
        public int Begin
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        /// <summary>
        ///     分页截止
        /// </summary>
        public int End
        {
            get { return CurrentPage * PageSize; }
        }
        
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}