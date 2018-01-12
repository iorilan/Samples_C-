<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JSCSharp.aspx.cs" Inherits="WebCode.asp.net.JsCSharp.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="jquery.1.9.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    
    <script type="text/javascript">
        $(document).ready(function (e) {

            //retrieve value here
            alert($("#txtHidden").val());
            //
        });

    </script>
</body>
</html>
