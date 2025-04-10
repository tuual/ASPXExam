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
        string userid = Session["UserID"].ToString();
        string servername = Session["ServerName"].ToString();
        string secilensirket = Session["SecilenSirket"].ToString();
        MsgBox msg = new MsgBox(userid+servername+secilensirket , this.Page, this);

     
    }
}
