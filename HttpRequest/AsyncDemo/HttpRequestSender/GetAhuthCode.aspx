<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetAhuthCode.aspx.cs" Inherits="HttpRequestSender.GetAhuthCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Account <asp:TextBox runat="server" ID="txtAccount" ></asp:TextBox>
    <br />
    System<asp:TextBox runat="server" ID="txtSystem"  ></asp:TextBox>
    <br />
    Key &nbsp;<asp:TextBox runat="server" ID="txtKey" ></asp:TextBox>
    <br />
    Random &nbsp;<asp:TextBox runat="server" ID="txtRandom" ></asp:TextBox>
    <br />

    </div>

    <br />
    <br />
    <div>
    <asp:TextBox ID="txtRet" runat="server" TextMode="MultiLine" Width="300px" Height="200px" />
    </div>
    <br />

    <asp:Button runat="server" ID="btnGet" Text="Get" Width="100px" 
        onclick="btnGet_Click"/>
    </form>
</body>
</html>
