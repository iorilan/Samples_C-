<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParentPage.aspx.cs" Inherits="WebCode.asp.net.DialogPassValue.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../JsCSharp/jquery.1.9.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
    <input id="btnOpenNewPage" value="open New Page" type="button" />
    <input id="btnOpenDialogPage" value="Open Dialog" type="button" />

    <asp:TextBox runat="server" ID="txtValue"></asp:TextBox>

    </form>
    <script type="text/javascript">

        $("#btnOpenNewPage").click(function () {
            window.open("DialogPage.aspx");
        });
        
        $("#btnOpenDialogPage").click(function () {
            var returnValue = window.showModalDialog("DialogPage.aspx", "", "dialogWidth:670px;dialogHeight:600px;");
            $("#<%=txtValue.ClientID %>").val(returnValue.data);
            
        });

        function setReturnData(data) {
            $("#<%=txtValue.ClientID %>").val(data);
        }
       
    </script>
</body>
</html>
