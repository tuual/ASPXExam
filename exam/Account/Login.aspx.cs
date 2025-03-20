using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

public partial class Login : System.Web.UI.Page
{
    private readonly string connectionString = "Server=BLTTUAL;Database=Kullanicilar;User Id=biltekbilisim;Password=Bilisim20037816;";

    protected void Page_Load(object sender, EventArgs e)
    {
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

                string query = "SELECT ID, ServerName, DbLogin, DbPassword, DatabaseName FROM Users WHERE Username = @Username AND PasswordHash = @Password";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string userId = reader["ID"].ToString();
                            string serverName = reader["ServerName"].ToString();
                            string dbLogin = reader["DbLogin"].ToString();
                            string dbPassword = reader["DbPassword"].ToString();
                            string databaseName = reader["DatabaseName"].ToString();

                            string dynamicConnectionString = string.Format(
                                "Server={0};Database={1};User Id={2};Password={3};",
                                serverName, databaseName, dbLogin, dbPassword
                            );

                            // **Session Değerlerini Kaydet**
                            Session["UserID"] = userId;
                            Session["DynamicConnectionString"] = dynamicConnectionString;
                            Session["DatabaseName"] = databaseName;
                            Session["IsAuthenticated"] = true;  // Kullanıcı oturum açtı mı?

                            Response.Redirect("~/Gridview.aspx");
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
}
