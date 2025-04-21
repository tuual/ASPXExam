    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.UI;
    using DevExpress.XtraCharts;

    public partial class Raporlar_Charts_Example : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCharts();
            }
        }

        protected void btnTarih_Click(object sender, EventArgs e)
        {
            LoadCharts();
        }

        private void LoadCharts()
        {
            DateTime date1 = DateFilter1.Date;
            DateTime date2 = DateFilter2.Date;

            string connStr = Session["DynamicConnectionString"] != null ? Session["DynamicConnectionString"].ToString() : null;
            if (string.IsNullOrEmpty(connStr)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                Chart1.DataSource = GetChartData(conn, "Belge Tipi", "ToplamAdet", GetQuery1(), date1, date2);
                Chart1.Series.Clear();
                Series s1 = new Series("Belge Tipleri", ViewType.Pie);
                s1.ArgumentDataMember = "Belge Tipi";
                s1.ValueDataMembers.AddRange("ToplamAdet");
                Chart1.Series.Add(s1);

                Chart2.DataSource = GetChartData(conn, "Stok Adı", "ToplamCikis", GetQuery2(), date1, date2);
                Chart2.Series.Clear();
                Series s2 = new Series("Toplam Çıkış", ViewType.Bar);
                s2.ArgumentDataMember = "Stok Adı";
                s2.ValueDataMembers.AddRange("ToplamCikis");
                Chart2.Series.Add(s2);

                Chart3.DataSource = GetChartData(conn, "Stok Adı", new[] { "ToplamCikis", "ToplamGiris" }, GetQuery3(), date1, date2);
                Chart3.Series.Clear();
                Series s3a = new Series("Çıkış", ViewType.Bar);
                s3a.ArgumentDataMember = "Stok Adı";
                s3a.ValueDataMembers.AddRange("ToplamCikis");
                Chart3.Series.Add(s3a);

                Series s3b = new Series("Giriş", ViewType.Bar);
                s3b.ArgumentDataMember = "Stok Adı";
                s3b.ValueDataMembers.AddRange("ToplamGiris");
                Chart3.Series.Add(s3b);
            }
        }

        private DataTable GetChartData(SqlConnection conn, string argField, string valField, string query, DateTime date1, DateTime date2)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@date1", SqlDbType.Date).Value = date1;
                cmd.Parameters.Add("@date2", SqlDbType.Date).Value = date2;

                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    return dt;
                }
            }
        }

        private DataTable GetChartData(SqlConnection conn, string argField, string[] valFields, string query, DateTime date1, DateTime date2)
        {
            return GetChartData(conn, argField, string.Join(",", valFields), query, date1, date2);
        }

        private string GetQuery1()
        {
            return @"
                   SELECT [Belge Tipi], COUNT(*) AS ToplamAdet
            FROM (
    SELECT
    CASE WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İrsaliyesi'
    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Faturası'
    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN  'Alış Faturası'
    WHEN TSH.STHAR_FTIRSIP = 6 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Müşteri Siparişi' 
    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN 'Alınan İade Faturası' 
    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade Faturası'
    WHEN TSH.STHAR_FTIRSIP = 4 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'G' THEN 'Alış İrsaliyesi'
    WHEN TSH.STHAR_FTIRSIP = 0 AND TSH.STHAR_HTUR = 'K' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Fişi'
    WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade İrsaliyesi (Taşıma)'

    ELSE 'Diğer'
    END AS 'Belge Tipi',
    TSH.FISNO AS 'Belge Numarası',
    TSH.STOK_KODU AS 'Stok Kodu', TS.STOK_ADI AS 'Stok Adı',
    CASE
    WHEN TSH.STHAR_GCKOD = 'C' THEN TSH.STHAR_GCMIK 
    ELSE 0 
    END AS 'Çıkış Miktar',
    CASE
    WHEN TSH.STHAR_GCKOD = 'G' THEN TSH.STHAR_GCMIK 
    ELSE 0 
    END AS 'Giriş Miktar',
    TSH.STHAR_NF AS 'Stok Net Fiyat',
    TSH.STHAR_BF AS 'Stok Birim Fiyat',
    TSH.DEPO_KODU AS 'İşlem Deposu',
    TSH.STHAR_TARIH TARIH

    FROM dbo.tStokMasterHareket TSH
    LEFT JOIN dbo.tStokMaster TS
    ON TS.STOK_KODU = TSH.STOK_KODU
            ) AS AltSorgu
            WHERE AltSorgu.TARIH BETWEEN @date1 AND @date2
            GROUP BY [Belge Tipi]";
        }

        private string GetQuery2()
        {
            return @"
            SELECT [Stok Adı], SUM([Çıkış Miktar]) AS ToplamCikis
            FROM (
    SELECT
    CASE WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İrsaliyesi'
    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Faturası'
    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN  'Alış Faturası'
    WHEN TSH.STHAR_FTIRSIP = 6 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Müşteri Siparişi' 
    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN 'Alınan İade Faturası' 
    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade Faturası'
    WHEN TSH.STHAR_FTIRSIP = 4 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'G' THEN 'Alış İrsaliyesi'
    WHEN TSH.STHAR_FTIRSIP = 0 AND TSH.STHAR_HTUR = 'K' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Fişi'
    WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade İrsaliyesi (Taşıma)'

    ELSE 'Diğer'
    END AS 'Belge Tipi',
    TSH.FISNO AS 'Belge Numarası',
    TSH.STOK_KODU AS 'Stok Kodu', TS.STOK_ADI AS 'Stok Adı',
    CASE
    WHEN TSH.STHAR_GCKOD = 'C' THEN TSH.STHAR_GCMIK 
    ELSE 0 
    END AS 'Çıkış Miktar',
    CASE
    WHEN TSH.STHAR_GCKOD = 'G' THEN TSH.STHAR_GCMIK 
    ELSE 0 
    END AS 'Giriş Miktar',
    TSH.STHAR_NF AS 'Stok Net Fiyat',
    TSH.STHAR_BF AS 'Stok Birim Fiyat',
    TSH.DEPO_KODU AS 'İşlem Deposu',
    TSH.STHAR_TARIH TARIH

    FROM dbo.tStokMasterHareket TSH
    LEFT JOIN dbo.tStokMaster TS
    ON TS.STOK_KODU = TSH.STOK_KODU
            ) AS AltSorgu
            WHERE AltSorgu.TARIH BETWEEN @date1 AND @date2
            GROUP BY [Stok Adı]";
        }

        private string GetQuery3()
        {
            return @"
            SELECT [Stok Adı], SUM([Çıkış Miktar]) AS ToplamCikis, SUM([Giriş Miktar]) AS ToplamGiris
            FROM (
    SELECT
    CASE WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İrsaliyesi'
    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Faturası'
    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'J' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN  'Alış Faturası'
    WHEN TSH.STHAR_FTIRSIP = 6 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Müşteri Siparişi' 
    WHEN TSH.STHAR_FTIRSIP = 2 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'G' THEN 'Alınan İade Faturası' 
    WHEN TSH.STHAR_FTIRSIP = 1 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade Faturası'
    WHEN TSH.STHAR_FTIRSIP = 4 AND TSH.STHAR_HTUR = 'H' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'G' THEN 'Alış İrsaliyesi'
    WHEN TSH.STHAR_FTIRSIP = 0 AND TSH.STHAR_HTUR = 'K' AND TSH.STHAR_BGTIP = 'F' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış Fişi'
    WHEN TSH.STHAR_FTIRSIP = 3 AND TSH.STHAR_HTUR = 'L' AND TSH.STHAR_BGTIP = 'I' AND TSH.STHAR_GCKOD = 'C' THEN 'Satış İade İrsaliyesi (Taşıma)'

    ELSE 'Diğer'
    END AS 'Belge Tipi',
    TSH.FISNO AS 'Belge Numarası',
    TSH.STOK_KODU AS 'Stok Kodu', TS.STOK_ADI AS 'Stok Adı',
    CASE
    WHEN TSH.STHAR_GCKOD = 'C' THEN TSH.STHAR_GCMIK 
    ELSE 0 
    END AS 'Çıkış Miktar',
    CASE
    WHEN TSH.STHAR_GCKOD = 'G' THEN TSH.STHAR_GCMIK 
    ELSE 0 
    END AS 'Giriş Miktar',
    TSH.STHAR_NF AS 'Stok Net Fiyat',
    TSH.STHAR_BF AS 'Stok Birim Fiyat',
    TSH.DEPO_KODU AS 'İşlem Deposu',
    TSH.STHAR_TARIH TARIH


    FROM dbo.tStokMasterHareket TSH
    LEFT JOIN dbo.tStokMaster TS
    ON TS.STOK_KODU = TSH.STOK_KODU
            ) AS AltSorgu
            WHERE AltSorgu.TARIH BETWEEN @date1 AND @date2
            GROUP BY [Stok Adı]";
        }
    }