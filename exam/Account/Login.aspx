<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="Content" runat="server">
     
<div class="accountHeader">
    <h2>Log In</h2>
    <p>
        Please enter your username and password. <a href="Register.aspx">Register</a> if you don't have an account.
    </p>
</div>
<dx:BootstrapTextBox ID="tbUserName" runat="server" Width="200px" Caption="User Name">
    <CaptionSettings Position="Before" />
    <ValidationSettings ValidationGroup="LoginUserValidationGroup">
        <RequiredField ErrorText="User Name is required." IsRequired="true" />
    </ValidationSettings>
</dx:BootstrapTextBox>
<dx:BootstrapTextBox ID="tbPassword" runat="server" Password="true" Width="200px" Caption="Password">
    <CaptionSettings Position="Before" />
    <ValidationSettings ValidationGroup="LoginUserValidationGroup">
        <RequiredField ErrorText="Password is required." IsRequired="true" />
    </ValidationSettings>
</dx:BootstrapTextBox>
<br />
<dx:BootstrapButton ID="btnLogin" runat="server" Text="Log In" ValidationGroup="LoginUserValidationGroup"
    OnClick="btnLogin_Click">
</dx:BootstrapButton>
 
</asp:Content>