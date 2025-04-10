using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for MsgBox
/// </summary>
public class MsgBox
{
	public MsgBox(String ex,Page pg,Object obj)
	{
        /*MsgBox msgBox = new  MsgBox("mesaj",this.Page,this);*/ 
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }
}