using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessHelper.Models
{
    public class SqlAnalyModel
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBType { get; set; }
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        public string SqlConnStringName { get; set; }
        /// <summary>
        /// SQL
        /// </summary>
        public string SqlText { get; set; }
    }
}
