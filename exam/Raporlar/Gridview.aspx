<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="~/Raporlar/Gridview.aspx.cs" Inherits="Account_Gridview" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <style>
        #page-wrapper {
            margin: 30px;
            transition: margin-left 0.3s ease-in-out;
        }
        
        @media (max-width: 768px) {
            #page-wrapper {
                margin: 15px;
            }
        }
        .calendar-margin {
    margin-left:50px;
    z-index: 1000; /* Üste çıkmasını garanti eder */
}

        .input-small .dxbs-dateedit,
    .input-small .dxbs-dateedit input {
        font-size: 0.875rem !important; /* Küçük yazı */
        height: 34px !important;        /* Daha az yükseklik */
    }

    .btn-small .btn {
        font-size: 0.875rem !important;
        height: 36px !important;
        padding: 4px 12px !important;
    }

    @media (max-width: 768px) {
    .dxbs-dateedit-popup {
        max-width: 95vw !important;
        left: 5px !important;
        right: auto !important;
    }
}


    </style>

    <div id="page-wrapper">

        <!-- Tarih filtre alanı -->
     <style>
    .input-small .dxbs-dateedit,
    .input-small .dxbs-dateedit input {
        font-size: 0.875rem !important;
        height: 34px !important;
    }

    .btn-small .btn {
        font-size: 0.875rem !important;
        height: 36px !important;
        padding: 4px 12px !important;
    }
</style>

<div class="row mb-3 d-flex align-items-end">
    <div class="col-auto input-small" style="min-width: 200px;">
        <dx:BootstrapDateEdit CssClasses-Calendar="calendar-margin" ID="DateFilter1" runat="server" EditFormat="Custom" Width="200px"
            AllowUserInput="true" EditFormatString="dd/MM/yyyy" Caption="Başlangıç Tarih" UseMaskBehavior="true">
        </dx:BootstrapDateEdit>
    </div>

    <div class="col-auto input-small" style="min-width: 200px;">
        <dx:BootstrapDateEdit CssClasses-Calendar="input-small" ID="DateFilter2" runat="server" EditFormat="Custom" EditFormatString="dd/MM/yyyy"
            AllowUserInput="true" Width="200px" Caption="Bitiş Tarih" UseMaskBehavior="true">
        </dx:BootstrapDateEdit>
    </div>

    <div class="col-auto btn-small mt-2" style="min-width: 150px;">
        <dx:BootstrapButton ID="btnTarih" Text="Uygula" runat="server" Width="200px" AutoPostBack="false" OnClick="btnTarih_Click">
            <SettingsBootstrap RenderOption="Default" />
        </dx:BootstrapButton>
    </div>
     

</div>
      
       <dx:BootstrapComboBox runat="server" SelectedIndex="0" Width="150px" Caption="Kayıt Sayısı">
    <Items>
        <dx:BootstrapListEditItem Text="Item1" Value="Item1" />
        <dx:BootstrapListEditItem Text="Item2" Value="Item2" />
        <dx:BootstrapListEditItem Text="Item3" Value="Item3" />
        <dx:BootstrapListEditItem Text="Item4" Value="Item4" />
        <dx:BootstrapListEditItem Text="Item5" Value="Item5" />
    </Items>
</dx:BootstrapComboBox>


        
        <br />

        <!-- GridView - responsive scroll destekli -->
        <div style="overflow-x:hidden;">
    
            <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="true" Width="100%" 
                KeyFieldName="FATIRS_NO" Theme="MaterialCompact" OnPageIndexChanged="ASPxGridView1_PageIndexChanged">
                
                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                <SettingsBehavior ColumnMoveMode="ThroughHierarchy" />
                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                <Settings ShowHeaderFilterButton="true" />
                <SettingsBehavior FilterRowMode="OnClick" AllowFocusedRow="true" EnableCustomizationWindow="true" />
                <SettingsExport EnableClientSideExportAPI="true" />
                <Settings VerticalScrollableHeight="300" />
                <SettingsContextMenu Enabled="true">
                    <RowMenuItemVisibility ExportMenu-Visible="true" />
                </SettingsContextMenu>
                <SettingsResizing ColumnResizeMode="Control" Visualization="Postponed" />
                <Styles>
                    <FixedColumn BackColor="LightBlue"></FixedColumn>
                </Styles>
                <Toolbars>
                    <dx:GridViewToolbar>
                        <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                        <Items>
                            <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF İndir" />
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="Excel İndir" />
                        </Items>
                    </dx:GridViewToolbar>
                </Toolbars>
              
            </dx:ASPxGridView>
             
        </div>

        <!-- Popup -->
        <dx:ASPxPopupControl ID="popupFaturaDetay" runat="server" Width="700px" Height="400px" ShowHeader="true" Modal="true" CloseAction="CloseButton">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxGridView ID="gridFaturaDetay" runat="server" Width="100%" AutoGenerateColumns="true" />
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

    </div> <!-- /#page-wrapper -->
</asp:Content>
