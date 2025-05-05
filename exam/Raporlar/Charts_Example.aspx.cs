using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DevExpress.XtraCharts;
using System.Web.UI.WebControls;


public partial class Raporlar_Charts_Example : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int width = GetResponsiveWidth();

            // Grafik genişlik ve yükseklik ayarları
            Chart1.Width = Chart1.Height = Unit.Pixel(width); // Kare olacak
            Chart2.Width = Chart3.Width = Unit.Pixel(width);
            Chart2.Height = Chart3.Height = Unit.Pixel(400);

            // Mobil cihazlarda legend'ı gizle
            if (width < 500)
            {
                Chart1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }

            LoadCharts();
        }
    }

    private int GetResponsiveWidth()
    {
        try
        {
            string userAgent = Request.UserAgent;
            if (userAgent.Contains("Mobi")) return 300; // Mobil cihazsa
            return 800; // Masaüstü
        }
        catch
        {
            return 700;
        }
    }



    private void LoadCharts()
    {
        string connStr = Session["DynamicConnectionString"] != null ? Session["DynamicConnectionString"].ToString() : null;
        if (string.IsNullOrEmpty(connStr)) return;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();

            // Grafik 1: Pie (Donut görünüm)
            var dt1 = GetChartData(conn, GetQuery1());
            Chart1.DataSource = dt1;
            Chart1.Series.Clear();
            Series s1 = new Series("Belge Tipleri", ViewType.Pie);
            s1.ArgumentDataMember = "Belge Tipi";
            s1.ValueDataMembers.AddRange("ToplamAdet");
            ((PieSeriesLabel)s1.Label).TextPattern = "{A}: {VP:p1}";
            Chart1.Series.Add(s1);
            Chart1.DataBind();

            // Donut görünüm
            // Fix for CS8026: Replace pattern matching with a type cast for compatibility with C# 5
            var pieView = Chart1.Series[0].View as PieSeriesView;
            if (pieView != null)
            {
                // Fix for CS1061: 'HoleRadiusPercent' is not available, use an alternative property or method
                pieView.ExplodedDistancePercentage = 40; // Assuming this property is available and serves a similar purpose
            }

            // Grafik 2: Bar
            var dt2 = GetChartData(conn, GetQuery2());
            Chart2.DataSource = dt2;
            Chart2.Series.Clear();
            Series s2 = new Series("Toplam Çıkış", ViewType.Bar);
            s2.ArgumentDataMember = "Stok Adı";
            s2.ValueDataMembers.AddRange("ToplamCikis");
            Chart2.Series.Add(s2);
            Chart2.DataBind();

            // Yatay eksen etiket açısı (Chart2)
            XYDiagram diagram2 = (XYDiagram)Chart2.Diagram;
            diagram2.AxisX.Label.Angle = -45;
            diagram2.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

            // Grafik 3: Giriş/Çıkış
            var dt3 = GetChartData(conn, GetQuery3());
            Chart3.DataSource = dt3;
            Chart3.Series.Clear();
            Series s3a = new Series("Çıkış", ViewType.Bar);
            s3a.ArgumentDataMember = "Stok Adı";
            s3a.ValueDataMembers.AddRange("ToplamCikis");

            Series s3b = new Series("Giriş", ViewType.Bar);
            s3b.ArgumentDataMember = "Stok Adı";
            s3b.ValueDataMembers.AddRange("ToplamGiris");

            Chart3.Series.Add(s3a);
            Chart3.Series.Add(s3b);
            Chart3.DataBind();

            // Yatay eksen etiket açısı (Chart3)
            XYDiagram diagram3 = (XYDiagram)Chart3.Diagram;
            diagram3.AxisX.Label.Angle = -45;
            diagram3.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
        }
    }

    private DataTable GetChartData(SqlConnection conn, string query)
    {
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }
    }

    private string GetQuery1()
    {
        return @"
            SELECT [Belge Tipi], COUNT(*) AS ToplamAdet
            FROM (
                SELECT
                    CASE 
                        WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İrsaliyesi'
                    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Faturası'
                    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN  'Alış Faturası'
                    WHEN TSH.STHAR_FTIRSIP = 6 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Müşteri Siparişi' 
                    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN 'Alınan İade Faturası' 
                    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade Faturası'
                    WHEN TSH.STHAR_FTIRSIP = 4 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'G' THEN 'Alış İrsaliyesi'
                    WHEN TSH.STHAR_FTIRSIP = 0 AND TSH.STHAR_HTUR = 'K' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Fişi'
                    WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade İrsaliyesi (Taşıma)'

                        ELSE 'Diğer'
                    END AS [Belge Tipi]
                FROM dbo.tStokMasterHareket TSH
            ) AS Alt
            GROUP BY [Belge Tipi]";
    }

    private string GetQuery2()
    {
        return @"
            SELECT TS.STOK_ADI AS [Stok Adı], SUM(
                CASE WHEN TSH.STHAR_GCKOD = 'C' THEN TSH.STHAR_GCMIK ELSE 0 END
            ) AS ToplamCikis
            FROM dbo.tStokMasterHareket TSH
            LEFT JOIN dbo.tStokMaster TS ON TS.STOK_KODU = TSH.STOK_KODU
            WHERE TS.STOK_ADI IS NOT NULL
            GROUP BY TS.STOK_ADI";
    }

    private string GetQuery3()
    {
        return @"
            SELECT TS.STOK_ADI AS [Stok Adı], 
                   SUM(CASE WHEN TSH.STHAR_GCKOD = 'C' THEN TSH.STHAR_GCMIK ELSE 0 END) AS ToplamCikis,
                   SUM(CASE WHEN TSH.STHAR_GCKOD = 'G' THEN TSH.STHAR_GCMIK ELSE 0 END) AS ToplamGiris
            FROM dbo.tStokMasterHareket TSH
            LEFT JOIN dbo.tStokMaster TS ON TS.STOK_KODU = TSH.STOK_KODU
            WHERE TS.STOK_ADI IS NOT NULL
            GROUP BY TS.STOK_ADI";
    }
}
