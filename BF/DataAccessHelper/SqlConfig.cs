using DataAccessHelper.Helper;
using System.Configuration;

namespace DataAccessHelper
{
    public class SqlConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DBType
        {
            get
            {
                return ConfigHelper.GetConfigValue("DBType", "MySql");
                //return ConfigValue("DBType", "MySql");
            }
        }
        private static string _sqlConnStringName;
        public static string SqlConnStringName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sqlConnStringName))
                {
                    return ConfigHelper.GetConfigValue("ConnStringName", "BOSDbContext");
                }
                return _sqlConnStringName;
            }
            set
            {
                _sqlConnStringName = value;
            }
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[SqlConnStringName].ConnectionString;
            }
        }
    }
}
