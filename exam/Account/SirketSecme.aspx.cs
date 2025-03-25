using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Routing;
using System.Web.UI;
using DevExpress.Web.Bootstrap;

public partial class Account_SirketSecme : System.Web.UI.Page
{
    String dbname, connectionString, selectQuery;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ServerName"] == null && Session["UserID"] == null)
        {
            Response.Redirect("~/Account/Login.aspx");
            return;
        }else
        {

       
            try
            {
              
                dbname = Session["ServerName"].ToString();
                connectionString = string.Format("Server={0};Database=BB_TICARI;User Id=biltekbilisim;Password=Bilisim20037816;", dbname);
                selectQuery = "SELECT SIRKET_ADI FROM dbo.FIRMALAR WHERE DURUM <> 2";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
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
                lblMessage.Text = "Bir hata oluştu: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (cbSirketSec.SelectedIndex > -1)
        {
            String secilenSirket = cbSirketSec.SelectedItem.Text;
            Session["SecilenSirket"] = secilenSirket;
            lblMessage.ForeColor = Color.Green;
            
            Session["DynamicConnectionString"] = "Server=" + Session["ServerName"] + ";Database=" + Session["SecilenSirket"] +";User Id=biltekbilisim;Password=Bilisim20037816;";
            lblMessage.Text = "Şirket seçimi başarılı. Yönlendiriliyorsunuz...";
            Timer timer = new Timer();
            timer.Interval = 2000;
            Response.Redirect("~/Gridview.aspx");

        }
        else
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "Lütfen bir şirket seçiniz.";
        }
     
    }
}
