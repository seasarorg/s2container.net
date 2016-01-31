<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="Quill.Sample.MainForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="frmMain" runat="server">
        郵便番号検索：<asp:TextBox ID="txtPostalCode" runat="server" MaxLength="7" Width="60px"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnSearch_Click" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1">
            <Columns>
                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select" TypeName="Quill.Sample.SampleDataSource"></asp:ObjectDataSource>
        <table style="width:300px"><tr>
        <td><asp:Button ID="btnUpdateSuccess" runat="server" Text="Update(Success)" Width="140px" OnClick="btnUpdateSuccess_Click" /></td>
        <td>&nbsp;<asp:Button ID="btnUpdateFailure" runat="server" Text="Update(Failure)" Width="140px" OnClick="btnUpdateFailure_Click" /></td>
        </tr></table>
    </form>
</body>
</html>
