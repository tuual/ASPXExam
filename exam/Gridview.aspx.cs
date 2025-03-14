using System;
using System.Activities.Expressions;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DevExpress.DashboardCommon.DataBinding;
using DevExpress.Web;


public partial class Account_Gridview : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        var defaultQuery = "SELECT CARI_KODU AS 'Müşteri Kodu' ,tcm.CARI_ISIM AS 'Müşteri Ünvan', FATIRS_NO AS 'Fatura Numarası', TARIH AS 'Fatura Tarihi', CAST(BRUTTUTAR AS NUMERIC(10,1)) AS 'Brüt Tutar' FROM tFatura LEFT JOIN dbo.tCariMaster tCM ON tcm.CARI_KOD = dbo.tFatura.CARI_KODU";
     

            BindGridView(defaultQuery);

        
        BindTextBox();
    }
    
    private void FilterDate()
    {
        
        var date1 = DateFilter1.Text;
        var formatDate = DateTime.Parse(date1).ToString("yyyy-MM-dd");
        var date2 = DateFilter2.Text;
        var formatDate2 = DateTime.Parse(date2).ToString("yyyy-MM-dd");
        var filterQuery = "SELECT CARI_KODU AS 'Müşteri Kodu', " +
                          "tcm.CARI_ISIM AS 'Müşteri Ünvan', " +
                          "FATIRS_NO AS 'Fatura Numarası', " +
                          "CONVERT(VARCHAR, TARIH, 23) AS 'Fatura Tarihi', " +
                          "CAST(BRUTTUTAR AS NUMERIC(10, 1)) AS 'Brüt Tutar' " +
                          "FROM dbo.tFatura " +
                          "LEFT JOIN dbo.tCariMaster tCM ON tcm.CARI_KOD = dbo.tFatura.CARI_KODU " +
                          "WHERE TARIH BETWEEN'" +formatDate+"' AND '" + formatDate2 + "'";
        BindGridView(filterQuery);
    }

    private void BindTextBox()
    {
        DateFilter1.Date = DateTime.Now;
        DateFilter1.EditFormatString = "dd/MM/yyyy";
        DateFilter2.Date = DateTime.Now;

    }

    private void BindGridView(String query)
    {
        string connectionString = "Server=192.168.22.150;Database=EDABGIDA;User Id=biltekbilisim;Password=Bilisim20037816;Trusted_Connection=True;";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, con))
            {
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
}
