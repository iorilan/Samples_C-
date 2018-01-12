using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPyttern.Observe
{
    /// <summary>
    /// Observer sample 
    /// </summary>
    /// <param name="sender"></param>

    public delegate void UpdatePaper(object sender);


    /// <summary>
    /// paper shop
    /// </summary>
    public class Paper
    {
        public event UpdatePaper OnChange;


        /// <summary>
        /// publish paper 
        /// </summary>
        public void ChangeData()
        {
            if (null != OnChange)
            {
                OnChange.Invoke("new data");
            }
        }
    }


    /// <summary>
    /// reader 
    /// </summary>
    public class Reader
    {
        public void ReadNews(object news)
        {
            Console.Write(news.ToString());
        }
    }

    //usage : 

    //Paper p = new Paper();
    //        Reader reader1 = new Reader();
    //        Reader reader2 = new Reader();


    //        p.OnChange += reader1.ReadNews;
    //        p.OnChange += reader2.ReadNews;


    //        p.ChangeData();
    //        Console.ReadLine();
}
