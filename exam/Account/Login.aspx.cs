using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class Login : System.Web.UI.Page
{
    private readonly string connectionString = "Server=BLTTUAL;Database=Kullanicilar;User Id=biltekbilisim;Password=Bilisim20037816;";

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private List<string> GetUserReports(int userId)
    {
        List<string> reportList = new List<string>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT ReportName FROM ReportPermissions WHERE UserID = @UserID AND CanView = 1";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reportList.Add(reader["ReportName"].ToString());
                    }
                }
            }
        }

        return reportList;
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = tbUserName.Text.Trim();
        string password = tbPassword.Text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "Lütfen kullanıcı adı ve şifre girin.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT ID, ServerName, DbLogin, DbPassword, DatabaseName, IsAdmin, PasswordHash FROM Users WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Kullanıcı bulunduysa
                        {
                            string storedHash = reader["PasswordHash"].ToString(); // Veritabanındaki hash

                            // Kullanıcının girdiği şifre ile hash karşılaştır
                            if (!VerifyPassword(password, storedHash))
                            {
                                lblMessage.Text = "Hatalı kullanıcı adı veya şifre!";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                return;
                            }

                            string userId = reader["ID"].ToString();
                            string serverName = reader["ServerName"].ToString();
                            string dbLogin = reader["DbLogin"].ToString();
                            string dbPassword = reader["DbPassword"].ToString();
                            string databaseName = reader["DatabaseName"].ToString();

                            // **IsAdmin Değerini Kontrol Et**
                            bool isAdmin = false;
                            if (reader["IsAdmin"] != DBNull.Value)
                            {
                                isAdmin = Convert.ToBoolean(reader["IsAdmin"]);
                            }

                            if (string.IsNullOrEmpty(databaseName))
                            {
                                lblMessage.Text = "Kullanıcının bağlı olduğu veritabanı bulunamadı!";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                return;
                            }

                            // **Session Değerlerini Kaydet**
                            Session["UserID"] = userId;
                            Session["ServerName"] = serverName;
                            Session["DatabaseName"] = databaseName;
                            Session["IsAdmin"] = isAdmin;
                            Session["IsAuthenticated"] = true;

                            // **Başarılı giriş, Main sayfasına yönlendir**
                            Response.Redirect("SirketSecme.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "Hatalı kullanıcı adı veya şifre!";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Bir hata oluştu: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    private bool VerifyPassword(string enteredPassword, string storedHash)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(enteredPassword);
            byte[] hash = sha256.ComputeHash(bytes);
            string enteredHash = Convert.ToBase64String(hash);
            return enteredHash == storedHash; // Girilen şifreyi hashleyip karşılaştır
        }
    }
}
