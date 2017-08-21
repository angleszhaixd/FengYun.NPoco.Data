<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DbFunctionTestForm.aspx.cs" Inherits="FengYun.NPoco.Data.Test.DbFunctionTestForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="test1" runat="server" OnClick="test1_Click" Text="测试查询" />
        <asp:Button ID="test2" runat="server" OnClick="test2_Click" Text="测试新增修改" />
        <asp:Button ID="text4" runat="server" OnClick="text4_Click" Text="测试删除" />
        <asp:Button ID="text3" runat="server" OnClick="text3_Click" Text="测试存储过程" />
    </div>
    <h1>
        测试结果:
    </h1>
        <textarea id="txtMessage" runat="server" style="width:680px;height:400px;color:red;font-size:120%;"></textarea>
    </form>
</body>
</html>
