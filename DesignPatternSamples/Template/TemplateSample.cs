using System;

namespace DesignPyttern.Template
{
    public abstract class BaseHandler
    {
        public virtual void Handle()
        {
            OperationA();
            Console.WriteLine("common part");
            OperationB();
        }
        protected virtual void OperationA()
        {
            Console.WriteLine("operationA in base handler");
        }
        protected virtual void OperationB()
        {
            Console.WriteLine("operationB in base handler");
        }
    }
    public class SubClass : BaseHandler
    {
        protected override void OperationA()
        {
            Console.WriteLine("implementation operationA in sub class");
        }


        protected override void OperationB()
        {
            Console.WriteLine("implementation operationB in sub class");
        }
    }

    // usage 
    //BaseHandler bh = new SubClass();
    //           bh.Handle();
}
