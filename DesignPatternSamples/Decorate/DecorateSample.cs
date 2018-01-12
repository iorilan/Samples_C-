using System;

namespace DesignPyttern.Decorate
{
    public abstract class Wear
    {
        public abstract void Show();
    }

    public abstract class BaseWear : Wear
    {
        protected Wear Wear;
        protected BaseWear(Wear w)
        {
            Wear = w;
        }
        public override void Show()
        {
            if (null != Wear)
            {
                Wear.Show();
            }
        }
    }

    public class CreateWear : Wear
    {
        public override void Show()
        {
            Console.WriteLine("base wear");
        }
    }

    public class WearCloze : BaseWear
    {
        public WearCloze(Wear w)
            : base(w)
        {

        }

        public override void Show()
        {
            Wear.Show();
            Console.WriteLine("wear cloze");
        }
    }

    public class WearThous : BaseWear
    {
        public WearThous(Wear w)
            : base(w)
        {

        }
        public override void Show()
        {
            Wear.Show();
            Console.WriteLine("wear thurous");
        }
    }

    public class WearShoes : BaseWear
    {
        public WearShoes(Wear w)
            : base(w)
        {

        }
        public override void Show()
        {
            Wear.Show();
            Console.WriteLine("wear shoes");
        }
    }

    public class WearCap : BaseWear
    {
        public WearCap(Wear w)
            : base(w)
        {

        }
        public override void Show()
        {
            Wear.Show();
            Console.WriteLine("wear cap");
        }
    }

    //usage 

      //Wear w = new CreateWear();
      //     // w.Show();
      //      Wear w1 = new WearCap(w);
      //     // w1.Show();
      //      Wear w2 = new WearCloze(w1);
      //     // w2.Show();
      //      Wear w3 = new WearShoes(w2);
      //      w3.Show();
      //      Console.ReadLine();
}
