using System;

namespace DesignPyttern.Responsibility
{
   public abstract class DataTypeHandler
    {
        protected DataTypeHandler Handler;
        public void SetHandler(DataTypeHandler handler)
        {
            Handler = handler;
        }


        public abstract void Handle(object data);


    }


    public class IntHandler : DataTypeHandler
    {


        public override void Handle(object data)
        {
            if (data is int)
            {
                Console.WriteLine("int handler handle it!");
            }
            else
            {
                if (Handler != null)
                {
                    Handler.Handle(data);
                }
            }
        }
    }


    public class BoolHandler : DataTypeHandler
    {
        public override void Handle(object data)
        {
            if (data is bool)
            {
                Console.WriteLine("BoolHandler handler handle it!");
            }
            else
            {
                if (Handler != null)
                {
                    Handler.Handle(data);
                }
            }
        }
    }


    public class DoubleHandler : DataTypeHandler
    {
        public override void Handle(object data)
        {
            if (data is double)
            {
                Console.WriteLine("double handler handle it!");
            }
            else
            {
                if (Handler != null)
                {
                    Handler.Handle(data);
                }
            }
        }
    }

    //usage 

    //DataTypeHandler bh1 = new IntHandler();
    //        DataTypeHandler bh2 = new BoolHandler();
    //        DataTypeHandler bh3 = new DoubleHandler();
    //        bh1.SetHandler(bh2);
    //        bh2.SetHandler(bh3);
    //        int a = 1;
    //        bool b = true;
    //        double c = 1.2;
    //        bh1.Handle(a);
    //        bh1.Handle(b);
    //        bh1.Handle(c);
}
