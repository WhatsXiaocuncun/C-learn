using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 进程
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] pros = Process.GetProcesses();
            foreach (var item in pros)
            {
                Console.WriteLine(item);
            }
            Process.Start("calc");
            Process.Start("mspaint");
            Process.Start("iexplore","http://www.baidu.com");

            Console.ReadKey();
        }
    }
}
