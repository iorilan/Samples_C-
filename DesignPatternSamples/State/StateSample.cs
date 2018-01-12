using System;

namespace DesignPyttern.State
{
    public interface ITime
    {
        void SayHello();
    }


    public abstract class BaseTime : ITime
    {
        public int Time { get; set; }
        public abstract void SayHello();

        protected BaseTime(int t)
        {
            Time = t;
        }


    }


    public class Morning : BaseTime
    {
        public Morning(int t) : base(t) { }


        public override void SayHello()
        {
            if (Time >= 7 && Time < 12)
            {
                Console.WriteLine("good morning,sir" + " time is {0}", Time);
            }
            else
            {
                new Noon(Time).SayHello();
            }
        }
    }
    public class Noon : BaseTime
    {
        public Noon(int t) : base(t) { }


        public override void SayHello()
        {
            if (Time >= 12 && Time <= 13)
            {
                Console.WriteLine("have a nice lunch ,sir" + " time is {0}", Time);
            }
            else
            {
                new AfterNoon(Time).SayHello();
            }
        }
    }


    public class AfterNoon : BaseTime
    {
        public AfterNoon(int t) : base(t) { }


        public override void SayHello()
        {
            if (Time > 13 && Time <= 18)
            {
                Console.WriteLine("good afternoon,sir" + " time is {0}", Time);
            }
            else
            {
                new Night(Time).SayHello();
            }
        }
    }


    public class Night : BaseTime
    {
        public Night(int t) : base(t) { }


        public override void SayHello()
        {
            if (Time > 18 && Time <= 24)
            {
                Console.WriteLine("good evening,sir" + " time is {0}", Time);
            }
            else
            {
                new DeepNight(Time).SayHello();
            }
        }
    }


    public class DeepNight : BaseTime
    {
        public DeepNight(int t) : base(t) { }


        public override void SayHello()
        {
            if (Time >= 0 && Time < 7)
            {
                Console.WriteLine("have a good sleep,sir" + " time is {0}", Time);
            }
            else
            {
                Console.WriteLine("oh,unexpected time {0},", Time);
            }
        }
    }

    //BaseTime bt = new Morning(1);
    //        bt.SayHello();
    //        bt.Time = 8;
    //        bt.SayHello();
    //        bt.Time = 12;
    //        bt.SayHello();
    //        bt.Time = 14;
    //        bt.SayHello();
    //        bt.Time = 21;
    //        bt.SayHello();

}
