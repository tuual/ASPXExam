<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="~/Raporlar/Gridview.aspx.cs" Inherits="Account_Gridview" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <style>
        body{
            padding-left: 20px;
            padding-right:20px;
        }
        #page-wrapper {
            margin: 50px;
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
    <asp:HiddenField ID="hfWidth" runat="server" />

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
       <dx:BootstrapDateEdit ID="DateFilter1" runat="server" Width="250px"
    EditFormat="Custom" EditFormatString="dd/MM/yyyy" Caption="Başlangıç Tarihi"
    UseMaskBehavior="true" ClientInstanceName="date1">
    <CalendarProperties ChangeVisibleDateAnimationType="Slide" />
</dx:BootstrapDateEdit>

    </div>

    <div class="col-auto input-small" style="min-width: 200px;">
        <dx:BootstrapDateEdit CssClasses-Calendar="input-small" ID="DateFilter2" runat="server" EditFormat="Custom" EditFormatString="dd/MM/yyyy"
            AllowUserInput="true" Width="250px" Caption="Bitiş Tarih" UseMaskBehavior="true">
        </dx:BootstrapDateEdit>
    </div>

    <div class="col-auto btn-small mt-2" style="min-width: 200px;">
        <dx:BootstrapButton ID="btnTarih" Text="Uygula" runat="server" Width="200px" AutoPostBack="false" OnClick="btnTarih_Click">
            <SettingsBootstrap RenderOption="Default" />
        </dx:BootstrapButton>
    </div>
     

</div>
      
       <dx:BootstrapComboBox runat="server" SelectedIndex="0" Width="150px" Caption="Kayıt Sayısı" AutoPostBack="true" OnSelectedIndexChanged="Unnamed_SelectedIndexChanged" ID="cbDegerSayisi">
    <Items>
        <dx:BootstrapListEditItem Text="10" Value="10" />
        <dx:BootstrapListEditItem Text="50" Value="50" />
        <dx:BootstrapListEditItem Text="100" Value="100" />
        <dx:BootstrapListEditItem Text="200" Value="200" />
    </Items>
</dx:BootstrapComboBox>


        
        <br />

        <!-- GridView - responsive scroll destekli -->
        <div style="overflow-x:hidden;">
    
            <dx:ASPxGridView ID="ASPxGridView1"  runat="server" AutoGenerateColumns="true" Width="100%" 
                KeyFieldName="INCKEYNO" ClientInstanceName="grid" Theme="MaterialCompact" OnDataBound="ASPxGridView1_DataBound" OnPageIndexChanged="ASPxGridView1_PageIndexChanged">
                
                <Settings ShowHeaderFilterButton="true" />
      
                
                <SettingsBehavior FilterRowMode="OnClick" AllowFocusedRow="true" EnableCustomizationWindow="true" />
                <Settings VerticalScrollableHeight="300" />
                <SettingsContextMenu Enabled="true">
                    <RowMenuItemVisibility ExportMenu-Visible="true" >
<ExportMenu Visible="True"></ExportMenu>
                    </RowMenuItemVisibility>
                </SettingsContextMenu>
                
                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsBehavior ColumnMoveMode="ThroughHierarchy" />
                <SettingsResizing ColumnResizeMode="Control" Visualization="Postponed" />
                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />

<SettingsPopup>
<FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>

                <SettingsSearchPanel Visible="True" />
                <SettingsExport EnableClientSideExportAPI="true" />
                <Columns>
                    <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0">
                    </dx:GridViewCommandColumn>
                </Columns>
                <Toolbars>
                    <dx:GridViewToolbar>
                        <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                        <Items>
                            <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF İndir" />
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="Excel İndir" />
                        </Items>
                    </dx:GridViewToolbar>
                </Toolbars>
              
                <Styles>
                    <FixedColumn BackColor="LightBlue"></FixedColumn>
                </Styles>
              
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
    <script type="text/javascript">
        function resizeCalendarPopup() {
            var editor = date1; // ClientInstanceName
            if (editor && editor.GetCalendar()) {
                var calendar = editor.GetCalendar();
                var editorWidth = editor.GetInputElement().offsetWidth;

                // Takvim popup'ını bul ve genişliğini ayarla
                var popup = calendar.GetMainElement();
                if (popup) {
                    popup.style.width = editorWidth + "px";
                }
            }
        }

        // Takvim açıldığında çağır
        date1.CalendarShown.AddHandler(resizeCalendarPopup);
    </script>


</asp:Content>
