using System;
using System.Activities.Expressions;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DevExpress.DashboardCommon.DataBinding;
using DevExpress.Web;


public partial class Account_Gridview : System.Web.UI.Page
{
    string connectionString = "Server=192.168.1.8;Database=EDABGIDA;User Id=biltekbilisim;Password=Bilisim20037816;Trusted_Connection=True;";
    SqlCommand command;
    protected void Page_Load(object sender, EventArgs e)
    {

      
            var defaultQuery = "SELECT CARI_KODU AS 'Müşteri Kodu' ,tcm.CARI_ISIM AS 'Müşteri Ünvan', FATIRS_NO AS 'Fatura Numarası', FORMAT(TARIH,'dd.MM.yyyy') as 'Fatura Tarihi' , FORMAT(CAST(BRUTTUTAR AS NUMERIC(10,1)),'N' ,'tr-TR')+' TL' AS 'Brüt Tutar', FORMAT(CAST(GENELTOPLAM AS NUMERIC(10,1)),'N','tr-TR')+ ' TL' AS 'Genel Toplam' FROM tFatura LEFT JOIN dbo.tCariMaster tCM ON tcm.CARI_KOD = dbo.tFatura.CARI_KODU";

            BindGridView(defaultQuery);


            BindTextBox();
        

      
    }
    
    private void FilterDate()
    {
        var formatDate = DateFilter1.Date.ToString("yyyy-MM-dd");
        var formatDate2 = DateFilter2.Date.ToString("yyyy-MM-dd");
        var filterQuery = "SELECT CARI_KODU AS 'Müşteri Kodu' ,tcm.CARI_ISIM AS 'Müşteri Ünvan', FATIRS_NO AS 'Fatura Numarası'," +
            " FORMAT(TARIH,'dd.MM.yyyy') AS 'Fatura Tarihi', " +
            "FORMAT(CAST(BRUTTUTAR AS NUMERIC(10,1)),'N' ,'tr-TR')+' TL' AS 'Brüt Tutar'," +
            " FORMAT(CAST(GENELTOPLAM AS NUMERIC(10,1)),'N','tr-TR')+ ' TL' AS 'Genel Toplam' " +
            "FROM tFatura" +
            " LEFT JOIN dbo.tCariMaster tCM" +
            " ON tcm.CARI_KOD = dbo.tFatura.CARI_KODU " +
            "WHERE TARIH BETWEEN @date1 AND @date2";
        BindGridViewWithParams(filterQuery, formatDate, formatDate2);

    }

    private void BindGridViewWithParams(string filterQuery, string formatDate, string formatDate2)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(filterQuery, con))
            {
                command.Parameters.Add("@date1", SqlDbType.Date).Value = formatDate;
                command.Parameters.Add("@date2", SqlDbType.Date).Value = formatDate2;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ASPxGridView1.DataSource = dt;
                    ASPxGridView1.DataBind();
                }
            }
        }
    }

    private void BindTextBox()
    {
        DateFilter1.EditFormatString = "dd/MM/yyyy";

    }

    private void BindGridView(String query)
    {

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
