using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PerfCounterDemo
{
    [IICPerformanceCounters("Imps:IDCS PerfCounter", CategoryType = PerformanceCounterCategoryType.MultiInstance)]
    public class IDCSPerformanceCounter
    {
        private static IDCSPerformanceCounter _instance = IICPerformanceCounterFactory.GetCounters<IDCSPerformanceCounter>();

        // 单例模式
        public static IDCSPerformanceCounter Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 当前正在处理的、尚未返回响应的HttpRequest请求的总量。
        /// </summary>
        [IICPerformanceCounter("HttpRequest Processing.", PerformanceCounterType.NumberOfItems32)]
        public IICPerformanceCounter HttpRequestProcessingTotal = null;

        /// <summary>
        /// 每秒钟平均收到的HttpRequest数量。
        /// </summary>
        [IICPerformanceCounter("HttpRequest Received per sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public IICPerformanceCounter HttpRequestReceivedPerSec = null;
    }
}
