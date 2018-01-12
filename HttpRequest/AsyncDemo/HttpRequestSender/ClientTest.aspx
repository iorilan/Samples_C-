<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientTest.aspx.cs" Inherits="HttpRequestSender.ClientTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="5" cellspacing="0" border="0">
            <tr>
                <td>接口地址：</td>
                <td><asp:TextBox ID="txtUrl" runat="server" Width="400px" Height="50px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td>ssic：</td>
                <td><asp:TextBox ID="txtSSIC" runat="server" Width="400px" Height="50px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td>doamin：</td>
                <td><asp:TextBox ID="txtDomain" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>请求XML：</td>
                <td><asp:TextBox ID="txtRequestXML" runat="server" Width="400px" Height="150px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td>返回结果：</td>
                <td><asp:TextBox ID="txtResultXML" runat="server" Width="400px" Height="150px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:Button ID="btnTest" runat="server" OnClick="btnTest_Click" Text="发送" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
