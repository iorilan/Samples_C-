using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebCode.asp.net.Redirect
{
    public partial class ExploreRedirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            Response.Redirect("Destination.aspx");
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            Server.Transfer("Destination.aspx", chkPreserveForm.Checked);
        }
    }
}