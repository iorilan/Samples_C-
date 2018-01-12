using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace HttpRequestDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPost_Click(object sender, EventArgs e)
        {

            while (true)
            {
                Thread.Sleep(1000);
                string url = "http://localhost:8083/demo/1.aspx";

                StringBuilder reqStr = new StringBuilder(100);
                reqStr.Append("?p1=1&p2=2");
                //reqStr.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                //reqStr.Append("<request>");
                //reqStr.Append("<head><reqtype>" +txtReqType.Text +"</reqtype></head>");
                //reqStr.Append("<body>");
                //reqStr.Append("<mobiles>");
                //reqStr.Append("<mobile>" + mobileNo +"</mobile>");
                //reqStr.Append("</mobiles>");
                //reqStr.Append("</body>");
                //reqStr.Append("</request>");

                string postData = reqStr.ToString();

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postData);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

                myRequest.Method = "Post";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                Stream newStream = myRequest.GetRequestStream();


                newStream.Write(data, 0, data.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                string content = reader.ReadToEnd();
                txtResult.Text = content;
            }

        }
    }
}