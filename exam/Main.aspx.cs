using System;
using System.Web.UI;

public partial class Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}
