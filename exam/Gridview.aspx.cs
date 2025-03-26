using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DevExpress.Web;

public partial class Account_Gridview : System.Web.UI.Page
{
    private string dbName;
    private string dbLogin = ConfigurationService.dbLogin;
    private string dbPassword = ConfigurationService.dbPassword;
    protected void Page_Load(object sender, EventArgs e)
    {
       

        // Kullanıcının giriş yapmasını zorunlu kıl-ma
        if (Session["UserID"] == null && Session["SecilenSirket"] == null && Session["ServerName"] == null)
        {
            Response.Redirect("~/Account/Login.aspx");
            return;
        }


        int userId = Convert.ToInt32(Session["UserID"]);
        string reportName = "Müşteri Hareketleri"; // Bu sayfa ile ilgili rapor adı
        UseReportLoader reportLoader = new UseReportLoader();
        if (!reportLoader.HasAccessToReport(userId, reportName))
        {
            Response.Redirect("~/Account/AccessDenied.aspx");
            return;
        }
      


            dbName = Session["SecilenSirket"].ToString();
        if (!IsPostBack)
        {
            BindEmptyGrid();
        }
        else
        {
            if (Session["FilteredQuery"] != null)
            {
                string query = Session["FilteredQuery"].ToString();
                string date1 = Session["Date1"].ToString();
                string date2 = Session["Date2"].ToString();
                BindGridView(query, date1, date2);
            }
        }
    }

    private void BindEmptyGrid()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Müşteri Kodu", typeof(string));
        dt.Columns.Add("Müşteri Ünvan", typeof(string));
        dt.Columns.Add("Fatura Numarası", typeof(string));
        dt.Columns.Add("Fatura Tarihi", typeof(string));
        dt.Columns.Add("Brüt Tutar", typeof(string));
        dt.Columns.Add("Genel Toplam", typeof(string));

        ASPxGridView1.DataSource = dt;
        ASPxGridView1.DataBind();
    }

    private void FilterDate()
    {
        string formatDate1 = DateFilter1.Date == DateTime.MinValue ? "" : DateFilter1.Date.ToString("yyyy-MM-dd");
        string formatDate2 = DateFilter2.Date == DateTime.MinValue ? "" : DateFilter2.Date.ToString("yyyy-MM-dd");

        string filterQuery = "SELECT CARI_KODU AS 'Müşteri Kodu', tcm.CARI_ISIM AS 'Müşteri Ünvan', FATIRS_NO AS 'Fatura Numarası'," +
                             " FORMAT(TARIH,'dd.MM.yyyy') AS 'Fatura Tarihi'," +
                             " FORMAT(CAST(BRUTTUTAR AS DECIMAL(18,2)),'N' ,'tr-TR') + ' TL' AS 'Brüt Tutar'," +
                             " FORMAT(CAST(GENELTOPLAM AS DECIMAL(18,2)),'N','tr-TR') + ' TL' AS 'Genel Toplam'" +
                             " FROM tFatura" +
                             " LEFT JOIN dbo.tCariMaster tCM ON tcm.CARI_KOD = dbo.tFatura.CARI_KODU";

        // Eğer tarih seçilmemişse tarih filtresini kaldır
        if (!string.IsNullOrEmpty(formatDate1) && !string.IsNullOrEmpty(formatDate2))
        {
            filterQuery += " WHERE TARIH BETWEEN @date1 AND @date2";
        }

        Session["FilteredQuery"] = filterQuery;
        Session["Date1"] = formatDate1;
        Session["Date2"] = formatDate2;

        BindGridView(filterQuery, formatDate1, formatDate2);
    }

    private void BindGridView(string query, string formatDate1, string formatDate2)
    {
        string connectionString = Session["DynamicConnectionString"] as string;

        if (string.IsNullOrEmpty(connectionString))
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            using (SqlCommand command = new SqlCommand(query, con))
            {
                DateTime date1, date2;

                if (DateTime.TryParse(formatDate1, out date1) && DateTime.TryParse(formatDate2, out date2))
                {
                    command.Parameters.Add("@date1", SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@date2", SqlDbType.Date).Value = date2;
                }
                else
                {
                    command.Parameters.Add("@date1", SqlDbType.Date).Value = DBNull.Value;
                    command.Parameters.Add("@date2", SqlDbType.Date).Value = DBNull.Value;
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ASPxGridView1.Columns.Clear();

                    foreach (DataColumn column in dt.Columns)
                    {
                        GridViewDataTextColumn gridColumn = new GridViewDataTextColumn
                        {
                            FieldName = column.ColumnName,
                            Caption = column.Caption,
                            VisibleIndex = dt.Columns.IndexOf(column)
                        };
                        ASPxGridView1.Columns.Add(gridColumn);
                    }

                    ASPxGridView1.DataSource = dt;
                    ASPxGridView1.KeyFieldName = "FATIRS_NO"; // Primary Key olabilecek bir alan
                    ASPxGridView1.SettingsBehavior.ColumnMoveMode = GridColumnMoveMode.ThroughHierarchy;
                    ASPxGridView1.Settings.ShowFilterRow = true;
                    ASPxGridView1.Settings.ShowFilterRowMenu = true;
                    ASPxGridView1.Settings.ShowHeaderFilterButton = true;
                    ASPxGridView1.SettingsBehavior.FilterRowMode = GridViewFilterRowMode.OnClick;
                    ASPxGridView1.SettingsBehavior.AllowFocusedRow = true;
                    ASPxGridView1.SettingsBehavior.EnableCustomizationWindow = true;
                    ASPxGridView1.SettingsBehavior.AllowDragDrop = true;

                    ASPxGridView1.DataBind();
                }
            }
        }
    }

    protected void btnTarih_Click(object sender, EventArgs e)
    {
        FilterDate();
    }

    protected void ASPxGridView1_PageIndexChanged(object sender, EventArgs e)
    {
        if (Session["FilteredQuery"] != null)
        {
        
            string query = Session["FilteredQuery"].ToString();
            string date1 = Session["Date1"].ToString();
            string date2 = Session["Date2"].ToString();
            BindGridView(query, date1, date2);
        }
    }
}
