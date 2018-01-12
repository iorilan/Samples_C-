using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class ObjectCache : System.Web.UI.Page
    {
        private static int i = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList datestamps;
            if (Cache["datestamps"] == null)
            {
                i++;
                datestamps = new ArrayList();
                datestamps.Add(DateTime.Now);
                datestamps.Add(DateTime.Now);
                datestamps.Add(DateTime.Now);

                Cache.Add("datestamps", datestamps, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 5), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
                datestamps = (ArrayList)Cache["datestamps"];

            foreach (DateTime dt in datestamps)
                Response.Write(dt.ToString() + "<br />");

            Response.Write("\r\n" +i);
        }
    }
}