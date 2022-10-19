using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.DbModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Marcaj.Pages.Tables;
using Marcaj.Models.LocalDbModels;
using Xamarin.Essentials;
using System.Diagnostics;
using Marcaj.Pages.Settings;
using Marcaj.Pages;

namespace Marcaj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        EmployeeFileModel EmplFl;
        public HomePage(EmployeeFileModel emplFl)
        {
            InitializeComponent();
            EmplFl = emplFl;
            PopPage();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                SyncLocal();
            }
        }

        async void PopPage()
        {
            string deviceName = DeviceInfo.Name;

            var a = await App.lDatabase.lGetStationSettings(deviceName);

            StationName.Text = "Station: "+ a.ComputerName;
            EmployeeName.Text = "Employee: " + EmplFl.FirstName;
        }
        async void SyncLocal()
        {

                //Sync TableGroups
                Debug.WriteLine("Sync TableGroups");
                var dineInTableGroupsAz = await App.manager.iGetDineInTableGroups();
                await App.lDatabase.lDeleteDineInTableGroups();
                foreach (var group in dineInTableGroupsAz)
                {
                    var lgroup = new LDineInTableGroupsModel();

                    lgroup.TableGroupText = group.TableGroupText;
                    lgroup.TableGroupID = group.TableGroupID;

                    await App.lDatabase.lPostDineInTableGroups(lgroup);
                }

                //Sync DineInTables
                Debug.WriteLine("Sync DineInTables");

                var dineInTablesAz = await App.manager.iGetDineInTables();
                await App.lDatabase.lDeleteDineInTables();
                foreach (var table in dineInTablesAz)
                {
                    var ltable = new LDineInTablesModel();

                    ltable.DineInTableText = table.DineInTableText;
                    ltable.DineInTableID = table.DineInTableID;
                    ltable.TableGroupID = table.TableGroupID;
                    ltable.DineInTableInActive = table.DineInTableInActive;

                    await App.lDatabase.lPostDineInTables(ltable);
                }

                //Sync OrderHeaders
                Debug.WriteLine("Sync OrderHeaders");

                var oheadersTableAz = await App.manager.iGetOrderHeadersSync();
                await App.lDatabase.lDeleteOrderHeaders();
                foreach (var table in oheadersTableAz)
                {
                    var ltable = new LOrderHeaderModel();

                    ltable.OrderID = table.OrderID;
                    ltable.OrderDateTime = table.OrderDateTime;
                    ltable.EmployeeID = table.EmployeeID;
                    ltable.StationID = table.StationID;
                    ltable.OrderType = table.OrderType;
                    ltable.DineInTableID = table.DineInTableID;
                    ltable.OrderStatus = table.OrderStatus;
                    ltable.AmountDue = table.AmountDue;
                    ltable.Kitchen1AlreadyPrinted = table.Kitchen1AlreadyPrinted;
                    ltable.Kitchen2AlreadyPrinted = table.Kitchen2AlreadyPrinted;
                    ltable.Kitchen3AlreadyPrinted = table.Kitchen3AlreadyPrinted;
                    ltable.BarAlreadyPrinted = table.BarAlreadyPrinted;
                    ltable.PackagerAlreadyPrinted = table.PackagerAlreadyPrinted;
                    ltable.SalesTaxAmountUsed = table.SalesTaxAmountUsed;
                    ltable.GuestCheckPrinted = table.GuestCheckPrinted;
                    ltable.SalesTaxRate = table.SalesTaxRate;
                    ltable.SubTotal = table.SubTotal;
                    ltable.RowGUID = table.RowGUID;

                    await App.lDatabase.lPostOrderHeader(ltable);

                }

                //Sync MenuItems
                Debug.WriteLine("Sync MenuItems");

                var menuItemsTableAz = await App.manager.iGetMenuItems();
                await App.lDatabase.lDeleteMenuItems();

                foreach (var table in menuItemsTableAz)
                {
                    var ltable = new LMenuItemsModel();

                    ltable.MenuItemID = table.MenuItemID;
                    ltable.MenuItemText = table.MenuItemText;
                    ltable.MenuCategoryID = table.MenuCategoryID;
                    ltable.MenuGroupID = table.MenuGroupID;
                    ltable.DisplayIndex = table.DisplayIndex;
                    ltable.DefaultUnitPrice = table.DefaultUnitPrice;
                    ltable.MenuItemNotification = table.MenuItemNotification;
                    ltable.MenuItemInActive = table.MenuItemInActive;
                    ltable.MenuItemInStock = table.MenuItemInStock;
                    ltable.MenuItemTaxable = table.MenuItemTaxable;
                    ltable.MenuItemDiscountable = table.MenuItemDiscountable;
                    ltable.MenuItemType = table.MenuItemType;
                    ltable.HasModifierPopUps = table.HasModifierPopUps;
                    ltable.ShowCaption = table.ShowCaption;
                    ltable.GSTApplied = table.GSTApplied;
                    ltable.Pizza = table.Pizza;
                    ltable.Bar = table.Bar;
                    ltable.OrderByWeight = table.OrderByWeight;
                    ltable.PrintPizzaLabel = table.PrintPizzaLabel;

                    await App.lDatabase.lPostMenuItems(ltable);
                }

                //Sync MenuGroups
                Debug.WriteLine("Sync MenuGroups");

                var menuGroupsTableAz = await App.manager.iGetMenuGroups();
                await App.lDatabase.lDeleteMenuGroups();
                foreach (var table in menuGroupsTableAz)
                {
                    var ltable = new LMenuGroupsModel();


                    ltable.MenuGroupID = table.MenuGroupID;
                    ltable.MenuGroupText = table.MenuGroupText;
                    ltable.DisplayIndex = table.DisplayIndex;
                    ltable.MenuGroupInActive = table.MenuGroupInActive;
                    ltable.ShowCaption = table.ShowCaption;
                    ltable.RowGUID = table.RowGUID;

                    await App.lDatabase.lPostMenuGroups(ltable);
                }

                //Sync OrderHeaders
                var orderHeaderSyncTableAz = await App.manager.iGetOrderHeadersSync();

                await App.lDatabase.lDeleteOrderHeaders();
                foreach (var order in orderHeaderSyncTableAz)
                {
                    var ltable = new LOrderHeaderModel();

                    ltable.OrderID = order.OrderID;
                    ltable.OrderDateTime = order.OrderDateTime;
                    ltable.EmployeeID = order.EmployeeID;
                    ltable.StationID = order.StationID;
                    ltable.OrderType = order.OrderType;
                    ltable.DineInTableID = order.DineInTableID;
                    ltable.OrderStatus = order.OrderStatus;
                    ltable.AmountDue = order.AmountDue;
                    ltable.Kitchen1AlreadyPrinted = order.Kitchen1AlreadyPrinted;
                    ltable.Kitchen2AlreadyPrinted = order.Kitchen2AlreadyPrinted;
                    ltable.Kitchen3AlreadyPrinted = order.Kitchen3AlreadyPrinted;
                    ltable.BarAlreadyPrinted = order.BarAlreadyPrinted;
                    ltable.PackagerAlreadyPrinted = order.PackagerAlreadyPrinted;
                    ltable.SalesTaxAmountUsed = order.SalesTaxAmountUsed;
                    ltable.GuestCheckPrinted = order.GuestCheckPrinted;
                    ltable.SalesTaxRate = order.SalesTaxRate;
                    ltable.SubTotal = order.SubTotal;
                    ltable.RowGUID = order.RowGUID;

                    await App.lDatabase.lPostOrderHeader(ltable);
                }
                //Sync OrderTransactions

                Debug.WriteLine("Sync OrderTransactions");

                int[] listOrderIDList;
                List<int> listToArray = new List<int>();

                foreach (var sync in orderHeaderSyncTableAz)
                {
                    listToArray.Add(sync.OrderID);
                }
                listOrderIDList = listToArray.ToArray();

                var orderTranTableAz = await App.manager.iGetOrderTransactionsByListOfOrderIDs(listOrderIDList);
                await App.lDatabase.lDeleteOrderTransactions();
                foreach (var orderTran in orderTranTableAz)
                {
                    var ltable = new LOrderTransactionsModel();

                    ltable.OrderTransactionID = orderTran.OrderTransactionID;
                    ltable.OrderID = orderTran.OrderID;
                    ltable.MenuItemID = orderTran.MenuItemID;
                    ltable.MenuItemUnitPrice = orderTran.MenuItemUnitPrice;
                    ltable.Quantity = orderTran.Quantity;
                    ltable.ExtendedPrice = orderTran.ExtendedPrice;
                    ltable.DiscountTaxable = orderTran.DiscountTaxable;
                    ltable.TransactionStatus = orderTran.TransactionStatus;
                    ltable.NotificationStatus = orderTran.NotificationStatus;
                    ltable.RowGUID = orderTran.RowGUID;
                    ltable.MenuItemTextOT = orderTran.MenuItemTextOT;

                    await App.lDatabase.lPostOrderTransactions(ltable);
                }

        }
        private async void btnMese_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllTables(EmplFl));
        }

        private async void btnAchita_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchitaPage(EmplFl));
        }

        private async void btnSettings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsHomePage());
        }
        private async void btnGrid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Page1());
        }
    }
}