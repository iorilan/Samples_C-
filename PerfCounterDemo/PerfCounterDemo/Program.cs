using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace PerfCounterDemo
{
    class Program
    {
        static void Main(string[] args)
        {

////注册性能计数器类别
            //if (!PerformanceCounterCategory.Exists("ConsolePerfCategory"))
            //{
            //    PerformanceCounterCategory.Create("ConsolePerfCategory",
            //        "My category description.",
            //        PerformanceCounterCategoryType.SingleInstance,
            //        "ConsolePerf",
            //        "console performace counter.");
            //}
            //else
            //{
            //    Console.WriteLine("ConsolePerfCategory already exists");
            //}

            ////注册完类别，就可以使用了
            //PerformanceCounter counter = new PerformanceCounter("ConsolePerfCategory", "ConsolePerf",false);
            


            ////等三秒，我还要打开性能监视器
            Thread.Sleep(3000);
            Console.WriteLine("start");
            double ret = 0;
            double increment = 0.0;
            for (; ; increment += 0.05)
            {
                ret = Math.Sin(increment) * 40 + 50;
                Console.WriteLine(ret);
                IDCSPerformanceCounter.Instance.HttpRequestProcessingTotal.SetRawValue((long)ret);
                //counter.RawValue = (long)ret;
                Thread.Sleep(200);
            }

            
           
        }
    }
}
