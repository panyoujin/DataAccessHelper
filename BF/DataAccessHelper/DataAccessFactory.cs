using BF.DataAccessHelper.Interface;
using BF.DataAccessHelper.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.DataAccessHelper
{
    public class DataAccessFactory
    {
        private static readonly IDBBase _dalBase = GetDALBase();

        private static IDBBase GetDALBase()
        {
            IDBBase dalBase = null;
            switch (SqlConfig.DBType)
            {
                case "MySql":
                    dalBase = MySqlDBBase.Instance;
                    break;
                default:
                    throw new Exception("暂不支持" + SqlConfig.DBType + "数据库");
                    break;
            }
            return dalBase;
        }
        private DataAccessFactory()
        {

        }
        public static IDBBase DALBase
        {
            get
            {
                return _dalBase;
            }
        }
    }
}
