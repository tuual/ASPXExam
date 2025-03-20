using System;
using System.Web;

public partial class SiteMaster : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null && !Request.Url.AbsolutePath.Contains("Login.aspx"))
        {
            Response.Redirect("Login.aspx"); // Kullanıcı giriş yapmadıysa Login sayfasına yönlendir
        }

        // Eğer Login sayfasındaysa Navbar'ı gizle
        if (Request.Url.AbsolutePath.Contains("Login.aspx"))
        {
            Navbar.Visible = false; // Navbar için kullanılan ID'yi doğru yaz!
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
        Response.Redirect("~/Account/Login.aspx");
    }



}
