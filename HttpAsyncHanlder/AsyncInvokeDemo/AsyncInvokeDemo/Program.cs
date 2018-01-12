using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncInvokeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<A> test = (a) =>
            {
                Console.WriteLine("start to invoke");
                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine(i);
                }

                Console.WriteLine("invoke args aint : {0},astr: {1} ", a.aInt, a.aStr);
            };

            AsyncInvokeProxy<A> proxy = new AsyncInvokeProxy<A>(test);

            proxy.BeginEnvoke<B>(new A { aInt = 1, aStr = "astr" }, (b, ex) =>
            {
                if (ex != null)
                {

                }

                Console.WriteLine("callback ret bint： {0},bstr： {1}", b.bInt, b.bStr);

            }, new B { bInt = 2, bStr = "bstr" });

            Console.ReadLine();
        }
    }

}
