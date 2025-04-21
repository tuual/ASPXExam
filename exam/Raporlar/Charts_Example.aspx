<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Charts_Example.aspx.cs" Inherits="Raporlar_Charts_Example" %>
<%@ Register Assembly="DevExpress.XtraCharts.v24.2.Web" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <style>
        #page-wrapper {
            margin: 40px auto;
            max-width: 1200px;
            padding: 10px;
        }

        .chart-wrapper {
            margin-bottom: 50px;
            width: 100%;
        }
    </style>

    <div id="page-wrapper">

        <!-- Grafik 1 -->
        <div class="chart-wrapper">
            <dx:WebChartControl ID="Chart1" runat="server" Width="1000px" Height="400px" CssClass="dxcharts" />
        </div>

        <!-- Grafik 2 -->
        <div class="chart-wrapper">
            <dx:WebChartControl ID="Chart2" runat="server" Width="1000px" Height="400px" CssClass="dxcharts" />
        </div>

        <!-- Grafik 3 -->
        <div class="chart-wrapper">
            <dx:WebChartControl ID="Chart3" runat="server" Width="1000px" Height="400px" CssClass="dxcharts" />
        </div>

    </div>
</asp:Content>
