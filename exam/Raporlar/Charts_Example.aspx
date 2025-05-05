<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Charts_Example.aspx.cs" Inherits="Raporlar_Charts_Example" %>
<%@ Register Assembly="DevExpress.XtraCharts.v24.2.Web" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <style>
    #page-wrapper {
        margin: 40px auto;
        max-width: 100%;
        padding: 10px;
    }

    .chart-wrapper {
        margin-bottom: 50px;
        width: 100%;
        overflow-x: auto;
        -webkit-overflow-scrolling: touch;
        display: flex;
        justify-content: center;
    }

    @media (max-width: 768px) {
        .chart-wrapper {
            padding: 0 5px;
        }
    }
</style>



    <div id="page-wrapper">
        <div class="chart-wrapper">
            <dx:WebChartControl ID="Chart1" runat="server"  Height="400px" />
        </div>
        <div class="chart-wrapper">
            <dx:WebChartControl ID="Chart2" runat="server"  Height="400px" />
        </div>
        <div class="chart-wrapper">
            <dx:WebChartControl ID="Chart3" runat="server"  Height="400px" />
        </div>
    </div>
</asp:Content>
