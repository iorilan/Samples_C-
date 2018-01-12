<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepeaterTest.aspx.cs" Inherits="WebCode.asp.net.Repeater.RepeaterTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater runat="server" ID="rptTest">
            <HeaderTemplate>
                <table border="1" width="60%">
                    <tr style="background-color: #1e90ff">
                        <th>
                            
                        </th>
                        <th>
                            Id
                        </th>
                        <th>
                            Name
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="background-color: #008000">
                    <td>
                        <asp:CheckBox runat="server" ID="chkRptItem" />
                    </td>
                    <td>
                       <asp:Label ID="lblRptItem_Id" runat="server" Text = <%#Eval("Id")%> ></asp:Label>
                    </td>
                    <td>
                       <asp:Label runat="server" ID="lblRptItem_Name" Text=<%#Eval("Name")%>></asp:Label> 
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr style="background-color: #00bfff">
                    <td>
                         <asp:CheckBox runat="server" ID="chkRptItem" />
                    </td>
                    <td>
                        <asp:Label ID="lblRptItem_Id" runat="server" Text = <%#Eval("Id")%> ></asp:Label>
                    </td>
                    <td>
                      <asp:Label runat="server" ID="lblRptItem_Name" Text=<%#Eval("Name")%>></asp:Label> 
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <SeparatorTemplate>
                <tr style="background-color: #008b8b">
                    <td></td>
                    <td>-----------
                    </td>
                    <td>-----------
                    </td>
                </tr>
            </SeparatorTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div id="rptPageArea">
            <span>Page Size: </span>&nbsp;&nbsp;
            <asp:DropDownList runat="server" ID="ddlPageSize">
                <asp:ListItem Text="5" Value="5" Selected="True"></asp:ListItem>
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
            </asp:DropDownList>
            <div>
                <asp:Label runat="server" ID="lblCurrentPageNo"></asp:Label>
                &nbsp; Of &nbsp;
                <asp:Label runat="server" ID="lblTotalPageNo"></asp:Label>
                &nbsp;&nbsp;
                <asp:Button runat="server" ID="btnPrevious" Text="Previous"></asp:Button>
                <asp:Button runat="server" ID="btnNext" Text="Next"></asp:Button>
            </div>
            <div>
                <span>Go To Page :</span> &nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtPageNo"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:Button runat="server" ID="btnGoToPageNo" Text="Go" />
            </div>
        </div>
        
        <div id="submitArea">
            <asp:Button runat="server" ID="btnSubmit" Text="Submit"/>
        </div>
    </div>
    </form>
</body>
</html>
