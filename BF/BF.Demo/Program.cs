using DataAccessHelper;
using DataAccessHelper.SQLHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccessHelper.Extensions;

namespace BF.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, object> dic = new A() { LastTime = DateTime.Now.AddYears(-2) }.ToDictionary();
            new Thread(s => {
                    var dt = SQLHelperFactory.Instance.QueryForDataTable("Get_add_fyyh_Account", dic);
                    Console.WriteLine("颍淮:{0}", dt.Rows.Count);
            }).Start();

            new Thread(s => {
                    var dt = SQLHelperFactory.Instance.QueryForDataTable("Get_add_CiticZX_Account", dic);
                    Console.WriteLine("中信:{0}",dt.Rows.Count);
            }).Start();
            Console.Read();
        }
    }

    public class A
    {
        public string Key { get; set; }

        public DateTime LastTime { get; set; }
    }
}
