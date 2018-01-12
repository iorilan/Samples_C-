using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebCode.asp.net.Redirect
{
    public partial class Destination : System.Web.UI.Page
    {
        protected string PreviousUrl
        {
            get { return "ExploreRedirect.aspx"; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            {
                var txt = PreviousPage.FindControl("txtName") as TextBox;
                if(null != txt)
                lblName.Text = txt.Text;
                return;
            }



            if(IsPostBack) return;
            
            
            if(PreviousPage == null)return;
            var txtName = PreviousPage.FindControl("txtName") as TextBox;
            if (null != txtName)
            {
                lblName.Text = txtName.Text;
            }
            lblId.Text = PreviousPage.Request.QueryString["SN"];
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
                Server.Transfer(PreviousUrl,false);
        }
    }
}