using System;

namespace DesignPyttern.Bridge
{
    public abstract class Game
    {
        protected Game(string name)
        {
            Name = name;
        }


        public Play HowToPlay { get; set; }
        public string Name { get; set; }


        public virtual void PlayForFun()
        {
            HowToPlay.PlayIt(Name);
        }
    }
    public class War3Game : Game
    {
        public War3Game(string name)
            : base(name)
        {

        }
        public override void PlayForFun()
        {
            Console.WriteLine("Loading war3...");
            base.PlayForFun();
        }
    }
    public class KofGame : Game
    {
        public KofGame(string name)
            : base(name)
        {

        }
        public override void PlayForFun()
        {
            Console.WriteLine("Loading  KOF... ");
            base.PlayForFun();
        }
    }
    public class CsGame : Game
    {
        public CsGame(string name)
            : base(name)
        {

        }
        public override void PlayForFun()
        {
            Console.WriteLine("Loading  CS... ");
            base.PlayForFun();
        }
    }


    public abstract class Play
    {
        public virtual void PlayIt(string gameName) { }
    }
    public class CpuPlay : Play
    {
        public override void PlayIt(string gameName)
        {
            Console.WriteLine("play" + gameName + " game on computer");
        }
    }
    public class PadPlay : Play
    {
        public override void PlayIt(string gameName)
        {
            Console.WriteLine("play" + gameName + " game on ipad");
        }
    }
    public class PhonePlay : Play
    {
        public override void PlayIt(string gameName)
        {
            Console.WriteLine("play " + gameName + " game on iphone");
        }
    }
    //usage 

    //Game g = new KofGame("kof97");
    //        g.m_play = new IPadPlay();
    //        g.PlayForFun();

}
