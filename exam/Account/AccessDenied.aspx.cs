using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccessDenied : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Timer timer = new Timer();
        timer.Interval = 5000;
        timer.Enabled = true;
        Response.Redirect("~/Account/Login.aspx");

    }
}