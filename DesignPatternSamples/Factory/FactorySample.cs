using System;

namespace DesignPyttern.Factory
{
    public interface IActionGame
    {
        void DoAction();
    }

    public class Kof : IActionGame
    {
        public Kof()
        {
            Console.WriteLine("KOF created");
        }

        public void DoAction()
        {
            Console.WriteLine("loading kof...");
        }
    }


    public class War3 : IActionGame
    {
        public War3()
        {
            Console.WriteLine("War3 created");
        }
        public void DoAction()
        {
            Console.WriteLine("loading war3...");
        }
    }


    public class Cs : IActionGame
    {
        public Cs()
        {
            Console.WriteLine("Cs created");
        }

        public void DoAction()
        {
            Console.WriteLine("loading cs...");
        }
    }


    public interface IRpg
    {
        void GetTask();
    }


    public class Menghuan : IRpg
    {
        public Menghuan()
        {
            Console.WriteLine("menghuan created");
        }
        public void GetTask()
        {
            Console.WriteLine("Get Task in menghuan...");
        }
    }


    public class Legend : IRpg
    {
        public Legend()
        {
            Console.WriteLine("Legend created");
        }
        public void GetTask()
        {
            Console.WriteLine("Get Task in Legend...");
        }
    }


    public class Diablo : IRpg
    {
        public Diablo()
        {
            Console.WriteLine("Diablo created");
        }
        public void GetTask()
        {
            Console.WriteLine("Get Task in Diablo...");
        }
    }


    public abstract class GameFactory
    {
        public abstract IActionGame CreateActionGame();
        public abstract IRpg CreateRpgGame();
    }

    public class MyGameFactory : GameFactory
    {
        public override IActionGame CreateActionGame()
        {
            return new Kof();
        }

        public override IRpg CreateRpgGame()
        {
            return new Legend();
        }
    }


}
