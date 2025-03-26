<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="Content" runat="server">
    <div class="login-container">
        <div class="login-box">
            <h2 class="text-center">Mobarch Portal Giriş Paneli</h2>

            <div class="form-group">
                <asp:Label ID="lblUsername" runat="server" AssociatedControlID="tbUserName" Text="Kullancı Adı: "></asp:Label>
                <asp:TextBox ID="tbUserName" runat="server" CssClass="form-control" Width="100%" />
            </div>

            <div class="form-group">
                <asp:Label ID="lblPassword" runat="server" AssociatedControlID="tbPassword" Text="Parola: "></asp:Label>
                <asp:TextBox ID="tbPassword" runat="server" CssClass="form-control" TextMode="Password" Width="100%" />
            </div>

            <div class="text-center mt-3">
                <asp:Button ID="btnLogin" runat="server" Text="Giriş Yap" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
            </div>

            <div class="text-center mt-2">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>

        </div>
    </div>

    <style>
        body {
            background-color: #f8f9fa;
        }

        .login-container {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .login-box {
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            width: 350px;
        }

        .btn-block {
            width: 100%;
        }
    </style>
</asp:Content>
