<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowPage.aspx.cs" Inherits="SpiderDemo.ShowPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.6.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
    </div>
    <div id="divRet">
        
    </div>
    <script type="text/javascript">

        $(document).ready(
        function () {

            var timer = setInterval(
        function () {

            $.ajax({
                type: "POST",
                url: "http://localhost:26820/StateServicePage.ashx",
                data: "op=info",
                success: function (msg) {
                
                    $("#divRet").html(msg);
                }
            });
        }, 2000);


        });
    </script>
    </form>
</body>
</html>
