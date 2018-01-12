using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
namespace SpiderDemo.Entity
{
    public class ClamThread
    {
        public Thread _thread { get; set; }
        public List<Link> lnkPool { get; set; }
    }
}