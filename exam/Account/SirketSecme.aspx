<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SirketSecme.aspx.cs" Inherits="Account_SirketSecme" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="Content" runat="server">
    <div class="login-container">
        <div class="login-box">
            <h3 class="text-center">Çalışacağınız Şirketi Seçin</h3>

            <div class="form-group">
                <asp:Label ID="lblUsername" runat="server" Text="Şirket: "></asp:Label>
                <dx:BootstrapComboBox ID="cbSirketSec" runat="server" ></dx:BootstrapComboBox>
            </div>

         

            <div class="text-center mt-3">
                <asp:Button ID="btnLogin" runat="server" Text="Şirket Seç" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
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
