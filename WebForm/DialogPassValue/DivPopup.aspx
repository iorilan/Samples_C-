<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DivPopup.aspx.cs" Inherits="WebCode.asp.net.DialogPassValue.DivPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="btnOpenPopup" Text="Open" OnClick="btnOpenPopup_Click" />
        <!--Popup-->
        <div id="div_popup_background" runat="server" enableviewstate="True" style="position: absolute;
            left: 0; top: 0; z-index: 10; width: 100%; height: 150%; background-color: grey;
            opacity: 0.4">
        </div>
        <div id="div_purchase_confirmation" runat="server" enableviewstate="True" style="z-index: 1000;
                top: 30%; position: absolute; background-color: #ffffff; width: 80%; margin-left: 10%;">
                <div align="center">
                    Purchase Confirmation</div>
                <br />
                <div style="float: left">
                    <asp:Button Text="Back" ID="btnPurchaseConfirmationBack" Style="margin-right: 15px;
                        margin-bottom: 15px; width: 280px; background-repeat: no-repeat; background-size: 100% 100%"
                        OnClick="btnPurchaseConfirmationBack_Click" runat="server" />
                    <asp:Button Text="Credit Card/NETS Offline" ID="btnPurchaseConfirmationCreditCard"
                         Style="margin-right: 15px; margin-bottom: 15px; width: 280px;
                        background-repeat: no-repeat; background-size: 100% 100%" OnClick="btnPurchaseConfirmationCreditCard_Click"
                        runat="server" />
                    <asp:Button Text="Cash/KIV Receipt" ID="btnPurchaseConfirmationCaseKiv" 
                        Style="margin-right: 15px; margin-bottom: 15px; width: 280px; background-repeat: no-repeat;
                        background-size: 100% 100%" runat="server" OnClick="btnPurchaseConfirmationCaseKiv_Click" />
                    <asp:Button Text="Do not Submit to POS" ID="btnPurchaseConfirmationPos" 
                        Style="margin-right: 15px; margin-bottom: 15px; width: 280px; background-repeat: no-repeat;
                        background-size: 100% 100%" OnClick="btnPurchaseConfirmationPos_Click" runat="server" />
                    <br />
                </div>
            </div>
        <!--End Popup-->
    </div>
    </form>
</body>
</html>
