﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PerfCounterDemo
{
    public class IICPerformanceCounterCategory
    {
        public IICPerformanceCounterCategory(string categoryName)
            : this(categoryName, PerformanceCounterCategoryType.MultiInstance, categoryName)
        {
        }

        public IICPerformanceCounterCategory(string categoryName, PerformanceCounterCategoryType categoryType)
            : this(categoryName, categoryType, categoryName)
        {
        }

        public IICPerformanceCounterCategory(string categoryName, PerformanceCounterCategoryType categoryType, string categoryHelp)
        {
            _categoryAttribute = new IICPerformanceCountersAttribute(categoryName, categoryType, categoryHelp);
        }

        public IICPerformanceCounter CreateCounter(string counterName, PerformanceCounterType counterType)
        {
            IICPerformanceCounter counter = new IICPerformanceCounter();
            counter._rawAttr = new IICPerformanceCounterAttribute(counterName, counterType);
            _counters.Add(counter);
            return counter;
        }

        internal IICPerformanceCountersAttribute _categoryAttribute;
        internal List<IICPerformanceCounter> _counters = new List<IICPerformanceCounter>();
    }
}
