<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Charts_Example.aspx.cs" Inherits="Raporlar_Charts_Example" %>
<%@ Register assembly="DevExpress.XtraCharts.v24.2.Web" namespace="DevExpress.XtraCharts.Web" tagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

<style>
    #page-wrapper {
        margin: 30px;
    }

    @media (max-width: 768px) {
        #page-wrapper {
            margin: 15px;
        }
    }

    .chart-wrapper {
        margin-bottom: 50px;
    }
</style>

<div id="page-wrapper">

    <!-- Tarih Filtreleri -->
    <div class="row mb-3 d-flex align-items-end">
        <div class="col-auto input-small" style="min-width: 200px;">
            <dx:BootstrapDateEdit ID="DateFilter1" runat="server" EditFormat="Custom" EditFormatString="dd/MM/yyyy" Caption="Başlangıç Tarihi" UseMaskBehavior="true" Width="200px" />
        </div>

        <div class="col-auto input-small" style="min-width: 200px;">
            <dx:BootstrapDateEdit ID="DateFilter2" runat="server" EditFormat="Custom" EditFormatString="dd/MM/yyyy" Caption="Bitiş Tarihi" UseMaskBehavior="true" Width="200px" />
        </div>

        <div class="col-auto btn-small mt-2" style="min-width: 150px;">
            <dx:BootstrapButton ID="btnTarih" Text="Uygula" runat="server" Width="200px" OnClick="btnTarih_Click">
                <SettingsBootstrap RenderOption="Default" />
            </dx:BootstrapButton>
        </div>
    </div>

    <!-- Grafik 1: Belge Tipine Göre Adet (Pasta Grafik) -->
    <div class="chart-wrapper">
        <dx:WebChartControl ID="Chart1" runat="server" Width="800px" Height="400px" />
    </div>

    <!-- Grafik 2: Stok Adı - Toplam Çıkış (Sütun Grafik) -->
    <div class="chart-wrapper">
        <dx:WebChartControl ID="Chart2" runat="server" Width="800px" Height="400px" />
    </div>

    <!-- Grafik 3: Stok Adı - Giriş/Çıkış Karşılaştırma (Çubuk Grafik) -->
    <div class="chart-wrapper">
        <dx:WebChartControl ID="Chart3" runat="server" Width="800px" Height="400px" />
    </div>

</div>

</asp:Content>
