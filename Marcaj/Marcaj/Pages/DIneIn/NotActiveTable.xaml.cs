using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.DbModels;
using Marcaj.Models.LocalDbModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Tables
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotActiveTable : ContentPage
    {
        EmployeeFileModel EmpFile;
        DineInTableModel DineIn;
        int GroupId = 0;
        bool IsFirstLoad = true;
        List<OrderTransactionsModel> orderTraList;
        List<LOrderTransactionsModel> lorderTraList;
        LStationSettingsModel StationModel;
        string _Type;
        public NotActiveTable(DineInTableModel dineIn, EmployeeFileModel empFile, string type)
        {
            InitializeComponent();
            _Type = type;
            lorderTraList = new List<LOrderTransactionsModel>();
            orderTraList = new List<OrderTransactionsModel>();
            EmpFile = empFile;
            DineIn = dineIn;
            PopList(IsFirstLoad, GroupId);
        }

        async void PopList(bool isFirstLoad, int groupId)
        {
            if (isFirstLoad == true)
            {
                string deviceName = DeviceInfo.Name;
                var station = await App.lDatabase.lGetStationSettings(deviceName);
                StationModel = station;
                txtServer.Text = "Server: " + EmpFile.FirstName;
                txtStation.Text = "Station: " + station.ComputerName;
                txtOrderName.Text = "Order: NewOrder";
                txtTableName.Text = "Table: " + DineIn.DineInTableText;
                txtDateTimeOpenedTable.Text = DateTime.Now.ToString();
                txtAmountDue.Text = "Amount Due: 0";
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                   
                    var a = await App.manager.iGetMenuGroups();
                    if (a != null)
                    {
                        if (a.Count > 0)
                        {
                            lstvwMenuGroups.ItemsSource = a;
                            lstvwMenuGroups.SelectedItem = a[0];
                            var b = await App.manager.iGetMenuItemsByGroupID(a[0].MenuGroupID);
                            if (b != null)
                            {
                                lstvwMenuItems.ItemsSource = b;
                            }
                        }
                    }
                }
                else
                {
                    var a = await App.lDatabase.lGetMenuGroups();
                    if(a!= null)
                    {
                        if(a.Count>0)
                        {
                            lstvwMenuGroups.ItemsSource = a;
                            lstvwMenuGroups.SelectedItem = a[0];
                            var b = await App.lDatabase.lGetMenuItemsByGroupID(a[0].MenuGroupID);
                            if(b!= null)
                            {
                                lstvwMenuItems.ItemsSource = b;
                            }
                        }
                    }
                }
                IsFirstLoad = false; 
            }
            else
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var b = await App.manager.iGetMenuItemsByGroupID(groupId);
                    if (b != null)
                    {
                        lstvwMenuItems.ItemsSource = b;
                    }
                }
                else
                {
                    var b = await App.lDatabase.lGetMenuItemsByGroupID(groupId);
                    if(b!= null)
                    {
                        lstvwMenuItems.ItemsSource = b;
                    }
                }
            }
        }
        private void lstvwMenuGroups_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (IsFirstLoad != true)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {

                    var selIt = e.SelectedItem as MenuGroupsModel;
                    IsFirstLoad = false;
                    PopList(IsFirstLoad, selIt.MenuGroupID);
                }
                else
                {
                    var selIt = e.SelectedItem as LMenuGroupsModel;
                    IsFirstLoad = false;
                    PopList(IsFirstLoad, selIt.MenuGroupID);
                }
            }
        }

        private void lstvwMenuItems_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var selIt = e.SelectedItem as MenuItemsModel;
                    var orderTra = new OrderTransactionsModel();

                    var exOrderTra = orderTraList.FirstOrDefault(x => x.MenuItemID == selIt.MenuItemID);
                    if (exOrderTra == null)
                    {
                        orderTra = new OrderTransactionsModel();
                        orderTra.MenuItemID = selIt.MenuItemID;
                        orderTra.MenuItemTextOT = selIt.MenuItemText;
                        orderTra.Quantity = 1;
                        orderTra.MenuItemUnitPrice = selIt.DefaultUnitPrice;
                        orderTra.ExtendedPrice = selIt.DefaultUnitPrice;
                        orderTraList.Add(orderTra);
                    }
                    else
                    {
                        exOrderTra.Quantity = exOrderTra.Quantity + 1;
                        exOrderTra.ExtendedPrice = exOrderTra.MenuItemUnitPrice * exOrderTra.Quantity;
                    }

                    lstvwOrderTransactions.ItemsSource = null;
                    lstvwOrderTransactions.ItemsSource = orderTraList;
                    float extPrice = 0;
                    foreach (var orderTr in orderTraList)
                    {
                        extPrice += orderTr.ExtendedPrice;
                    }
                    txtAmountDue.Text = "Amount Due: " + extPrice.ToString();
                }
                else
                {
                    var selIt = e.SelectedItem as LMenuItemsModel;
                    var orderTra = new LOrderTransactionsModel();

                    var exOrderTra = lorderTraList.FirstOrDefault(x => x.MenuItemID == selIt.MenuItemID);
                    if (exOrderTra == null)
                    {
                        orderTra = new LOrderTransactionsModel();
                        orderTra.MenuItemID = selIt.MenuItemID;
                        orderTra.MenuItemTextOT = selIt.MenuItemText;
                        orderTra.Quantity = 1;
                        orderTra.MenuItemUnitPrice = selIt.DefaultUnitPrice;
                        orderTra.ExtendedPrice = selIt.DefaultUnitPrice;
                        lorderTraList.Add(orderTra);
                    }
                    else
                    {
                        exOrderTra.Quantity = exOrderTra.Quantity + 1;
                        exOrderTra.ExtendedPrice = exOrderTra.MenuItemUnitPrice * exOrderTra.Quantity;
                    }

                    lstvwOrderTransactions.ItemsSource = null;
                    lstvwOrderTransactions.ItemsSource = lorderTraList;
                    float extPrice = 0;
                    foreach (var orderTr in lorderTraList)
                    {
                        extPrice += orderTr.ExtendedPrice;
                    }
                    txtAmountDue.Text = "Amount Due: " + extPrice.ToString();
                }
                    
            }

            lstvwMenuItems.SelectedItem = null;
        }

        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if(orderTraList.Count>0)
                {
                  
                    var ordHd = new OrderHeadersModel();
                    var stat = await App.manager.iGetStationSettings(DeviceInfo.Name);
                    ordHd.OrderDateTime = DateTime.Now;
                    ordHd.EmployeeID = EmpFile.EmployeeID;
                    ordHd.StationID = stat.StationID;
                    ordHd.OrderType = "1";
                    ordHd.DineInTableID = DineIn.DineInTableID;
                    ordHd.OrderStatus = "1";
                    ordHd.AmountDue = Convert.ToSingle(txtAmountDue.Text.Split(' ')[2]);
                    ordHd.Kitchen1AlreadyPrinted = false;
                    ordHd.Kitchen2AlreadyPrinted = false;
                    ordHd.Kitchen3AlreadyPrinted = false;
                    ordHd.BarAlreadyPrinted = false;
                    ordHd.SynchVer = DateTime.MinValue;
                    ordHd.EditTimestamp = DateTime.Now;
                    ordHd.PackagerAlreadyPrinted = false;
                    ordHd.SalesTaxAmountUsed = 0;
                    ordHd.GuestCheckPrinted = false;
                    ordHd.SalesTaxRate = 0;
                    ordHd.SubTotal = ordHd.AmountDue;
                    ordHd.RowGUID = Guid.NewGuid().ToString();

                    Debug.WriteLine(ordHd.OrderDateTime);
                    Debug.WriteLine(ordHd.EmployeeID);
                    Debug.WriteLine(ordHd.StationID);
                    Debug.WriteLine(ordHd.DineInTableID);
                    Debug.WriteLine(ordHd.RowGUID);
                    await App.manager.iPostOrderHeader(ordHd);

                    var ordTraListToPost = new List<OrderTransactionsModel>();

                    foreach (var ordTraItem in orderTraList)
                    {
                        var ordTra = new OrderTransactionsModel();

                        ordTra.MenuItemID = ordTraItem.MenuItemID;
                        ordTra.MenuItemUnitPrice = ordTraItem.MenuItemUnitPrice;
                        ordTra.Quantity = ordTraItem.Quantity;
                        ordTra.ExtendedPrice = ordTraItem.ExtendedPrice;
                        ordTra.DiscountTaxable = false;
                        ordTra.TransactionStatus = "1";
                        ordTra.NotificationStatus = "2";
                        ordTra.RowGUID = Guid.NewGuid().ToString();
                        ordTra.MenuItemTextOT = ordTraItem.MenuItemTextOT;
                        ordTraListToPost.Add(ordTra);
                    }

                    await App.manager.iPostOrderTransactionNotActive(ordTraListToPost, DineIn.DineInTableID);
                    //DineIn.DineInTableInActive = true;
                    //await App.manager.iPutDineInTable(DineIn, DineIn.DineInTableID);

                    MessagingCenter.Send<NotActiveTable>(this, "Up");
                }

                
            }
           else
            {
                if(lorderTraList.Count>0)
                {
                    var ordHd = new LOrderHeaderModel();
                    var id = await App.lDatabase.lGetLastIdOrderHeaders();
                    Debug.WriteLine(id.OrderID);
                    ordHd.OrderID = id.OrderID + 1;
                    ordHd.OrderDateTime = DateTime.Now;
                    ordHd.EmployeeID = EmpFile.EmployeeID;
                    ordHd.StationID = StationModel.StationID;
                    ordHd.OrderType = "1";
                    ordHd.DineInTableID = DineIn.DineInTableID;
                    ordHd.OrderStatus = "1";
                    ordHd.AmountDue = Convert.ToSingle(txtAmountDue.Text.Split(' ')[2]);
                    ordHd.Kitchen1AlreadyPrinted = false;
                    ordHd.Kitchen2AlreadyPrinted = false;
                    ordHd.Kitchen3AlreadyPrinted = false;
                    ordHd.BarAlreadyPrinted = false;
                    ordHd.PackagerAlreadyPrinted = false;
                    ordHd.SalesTaxAmountUsed = 0;
                    ordHd.GuestCheckPrinted = false;
                    ordHd.SalesTaxRate = 0;
                    ordHd.SubTotal = ordHd.AmountDue;
                    ordHd.RowGUID = Guid.NewGuid().ToString();
                    await App.lDatabase.lPostOrderHeader(ordHd);

                    var ordTraListToPost = new List<LOrderTransactionsModel>();

                    foreach (var ordTraItem in lorderTraList)
                    {
                        var ordTra = new LOrderTransactionsModel();

                        ordTra.MenuItemID = ordTraItem.MenuItemID;
                        ordTra.MenuItemUnitPrice = ordTraItem.MenuItemUnitPrice;
                        ordTra.Quantity = ordTraItem.Quantity;
                        ordTra.ExtendedPrice = ordTraItem.ExtendedPrice;
                        ordTra.DiscountTaxable = false;
                        ordTra.TransactionStatus = "1";
                        ordTra.NotificationStatus = "2";
                        ordTra.RowGUID = Guid.NewGuid().ToString();
                        ordTra.MenuItemTextOT = ordTraItem.MenuItemTextOT;
                        ordTraListToPost.Add(ordTra);
                    }

                    await App.lDatabase.lPostOrderTransactionNotActiveTable(ordTraListToPost, DineIn.DineInTableID);
                    DineIn.DineInTableInActive = true;
                    LDineInTablesModel dineIn = new LDineInTablesModel();
                    dineIn.DineInTableText = DineIn.DineInTableText;
                    dineIn.DineInTableID = DineIn.DineInTableID;
                    dineIn.TableGroupID = DineIn.TableGroupID;
                    //dineIn.DineInTableInActive = DineIn.DineInTableInActive;
                    //await App.lDatabase.lPutDineInTable(dineIn);

                    // MessagingCenter.Send<NotActiveTable>(this, "Up");
                }
                
            }

            if (_Type == "opened")
            {
               
                int BackCount = 2;
                for (var counter = 1; counter < BackCount; counter++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
                await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            if(_Type == "opened")
            {
                int BackCount = 2;
                for (var counter = 1; counter < BackCount; counter++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
                await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
    }
}