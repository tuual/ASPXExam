using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using DevExpress.Xpo.DB;
using DevExpress.Web.ASPxRichEdit.Forms;
using System.Web.Routing;
using System.IO;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null && Session["ServerName"] == null && Session["SecilenSirket"] == null)
        {
            string path = Request.Url.AbsolutePath.ToLower();

            // 🟡 Navbar'ı sadece belirli sayfalarda gizle (örneğin: iframe modallar)
            if (path.Contains("sirketdegistirme.aspx") || path.Contains("sirketsecme.aspx") || path.Contains("login.aspx"))
            {
                if (Navbar != null)
                    Navbar.Visible = false;

                if (sidebar != null) // Sidebar'a da runat="server" verildiğini varsayıyorum
                    sidebar.Visible = false;
            }

            // 🔵 Login kontrolü ve admin butonları
            if (Session["UserID"] == null && Session["ServerName"] == null && Session["SecilenSirket"] == null)
            {
                if (!path.Contains("login.aspx"))
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
            }

            if (Session["UserID"] != null)
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                bool isAdmin = Session["IsAdmin"] != null && Convert.ToBoolean(Session["IsAdmin"]);

                lnkUserAdd.Visible = isAdmin;
            }
            if (path.Contains("sirketdegistirme.aspx"))
            {
                if (Navbar != null)
                {
                    Navbar.Visible = false;
                }
            }

            if (!IsPostBack)
            {
                LoadUserReports();

                // Şirket adını yazdır (Literal için Text kullanılabilir)
                string sirketAdi = Session["SirketAdi"] != null ? Session["SirketAdi"].ToString() : "Şirket Seçilmedi";
                selectedCompanyName.Text = sirketAdi;
            }

        }


        // **Eğer Kullanıcı Adminse, Kullanıcı Ekle Butonunu Göster**
        if (Session["UserID"] != null)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            bool isAdmin = Session["IsAdmin"] != null && Convert.ToBoolean(Session["IsAdmin"]);
                    
            if (isAdmin == false)
            {
                lnkUserAdd.Visible = false;
            }
            else
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
        // Eğer Login sayfasındaysa Navbar'ı gizle
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

        // ✅ BURASI EKLENECEK KISIM
        if (Request.Url.AbsolutePath.Contains("Account/SirketDegistirme.aspx"))
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

    protected void lnkUserAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("KullaniciEkle.aspx");
    }

    protected void btnSwitchCompany_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Account/SirketSecme.aspx");

    }
}
