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
        if (Session["UserID"] == null)
        {
            string currentPage = Request.Url.AbsolutePath.ToLower();

            // **Eğer zaten Login sayfasındaysa tekrar yönlendirme yapma**
            if (!currentPage.Contains("login.aspx"))
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        // **Eğer Login sayfasındaysa Navbar'ı gizle**
        if (Request.Url.AbsolutePath.Contains("Login.aspx"))
        {
            if (Navbar != null)
            {
                Navbar.Visible = false;
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
        string connectionString = "Server=BLTTUAL;Database=Kullanicilar;User Id=biltekbilisim;Password=Bilisim20037816;";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = isAdmin
                ? "SELECT DISTINCT ReportName FROM dbo.ReportPermissions"
                : "SELECT ReportName FROM dbo.ReportPermissions WHERE UserID = @UserID AND CanView = 1";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (!isAdmin)
                {
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    string reportsHtml = "";
                    while (reader.Read())
                    {
                        string reportName = reader["ReportName"].ToString();
                        string reportUrl = "";

                        // **Hangi rapor hangi sayfaya gidecek?**
                        if (reportName == "Müşteri Hareketleri")
                            reportUrl = "Gridview.aspx";
                        else if (reportName == "Fatura Raporu")
                            reportUrl = "FaturaGrid.aspx";

                        // **Eğer link boşsa, hata mesajı ekleyelim**
                        if (string.IsNullOrEmpty(reportUrl))
                        {
                            Debug.WriteLine("UYARI: " + reportName + " için bir URL atanmadı!");
                        }

                        reportsHtml += "<li class='list-group-item'><a href='" + reportUrl + "'>" + reportName + "</a></li>";
                    }

                    Debug.WriteLine("LoadUserReports: Oluşturulan HTML => " + reportsHtml);

                    ltReports.Text = reportsHtml;
                }
            }
        }
    }


    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/Account/Login.aspx");
    }
}
