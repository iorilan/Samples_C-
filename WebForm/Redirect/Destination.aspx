<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Destination.aspx.cs" Inherits="WebCode.asp.net.Redirect.Destination" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Welcome ! <asp:Label runat="server" ID="lblName"></asp:Label><br/>
        Your ID: <asp:Label runat="server" ID="lblId"></asp:Label> <br />
<asp:Button runat="server" Text="Go Back" ID="btnGoBack" OnClick="btnGoBack_Click"/>
    </div>
    </form>
</body>
</html>
