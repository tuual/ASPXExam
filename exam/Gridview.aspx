<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="~/Gridview.aspx.cs" Inherits="Account_Gridview" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

  <div class="column" style="display: flex; align-items: flex-end; gap: 10px;">
    <div>
        <dx:BootstrapDateEdit ID="DateFilter1" runat="server" EditFormat="Custom" Width="190"
            AllowUserInput="true" EditFormatString="dd/MM/yyyy" Caption="Başlangıç Tarih" UseMaskBehavior="true">
        </dx:BootstrapDateEdit>
    </div>

    <div>
        <dx:BootstrapDateEdit ID="DateFilter2" runat="server" EditFormat="Custom" EditFormatString="dd/MM/yyyy" AllowUserInput="true" 
            Width="190" Caption="Bitiş Tarih"  UseMaskBehavior="true">
        </dx:BootstrapDateEdit>
    </div>

    <div style="position: relative;">
        <dx:BootstrapButton ID="btnTarih" Text="Uygula" runat="server" Width="150"  AutoPostBack="false" OnClick="btnTarih_Click">
            <SettingsBootstrap RenderOption="Default" />
        </dx:BootstrapButton>
    </div>
     
</div>
     <div style="position: relative; margin-top: 10px;">
     <dx:BootstrapCheckBox ID="cbTarih" ClientInstanceName="ClientCheckBox" runat="server" Text="Tarih Filtresi Kullanılsın">

     </dx:BootstrapCheckBox>
 </div>


    <!-- Gridview -->
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="true" Width="100%" 
        KeyFieldName="FATIRS_NO" Theme="MaterialCompact" OnPageIndexChanged="ASPxGridView1_PageIndexChanged" >
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
    <dx:ASPxPopupControl ID="popupFaturaDetay" runat="server" Width="700px" Height="400px" ShowHeader="true" Modal="true" CloseAction="CloseButton">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <dx:ASPxGridView ID="gridFaturaDetay" runat="server" Width="100%" AutoGenerateColumns="true">
            </dx:ASPxGridView>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
</asp:Content>
