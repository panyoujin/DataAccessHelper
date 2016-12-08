using BF.DataAccessHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ApplyUserID"] = "9f2ec079-7d0f-4fe2-90ab-8b09a8302aba";
            var dt = DataAccessFactory.DALBase.QueryForDataTable("GetMyFlowApply", dic);
            Console.WriteLine(dt.Rows.Count);
            Console.Read();
        }
    }
}
