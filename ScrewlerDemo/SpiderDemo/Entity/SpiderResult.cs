using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderDemo.Entity
{

    public class Link
    {
        public string Href { get; set; }
        public string LinkName { get; set; }
        public string Context { get; set; }

        public int TheadId { get; set; }
    }
}