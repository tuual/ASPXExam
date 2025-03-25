using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null && Session["ServerName"] == null && Session["SecilenSirket"] == null)
        {
            string currentPage = Request.Url.AbsolutePath.ToLower();

            // **Eğer zaten Login sayfasındaysa tekrar yönlendirme yapma**
            if (!currentPage.Contains("login.aspx"))
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }


        // **Eğer Kullanıcı Adminse, Kullanıcı Ekle Butonunu Göster**
        if (Session["UserID"] != null)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            bool isAdmin = Session["IsAdmin"] != null && Convert.ToBoolean(Session["IsAdmin"]);

            if (isAdmin && lnkUserAdd != null)
            {
                lnkUserAdd.Visible = true;
            }
        }
        // **Kullanıcının Yetkili Olduğu Raporları Getir**
        if (!IsPostBack)
        {
            LoadUserReports();
        }
        navBarGizleme();
    }


    private void navBarGizleme()
    {
        // **Eğer Login sayfasındaysa Navbar'ı gizle**
        if (Request.Url.AbsolutePath.Contains("Login.aspx"))

            if (Session["UserID"] == null && !Request.Url.AbsolutePath.Contains("Account/Login.aspx"))
            {
                Response.Redirect("Account/Login.aspx"); // Kullanıcı giriş yapmadıysa Login sayfasına yönlendir
            }
        if (Request.Url.AbsolutePath.Contains("SirketSecme.aspx"))

            if (Session["ServerName"] == null && Session["SecilenSirket"] == null)
            {
                Response.Redirect("Account/SirketSecme.aspx");
            }

        if (Request.Url.AbsolutePath.Contains("Account/SirketSecme.aspx"))
        {
            if (Navbar != null)
            {
                Navbar.Visible = false;
            }
        }


        // Eğer Login sayfasındaysa Navbar'ı gizle
        if (Request.Url.AbsolutePath.Contains("Account/Login.aspx"))
        {
            if (Navbar != null)
            {
                Navbar.Visible = false;
            }
        }

      
    }

    private void LoadUserReports()
    {
        if (Session["UserID"] == null)
        {
            ltReports.Text = "<li class='list-group-item text-danger'>Oturum Açın!</li>";
            Debug.WriteLine("LoadUserReports: Kullanıcı oturum açmamış!");
            return;
        }

        int userId = Convert.ToInt32(Session["UserID"]);
        bool isAdmin = Session["IsAdmin"] != null && Convert.ToBoolean(Session["IsAdmin"]);

        UseReportLoader reportLoader = new UseReportLoader();
        string reportsHtml = reportLoader.LoadUserReports(userId, isAdmin);

        ltReports.Text = reportsHtml;
    }


    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/Account/Login.aspx");
    }
}
