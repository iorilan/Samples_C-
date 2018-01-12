[IICPerformanceCounters("Namespace.SomeClass")]
		private class TransQueuePerfCounters
		{
			[IICPerformanceCounter("Enqueue /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
			public IICPerformanceCounter EnqueuePerSecond = null;

			[IICPerformanceCounter("Enqueue Total.", PerformanceCounterType.NumberOfItems64)]
			public IICPerformanceCounter EnqueueTotal = null;

			[IICPerformanceCounter("Dequeue /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
			public IICPerformanceCounter DequeuePerSecond = null;

			[IICPerformanceCounter("Dequeue Total.", PerformanceCounterType.NumberOfItems64)]
			public IICPerformanceCounter DequeueTotal = null;

			[IICPerformanceCounter("Dequeue Failed.", PerformanceCounterType.NumberOfItems64)]
			public IICPerformanceCounter DequeueFailed = null;

			[IICPerformanceCounter("Discard Total.", PerformanceCounterType.NumberOfItems32)]
			public IICPerformanceCounter DiscardTotal = null;

			[IICPerformanceCounter("Queue Length.", PerformanceCounterType.NumberOfItems64)]
			public IICPerformanceCounter QueueLength = null;
		}