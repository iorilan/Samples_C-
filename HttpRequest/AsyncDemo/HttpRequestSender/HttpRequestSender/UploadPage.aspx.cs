using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;

namespace HttpRequestSender
{
    public partial class UploadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strReturn = postFile(txtServerUrl.Text, txtServerUrl.Text.Trim(), txtMobileNo.Text.Trim());
            txtReturn.Text = strReturn;
        }

        public string postFile(string strUrl, string strFileName, string struploadCode)
        {
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(strUrl);
            //webrequest.CookieContainer = cookies;
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";
            webrequest.Headers.Add(getAhthCode(struploadCode));

            // Build up the post message header    
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n");
            sb.Append("\r\n----");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"uploadCode\"\r\n\r\n");
            sb.Append(struploadCode);
            sb.Append("\r\n----" + boundary + "\r\n");
            sb.Append("Content-Disposition: form-data; name=\"redirectURL\"\r\n\r\n");
            sb.Append(strUrl);
            sb.Append("\r\n----" + boundary + "\r\n");
            sb.Append("Content-Disposition: form-data; name=\"f1\";filename=\"\r\n");
            sb.Append(strFileName.Substring(0, strFileName.LastIndexOf("\\")) + "\"\r\n");
            sb.Append("Content-Type:application/octet-stream\r\n\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array    
            // ensuring the boundary appears on a line by itself    
            byte[] boundaryBytes =
                   Encoding.ASCII.GetBytes("--" + boundary + "");

            FileStream fileStream = new FileStream(strFileName,
                                        FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length +
                                                   boundaryBytes.Length;
            webrequest.ContentLength = length;
            Stream requestStream = webrequest.GetRequestStream();

            // Write out our post header    
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // Write out the file contents    
            byte[] buffer = new Byte[checked((uint)Math.Min(4096,
                                     (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // Write out the trailing boundary    
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            return sr.ReadToEnd();

        }

        private string GetRandomStr()
        {
            string random = ((int)(DateTime.Now - DateTime.Parse("1970-1-1 00:00:00")).TotalSeconds).ToString();
            return random;
        }

        private string getAhthCode(string strAccount)
        {
            string strKey = "326892";
            string strSystem = "Fetion";
            string strRandom = GetRandomStr();
            string account = Convert.ToBase64String(Encoding.Default.GetBytes(strAccount));
            string code = strSystem + strRandom + strKey;


            string md5Str = FormsAuthentication.HashPasswordForStoringInConfigFile(code, "MD5");

            StringBuilder header = new StringBuilder(100);
            header.Append("space accessname=\"");
            header.Append(strSystem);
            header.Append("\",");
            header.Append("random=\"");
            header.Append(strRandom);
            header.Append("\",");
            header.Append("code=\"");
            header.Append(md5Str);
            header.Append("\",");
            header.Append("account=\"");
            header.Append(account + "\"");

            string ret = header.ToString();

            return ret;
        }
    }
}