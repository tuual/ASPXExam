using System;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["SecilenSirket"] == null)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Session["DynamicConnectionString"] == null)
            {
                // Gerekirse yönlendir
                Response.Redirect("~/Account/SirketSecme.aspx");
            }

            // → BURADA connection stringi kullanarak veri çekiyorsan, Session'dan aldığından emin ol:
            string connStr = Session["DynamicConnectionString"].ToString();

            // örnek:
            using (SqlConnection con = new SqlConnection(connStr))
            {
                // veriyi çek ve grid'e/label'a bind et
            }
        }


    }
}
