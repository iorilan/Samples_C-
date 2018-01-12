<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs"  ValidateRequest="false"
Inherits="HttpRequestSender.MainPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    请求地址
    <asp:TextBox runat="server" ID="txtAddress" Width="600px" />
    <br />
    <br />
    Authorization
     <br /><br /><asp:TextBox runat="server" ID="txtAuth" TextMode="MultiLine" Width="500px" Height="100px"></asp:TextBox>
    <br />
    <br />
    
    请求内容
    <br />
    <br />
    <asp:TextBox runat="server" ID="txtContent" TextMode ="MultiLine" Width="500px" Height="200px" />
    <br /><br />
    请求方式
    <asp:RadioButtonList ID="rbtnSendType" runat="server">
       <asp:ListItem Text="同步" Value="1"></asp:ListItem>
       <asp:ListItem Text="异步" Value="2"></asp:ListItem>
    </asp:RadioButtonList>
    <asp:Button runat="server" ID="btnSend" Text="发送" onclick="btnSend_Click"/>
    </div>
    <br />
    <div>
    返回结果
    <br />
    <asp:TextBox runat="server" ID="txtResult" TextMode="MultiLine" Width="500px" Height="200px" />
    </div>
    </form>
</body>
</html>
