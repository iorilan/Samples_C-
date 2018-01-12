<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutputCacheWithP.aspx.cs" Inherits="WebApplication2.OutputCacheWithP" %>
<%@ OutputCache duration="10" varybyparam="p" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <%= DateTime.Now.ToString() %><br />
        <a href="?p=1">1</a><br />
        <a href="?p=2">2</a><br />
        <a href="?p=3">3</a><br />
    </div>
    </form>
</body>
</html>
