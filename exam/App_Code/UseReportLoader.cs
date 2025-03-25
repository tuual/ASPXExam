using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UseReportLoader
/// </summary>
public class UseReportLoader
{
    private string connectionString = "Server=BLTTUAL;Database=Kullanicilar;User Id=biltekbilisim;Password=Bilisim20037816;";

    public string LoadUserReports(int? userId, bool isAdmin)
    {
        if (userId == null)
        {
            return "<li class='list-group-item text-danger'>Oturum Açın!</li>";
        }

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = isAdmin
                ? "SELECT DISTINCT ReportName FROM dbo.ReportPermissions"
                : "SELECT ReportName FROM dbo.ReportPermissions WHERE UserID = @UserID AND CanView = 1";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (!isAdmin)
                {
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    string reportsHtml = "";
                    while (reader.Read())
                    {
                        string reportName = reader["ReportName"].ToString();
                        string reportUrl = "";

                        // **Hangi rapor hangi sayfaya gidecek?**
                        if (reportName == "Müşteri Hareketleri")
                            reportUrl = "Gridview.aspx";
                        else if (reportName == "Fatura Raporu")
                            reportUrl = "Raporlar/FaturaGrid.aspx";

                        // **Eğer link boşsa, hata mesajı ekleyelim**
                        if (string.IsNullOrEmpty(reportUrl))
                        {
                            Debug.WriteLine("UYARI: " + reportName + " için bir URL atanmadı!");
                        }

                        reportsHtml += "<li class='list-group-item'><a href='" + reportUrl + "'>" + reportName + "</a></li>";
                    }

                    return reportsHtml;
                }
            }
        }
    }


    public bool HasAccessToReport(int userId, string reportName)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT COUNT(*) FROM dbo.ReportPermissions WHERE UserID = @UserID AND ReportName = @ReportName AND CanView = 1";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = reportName;

                int count = (int)cmd.ExecuteScalar();

                // Eğer 1'den fazla kayıt varsa, erişim izni vardır
                return count > 0;
            }
        }
    }
}