<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KullaniciEkle.aspx.cs" Inherits="KullaniciEkle" %>

<!DOCTYPE html>
<html lang="tr">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Yeni Kullanıcı Ekle</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet">
    <link href="~/Content/Site.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h2 class="text-center">Yeni Kullanıcı Ekle</h2>

            <div class="alert alert-success d-none" id="successMessage" runat="server"></div>
            <div class="alert alert-danger d-none" id="errorMessage" runat="server"></div>

            <div class="card p-4 shadow-sm mt-3">
                <div class="mb-3">
                    <label for="txtUsername" class="form-label">Kullanıcı Adı:</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Şifre:</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="ddlIsAdmin" class="form-label">Yetki:</label>
                    <asp:DropDownList ID="ddlIsAdmin" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlIsAdmin_SelectedIndexChanged">
                        <asp:ListItem Text="Normal Kullanıcı" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Admin" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- Rapor Yetkileri Seçimi -->
                <div class="mb-3">
                    <label class="form-label">Rapor Yetkileri:</label>
                    <asp:CheckBoxList ID="cblReports" runat="server" CssClass="form-check">
                    </asp:CheckBoxList>
                </div>

                <div class="text-center">
                    <asp:Button ID="btnSaveUser" runat="server" Text="Kullanıcı Ekle" CssClass="btn btn-primary" OnClick="btnSaveUser_Click" />
                    <asp:Button ID="btnBack" runat="server" Text="Geri Dön" CssClass="btn btn-secondary"
    OnClick="btnBack_Click" CausesValidation="false" UseSubmitBehavior="false" />



                </div>
            </div>
        </div>
    </form>
</body>
</html>
