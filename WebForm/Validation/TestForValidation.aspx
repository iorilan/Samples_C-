<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestForValidation.aspx.cs"
    Inherits="WebCode.asp.net.Validation.TestForValidation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="uplPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div>
                    This Field is For Validation Summary
                </div>
                <asp:ValidationSummary runat="server" ID="validationSummary" HeaderText="Please correct the following errors in the form:" />
            </div>
            <div>
                <div>
                    This Field is For Required Validation
                </div>
                <asp:TextBox runat="server" ID="txtRequired"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="requireValidator" ControlToValidate="txtRequired"
                    Text="!" Display="Static" />
            </div>
            <div>
                <div>
                    This Field Is For Customer Validation</div>
            </div>
            <asp:TextBox runat="server" ID="txtCustomer"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="customerValidator" ControlToValidate="txtCustomer" Display="Static"
                ErrorMessage="length can not more than 5 charactors !" OnServerValidate="ServerValidation"
                runat="server" />
            <div>
                <div>
                    This Field is For Range Validation
                </div>
                <asp:TextBox ID="txtRange" runat="server" />
                <br />
                <asp:RangeValidator ID="rangeValidator" ControlToValidate="txtRange" MinimumValue="1"
                    MaximumValue="10" Type="Integer" EnableClientScript="false" Text="The value must be from 1 to 10!"
                    runat="server" />
            </div>
            <div>
                <div>
                    This Field is For Regular Expression Validation
                </div>
                <asp:TextBox runat="server" ID="txtRegular"></asp:TextBox>
                <br />
                <asp:RegularExpressionValidator ID="regularExpressionValidator" ControlToValidate="txtRegular"
                    ValidationExpression="\d{5}" Display="Static" ErrorMessage="must be 5 numeric digits"
                    EnableClientScript="False" runat="server" />
            </div>
            <div>
                <div>
                    This Field is For Compared Validation
                </div>
                <div>
                    Operator :
                    <asp:ListBox ID="lstOperator" OnSelectedIndexChanged="Operator_Index_Changed" runat="server" AutoPostBack="True">
                        <asp:ListItem Selected="True" Value="Equal">Equal</asp:ListItem>
                        <asp:ListItem Value="NotEqual">NotEqual</asp:ListItem>
                        <asp:ListItem Value="GreaterThan">GreaterThan</asp:ListItem>
                        <asp:ListItem Value="GreaterThanEqual">GreaterThanEqual</asp:ListItem>
                        <asp:ListItem Value="LessThan">LessThan</asp:ListItem>
                        <asp:ListItem Value="LessThanEqual">LessThanEqual</asp:ListItem>
                        <asp:ListItem Value="DataTypeCheck">DataTypeCheck</asp:ListItem>
                    </asp:ListBox>
                    &nbsp;&nbsp; DataType :
                    <asp:ListBox ID="lstDataType" OnSelectedIndexChanged="Type_Index_Changed"
                    AutoPostBack="True" runat="server">
                        <asp:ListItem Selected="true" Value="String">String</asp:ListItem>
                        <asp:ListItem Value="Integer">Integer</asp:ListItem>
                        <asp:ListItem Value="Double">Double</asp:ListItem>
                        <asp:ListItem Value="Date">Date</asp:ListItem>
                        <asp:ListItem Value="Currency">Currency</asp:ListItem>
                    </asp:ListBox>
                </div>
                <div>
                    <asp:TextBox ID="txtCompareText1" runat="server" />
                    &nbsp;Compare to &nbsp;
                    <asp:TextBox runat="server" ID="txtCompareText2"></asp:TextBox>
                </div>
                <asp:CompareValidator ID="compareValidator" ControlToValidate="txtCompareText1" ControlToCompare="txtCompareText2"
                    EnableClientScript="False" Type="String" runat="server" />

            </div>
            <div>
                <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" />
                <asp:Button runat="server" Text="Submit" ID="btnSubmit" OnClick="btnSubmit_Click" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
