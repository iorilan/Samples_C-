using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;

namespace HttpRequestSender
{
    public partial class GetAhuthCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                Init();
            }
        }

        private void Init() {
            txtAccount.Text = "13632799971";
            string key = "326892";
            txtKey.Text = key;
            string SysName = "Fetion";
            txtSystem.Text = SysName;

            GetRandomStr();

        }

        private void PutAhthCode()
        {
           
            string account = Convert.ToBase64String(Encoding.Default.GetBytes(txtAccount.Text));
            string code = txtSystem.Text + txtRandom.Text + txtKey.Text;


            string md5Str = FormsAuthentication.HashPasswordForStoringInConfigFile(code, "MD5");

            StringBuilder header = new StringBuilder(100);
            header.Append("space accessname=\"");
            header.Append(txtSystem.Text);
            header.Append("\",");
            header.Append("random=\"");
            header.Append(txtRandom.Text);
            header.Append("\",");
            header.Append("code=\"");
            header.Append(md5Str);
            header.Append("\",");
            header.Append("account=\"");
            header.Append(account + "\"");

            string ret = header.ToString();

            txtRet.Text = ret;
        }

        
        protected void btnGet_Click(object sender, EventArgs e)
        {
            PutAhthCode();

            GetRandomStr();
        }

        private void GetRandomStr()
        {
            string random = ((int)(DateTime.Now - DateTime.Parse("1970-1-1 00:00:00")).TotalSeconds).ToString();
            txtRandom.Text = random;
        }


    }
}