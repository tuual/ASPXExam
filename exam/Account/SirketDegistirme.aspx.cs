using System;
using System.Data.SqlClient;
using System.Drawing;
using DevExpress.Web.Bootstrap;
using DevExpress.Web.Internal;

public partial class Account_SirketDegistirme : System.Web.UI.Page
{
    String dbname, connectionString, selectQuery;
    private string dbLogin = ConfigurationService.dbLogin;
    private string dbPassword = ConfigurationService.dbPassword;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["ServerName"] == null || Session["UserID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            try
            {
                dbname = Session["ServerName"].ToString();
                connectionString = string.Format("Server={0};Database=BB_TICARI;User Id=" + dbLogin + ";Password=" + dbPassword + ";", dbname);
                selectQuery = "SELECT SIRKET_ADI FROM dbo.FIRMALAR WHERE DURUM <> 2";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cbSirketSec.Items.Clear();

                            while (reader.Read())
                            {
                                string sirketAdi = reader["SIRKET_ADI"].ToString();
                                BootstrapListEditItem li = new BootstrapListEditItem(sirketAdi);
                                cbSirketSec.Items.Add(li);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Hata: " + ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (cbSirketSec.SelectedIndex > -1)
        {
            String secilenSirket = cbSirketSec.SelectedItem.Text;
            Session["SecilenSirket"] = secilenSirket;

            Session["DynamicConnectionString"] =
                "Server=" + Session["ServerName"] +
                ";Database=" + Session["SecilenSirket"] +
                ";User Id=" + dbLogin + ";Password=" + dbPassword + ";";

            lblMessage.ForeColor = Color.Green;
            lblMessage.Text = "Şirket başarıyla değiştirildi. Yönlendiriliyorsunuz...";

            ClientScript.RegisterStartupScript(this.GetType(), "CloseAndReload", "window.parent.location.reload();", true);
        }
        else
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "Lütfen bir şirket seçiniz.";
        }
    }
}
