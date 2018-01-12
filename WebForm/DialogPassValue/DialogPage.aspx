<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DialogPage.aspx.cs" Inherits="WebCode.asp.net.Test2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../JsCSharp/jquery.1.9.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        
    <div>
    <input id="btnPassBy_WindowReturnValue" type="button" value="PassBy_Window_ReturnValue" />
    <input id="btnPassBy_OpenerFunction" type="button" value="PassBy_Window_OpenerFunction"/>
    </div>
    </form>
    
    <script type="text/javascript">

        $("#btnPassBy_OpenerFunction").click(function () {
            window.opener.setReturnData("data from opener_set_data");
        });

        $("#btnPassBy_WindowReturnValue").click(function () {
            var vReturnValue = new Object();
            vReturnValue.data = "data from window_return_value";
            window.returnValue = vReturnValue;
            window.close(); //only if close this window then return data .
        });
        
    </script>
      

</body>
</html>
