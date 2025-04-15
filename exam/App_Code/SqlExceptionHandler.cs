using System;
using System.Web;
using System.Web.UI;

/// <summary>  
/// SqlException için özet açıklama  
/// </summary>  
public class SqlExceptionHandler
{
    private MsgBox msgBox;
    public SqlExceptionHandler(System.Data.SqlClient.SqlException ex, Page page)
    {
        switch (ex.Number)
        {
            case 4060:
                msgBox = new MsgBox("Veritabanı bağlantısı sağlanamadı", page, this);
                break;
            case 18456:
                msgBox = new MsgBox("Veritabanı bağlantısı sağlanamadı", page, this);
                break;
            case 2627:
                msgBox = new MsgBox("Bu kayıt sistemde mevcut.", page, this);
                break;
            case 2601:
                msgBox = new MsgBox("Benzersiz index verilmeli", page, this);
                break;
            case 208:
                msgBox = new MsgBox("Tablo veya sütun bulunamadı", page, this);
                break;
            case 245:
                msgBox = new MsgBox("Dönüştürme hatası.", page, this);
                break;
            default:
                msgBox = new MsgBox("Hata oluştu", page, this);
                break;
        }
    }
}