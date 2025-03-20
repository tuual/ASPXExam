<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) {
        DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
        DevExpress.Security.Resources.AccessSettings.DataResources.SetRules(
            DevExpress.Security.Resources.DirectoryAccessRule.Allow(Server.MapPath("~/Content")),
            DevExpress.Security.Resources.UrlAccessRule.Allow()
        );
    }

    void Application_End(object sender, EventArgs e) {
        // Uygulama kapandığında çalışacak kod
    }

    void Application_Error(object sender, EventArgs e) {
        // Beklenmeyen bir hata oluştuğunda çalışacak kod
    }

    void Session_Start(object sender, EventArgs e) {
        // Yeni oturum başladığında kullanıcıyı temizle
        Session["UserID"] = null;
        Session["DynamicConnectionString"] = null;
    }

    void Session_End(object sender, EventArgs e) {
        // Oturum sona erdiğinde session'ı temizle
        Session.Clear();
        Session.Abandon();
    }
</script>
