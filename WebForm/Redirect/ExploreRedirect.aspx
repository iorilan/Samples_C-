<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExploreRedirect.aspx.cs" Inherits="WebCode.asp.net.Redirect.ExploreRedirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Name: <asp:TextBox runat="server" ID="txtName" ></asp:TextBox>
        </div>
        <br />
    <div>
        
    Redirect : <asp:Button runat="server" ID="btnRedirect" Text="Redirect" OnClick="btnRedirect_Click"/>
    </div>
    <br/>
    <div>    
        Transfer : <asp:Button runat="server" ID="btnTransfer" Text="Transfer" OnClick="btnTransfer_Click"/>
        <asp:CheckBox runat="server" ID="chkPreserveForm" Text="Preserve Form ?"/>
    </div>
    <div>
        CrossPage Post Back<asp:Button runat="server" ID="btnCrossPagePostBack"
         Text="CrossPagePostBack" PostBackUrl="Destination.aspx"/>
    </div>
    </form>
</body>
</html>
