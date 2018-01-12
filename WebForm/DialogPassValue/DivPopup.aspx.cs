using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebCode.asp.net.DialogPassValue
{
    public partial class DivPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowPopup(false);
            }
        }

        protected void btnPurchaseConfirmationCreditCard_Click(object sender, EventArgs e)
        {
            ShowPopup(false);
        }

        protected void btnPurchaseConfirmationBack_Click(object sender, EventArgs e)
        {
            ShowPopup(false);
        }

        protected void btnPurchaseConfirmationCaseKiv_Click(object sender, EventArgs e)
        {
            ShowPopup(false);
        }

        protected void btnPurchaseConfirmationPos_Click(object sender, EventArgs e)
        {
            ShowPopup(false);
        }

        protected void btnOpenPopup_Click(object sender, EventArgs e)
        {
            ShowPopup(true);
        }

        private void ShowPopup(bool isVisible)
        {
            div_popup_background.Visible = isVisible;
            div_purchase_confirmation.Visible = isVisible;
        }

    }
}