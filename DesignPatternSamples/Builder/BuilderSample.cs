using System;

namespace DesignPyttern.Builder
{
    public interface IBuilder
    {
        void ShowTop();
        void ShowMiddle();
        void ShowBottom();
    }


    /// <summary>
    /// Print *
    /// </summary>
    public class StarBuilder : IBuilder
    {
        public void ShowTop()
        {
            Console.WriteLine("*");
        }


        public void ShowMiddle()
        {
            Console.WriteLine("**");
        }


        public void ShowBottom()
        {
            Console.WriteLine("***");
        }
    }


    /// <summary>
    /// Print +
    /// </summary>
    public class AddBuilder : IBuilder
    {
        public void ShowTop()
        {
            Console.WriteLine("+");
        }


        public void ShowMiddle()
        {
            Console.WriteLine("++");
        }


        public void ShowBottom()
        {
            Console.WriteLine("+++");
        }
    }


    /// <summary>
    /// Print -
    /// </summary>
    public class SubBuilder : IBuilder
    {
        public void ShowTop()
        {
            Console.WriteLine("-");
        }


        public void ShowMiddle()
        {
            Console.WriteLine("--");
        }


        public void ShowBottom()
        {
            Console.WriteLine("---");
        }
    }


    public class TopDirector
    {
        private readonly IBuilder _builder;
        public TopDirector(IBuilder builder)
        {
            _builder = builder;
        }
        public void Show()
        {
            _builder.ShowTop();
            _builder.ShowMiddle();
            _builder.ShowBottom();
        }
    }


    public class MiddleDirector
    {
        private readonly IBuilder _builder;
        public MiddleDirector(IBuilder builder)
        {
            _builder = builder;
        }
        public void Show()
        {
            _builder.ShowMiddle();
            _builder.ShowTop();
            _builder.ShowBottom();
        }
    }


    public class BottomDirector
    {
        private readonly IBuilder _builder;
        public BottomDirector(IBuilder builder)
        {
            _builder = builder;
        }
        public void Show()
        {
            _builder.ShowBottom();
            _builder.ShowTop();
            _builder.ShowMiddle();
        }
    }

    // usage :

      //IBuilder builder = new StarBuilder();
      //      TopDirector td = new TopDirector(builder);
      //      td.Show();


      //      builder = new AddBuilder();
      //      MiddleDirector md = new MiddleDirector(builder);
      //      md.Show();
            
      //      builder = new SubBuilder();
      //      BottomDirector bd = new BottomDirector(builder);
      //      bd.Show();
}
