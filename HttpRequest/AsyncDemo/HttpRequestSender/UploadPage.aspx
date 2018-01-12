<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadPage.aspx.cs" Inherits="HttpRequestSender.UploadPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    redirect url
    <asp:TextBox ID="txtServerUrl" runat="server" Width="400px" Height="100px" TextMode="MultiLine"></asp:TextBox><br />
    文件路径<asp:TextBox runat="server" ID="txtPath" Width="200px" Text="D:/temp/testData.txt"></asp:TextBox>
    <asp:Button runat="server" ID="btnUpload" Text="上传" onclick="btnUpload_Click" />
    <br />
    手机号码<asp:TextBox ID="txtMobileNo" runat="server"></asp:TextBox><br />
    <asp:TextBox ID="txtReturn" runat="server" Width="400px" Height="200px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
