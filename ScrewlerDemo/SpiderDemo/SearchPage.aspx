<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchPage.aspx.cs" Inherits="SpiderDemo.SearchPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    关键字：<asp:TextBox runat="server" ID="txtKeyword" ></asp:TextBox>
    <asp:Button runat="server" ID="btnSearch" Text="搜索" onclick="btnSearch_Click"/>
        &nbsp;&nbsp;
    <asp:Button runat="server" ID="btnStop" Text="停止" onclick="btnStop_Click" />
    
    </div>
    <div>
     
   <iframe width="800px" height="700px" src="ShowPage.aspx">
   
   </iframe>
   </div>

    </form>
</body>
</html>
