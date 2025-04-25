using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using DevExpress.DataProcessing;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;

/// <summary>
/// Summary description for UseReportLoader
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
                    Dictionary<string, List<KeyValuePair<string, string>>> groupedReports = new Dictionary<string, List<KeyValuePair<string, string>>>();

                    groupedReports["Finans"] = new List<KeyValuePair<string, string>>();
                    groupedReports["Stok"] = new List<KeyValuePair<string, string>>();
                    groupedReports["Diğer"] = new List<KeyValuePair<string, string>>();

                    while (reader.Read())
                    {
                        // Kullanıcının yetkili olduğu raporları oku
                        string reportName = reader["ReportName"].ToString();
                        string reportUrl = "";
                        if (reportName == "Müşteri Hareketleri")
                        {

                            reportUrl = "/Raporlar/Gridview.aspx";
                            groupedReports["Finans"].Add(new KeyValuePair<string, string>(reportName, reportUrl));
                            
                        }
                        else if (reportName == "Fatura Raporu")
                        {
                            reportUrl = "/Raporlar/FaturaGrid.aspx";
                            groupedReports["Finans"].Add(new KeyValuePair<string, string>(reportName, reportUrl));
                        }
                        else if (reportName == "Stok Durumu")
                        {
                            reportUrl = "/Raporlar/StokDurumu.aspx";
                            groupedReports["Stok"].Add(new KeyValuePair<string, string>(reportName, reportUrl));
                        }
                        else if (reportName == "Evrak Bilgileri")
                        {
                            reportUrl = "/Raporlar/Charts_Example.aspx";
                            groupedReports["Finans"].Add(new KeyValuePair<string, string>(reportName, reportUrl));
                        }
                        else
                        {
                            reportUrl = "#";
                            groupedReports["Diğer"].Add(new KeyValuePair<string, string>(reportName, reportUrl));
                        }
                    }

                    // Lineicon ikonları kategori adlarıyla eşleşiyor
                    Dictionary<string, string> categoryIcons = new Dictionary<string, string>()
                {
                    { "Finans", "lni-credit-cards" },
                    { "Stok", "lni-dropbox" },
                    { "Diğer", "lni-archive" }
                };

                    string reportsHtml = "<div class='accordion' id='reportAccordion'>";
                    int index = 0;

                    foreach (var group in groupedReports)
                    {
                        if (group.Value.Count == 0)
                            continue;

                        string collapseId = "collapse" + index;
                        string headingId = "heading" + index;

                        string iconClass = categoryIcons.ContainsKey(group.Key) ? categoryIcons[group.Key] : "";

                        reportsHtml += "<div class='accordion-item'>";
                        reportsHtml += "<h2 class='accordion-header' id='" + headingId + "'>";
                        reportsHtml += "<button class='accordion-button collapsed' type='button' data-bs-toggle='collapse' data-bs-target='#" + collapseId + "' aria-expanded='false' aria-controls='" + collapseId + "'>";

                        // İKON + KATEGORİ ADI
                        reportsHtml += "<i class='lni " + iconClass + " me-2'></i> " + group.Key + " Raporları";

                        reportsHtml += "</button>";
                        reportsHtml += "</h2>";

                        reportsHtml += "<div id='" + collapseId + "' class='accordion-collapse collapse' aria-labelledby='" + headingId + "' data-bs-parent='#reportAccordion'>";
                        reportsHtml += "<div class='accordion-body p-0'>";
                        reportsHtml += "<ul class='list-group list-group-flush'>";

                        foreach (var report in group.Value)
                        {
                            reportsHtml += "<li class='list-group-item tree-child'><span class='tree-child-arrow'>↓</span> <a href='" + report.Value + "'>" + report.Key + "</a></li>";
                        }

                        reportsHtml += "</ul></div></div></div>";
                        index++;
                    }

                    reportsHtml += "</div>";
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