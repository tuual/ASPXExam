using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KullaniciEkle : System.Web.UI.Page
{
    private string dbLogin = ConfigurationService.dbLogin;
    private string dbPassword = ConfigurationService.dbPassword;
    protected void Page_Load(object sender, EventArgs e)
    {
      

        // Sadece adminler erişebilir
        if (Session["UserID"] == null || Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
        {
            Response.Redirect("~/Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            LoadReports(); // Mevcut raporları yükle
        }
    }

    private void LoadReports()
    {
        string connectionString = "Server=BLTTUAL;Database=Kullanicilar;User Id=" + dbLogin + ";Password=" + dbPassword + ";";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT DISTINCT ReportName FROM ReportPermissions";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string reportName = reader["ReportName"].ToString();
                        cblReports.Items.Add(new ListItem(reportName, reportName));
                    }
                }
            }
        }
    }

    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;
        bool isAdmin = ddlIsAdmin.SelectedValue == "1";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            errorMessage.InnerText = "Tüm alanları doldurunuz!";
            errorMessage.Attributes["class"] = "alert alert-danger d-block";
            return;
        }

        // **Admin'in bağlı olduğu DatabaseName ve ServerName bilgisini al**
        string adminServerName = Session["ServerName"] as string;
        string adminDatabaseName = Session["DatabaseName"] as string;

        if (string.IsNullOrEmpty(adminServerName) || string.IsNullOrEmpty(adminDatabaseName))
        {
            errorMessage.InnerText = "Admin'in bağlı olduğu veritabanı bilgileri eksik!";
            errorMessage.Attributes["class"] = "alert alert-danger d-block";
            return;
        }

        string hashedPassword = HashPassword(password);
        string connectionString = "Server=BLTTUAL;Database=Kullanicilar;User Id=biltekbilisim;Password=Bilisim20037816;";

        int newUserId;
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // **Kullanıcıyı ekle ve ID'sini al**
                string query = "INSERT INTO dbo.Users (Username, PasswordHash,DbLogin,DbPassword, IsAdmin, ServerName, DatabaseName) " +
                               "OUTPUT INSERTED.ID VALUES (@Username, @PasswordHash,@dbLogin,@dbPassword, @IsAdmin, @ServerName, @DatabaseName)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = hashedPassword;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = isAdmin;
                    cmd.Parameters.Add("@dbLogin", SqlDbType.NVarChar, 100).Value = "biltekbilisim";
                    cmd.Parameters.Add("@dbPassword", SqlDbType.NVarChar, 100).Value = "Bilisim20037816";
                    cmd.Parameters.Add("@ServerName", SqlDbType.NVarChar, 100).Value = adminServerName;
                    cmd.Parameters.Add("@DatabaseName", SqlDbType.NVarChar, 100).Value = adminDatabaseName;

                    newUserId = (int)cmd.ExecuteScalar(); // Yeni kullanıcının ID'sini al
                }

                // **Seçilen rapor yetkilerini kaydet**

                foreach (ListItem item in cblReports.Items)
                {

                    if (item.Selected)
                    {
                        string insertReportQuery = "INSERT INTO ReportPermissions (UserID, ReportName, CanView) VALUES (@UserID, @ReportName, 1)";

                        using (SqlCommand cmdReport = new SqlCommand(insertReportQuery, con))
                        {
                            cmdReport.Parameters.Add("@UserID", SqlDbType.Int).Value = newUserId;
                            cmdReport.Parameters.Add("@ReportName", SqlDbType.NVarChar, 100).Value = item.Value;
                            cmdReport.ExecuteNonQuery();
                        }
                    }
                }
            }

            successMessage.InnerText = "Kullanıcı başarıyla eklendi!";
            successMessage.Attributes["class"] = "alert alert-success d-block";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Main.aspx", false);
    }

    protected void ddlIsAdmin_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}
