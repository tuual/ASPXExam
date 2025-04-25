using System;
using System.Web.UI;

public partial class Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["SecilenSirket"] == null)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
      

     
    }
}
