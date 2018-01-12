using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebCode.asp.net.JsCSharp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //can be used when only need push some data to client script .
        protected override void OnPreInit(EventArgs e)
        {
            // add value in preinit 
            Page.Controls.Add(new LiteralControl("<input id=\"txtHidden\" style=\"display:none;\" value=\"abc\" \\>"));

            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}