using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.DbModels;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Marcaj.Pages.Tables;
using Xamarin.Essentials;
using Marcaj.Models.LocalDbModels;
using Marcaj.Models.CustomModels;

namespace Marcaj.Pages.Tables
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllTables : ContentPage
    {
        EmployeeFileModel EmplFl;
        int GroupId = 1;
        bool IsFirstLoad = true;
        DineInTableModel dineIn;
        LDineInTablesModel ldineIn;
        public AllTables(EmployeeFileModel emplFl)
        {
            InitializeComponent();

            EmplFl = emplFl;
            PopList(GroupId);
            dineIn= new DineInTableModel();
            ldineIn = new LDineInTablesModel();

            MessagingCenter.Subscribe<NotActiveTable>(this, "Up", (sender) =>
            {
                PopList(GroupId);
            });
            MessagingCenter.Subscribe<App>(this, "ConOk", async (sender) =>
            {
                SyncPage();
            });
        }

        async void SyncPage()
        {
            //Employee Sync
            var AzEmpLastId = await App.manager.iGetLastIdEmployeeFiles();
            var LEmp = await App.lDatabase.lGetLastIdEmployeeFiles();
            var LEmplLastId = LEmp.EmployeeID;
            if (AzEmpLastId < LEmplLastId)
            {
                var LEmpList = await App.lDatabase.lGetAllEmployeeFiles();
                var PostList = LEmpList.Where(x => x.EmployeeID > AzEmpLastId);

                var AzPostList = new List<EmployeeFileModel>();

                foreach (var postItem in PostList)
                {
                    var model = new EmployeeFileModel();

                    model.EmployeeID = postItem.EmployeeID;
                    model.FirstName = postItem.FirstName;
                    model.LastName = postItem.LastName;
                    model.AccessCode = postItem.AccessCode;
                    model.JobTitleID = 1;
                    model.SecurityLevel = "5";
                    model.PayBasis = "1";
                    model.PayRate = 0;
                    model.PrefUserInterfaceLocale = "3";
                    model.RowGUID = Guid.NewGuid().ToString();
                    model.ScanCode = "2";

                    AzPostList.Add(model);
                }

                await App.manager.iPostEmployeeFiles(AzPostList);
            }

            //DineInTables Sync
            var AzDineInTables = await App.manager.iGetDineInTables();
            var LDineInTables = await App.lDatabase.lGetDineInTables();

            foreach (var lDineIn in LDineInTables)
            {
                foreach (var azDineIn in AzDineInTables)
                {
                    if (lDineIn.DineInTableID == azDineIn.DineInTableID)
                    {
                        if (lDineIn.DineInTableInActive != azDineIn.DineInTableInActive)
                        {
                            azDineIn.DineInTableInActive = lDineIn.DineInTableInActive;
                            await App.manager.iPutDineInTable(azDineIn, azDineIn.DineInTableID);
                        }
                    }
                }
            }
            var newIds = LDineInTables.Where(x => x.DineInTableID > AzDineInTables.OrderByDescending(y => y.DineInTableID).FirstOrDefault().DineInTableID);
            if (newIds.Count() > 0)
            {
                foreach (var lNewDineIn in newIds)
                {
                    var model = new DineInTableModel();

                    model.DineInTableText = lNewDineIn.DineInTableText;
                    model.TableGroupID = lNewDineIn.TableGroupID;
                    model.DineInTableInActive = lNewDineIn.DineInTableInActive;
                    model.DisplayIndex = 0;
                    model.RowGUID = Guid.NewGuid().ToString();

                    await App.manager.iPostDineInTable(model);
                }
            }

            //OrderHeaders Sync
            var AzOh = await App.manager.iGetOrderHeadersSync();
            var LOh = await App.lDatabase.lGetOrderHeaders();
            var idsIn = new List<int>();
            foreach (var order in LOh)
            {
                foreach (var azOh in AzOh)
                {
                    if (order.OrderID == azOh.OrderID)
                    {
                        var ltable = new OrderHeadersModel();

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
                        Debug.WriteLine(order.OrderID);
                        await App.manager.iPutOrderHeaders(ltable, order.OrderID);
                        idsIn.Add(order.OrderID);
                    }
                }
            }

            Debug.WriteLine("LoHCountBefore: "+LOh.Count.ToString());

            foreach (var i in idsIn)
            {
                var toDel = LOh.Where(x => x.OrderID == i).FirstOrDefault();
                LOh.Remove(toDel);
            }
            Debug.WriteLine("LoHCountAfter: " + LOh.Count.ToString());

            int index = 0;
            foreach (var order in LOh)
            {
                var ltable = new OrderHeadersModel();
                index++;
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

                await App.manager.iPostOrderHeader(ltable);

            }

            var idsInArr = idsIn.ToArray();
            //var AzOt = await App.manager.iGetOrderTransactionsByListOfOrderIDs(idsInArr);
            var LOt = await App.lDatabase.lGetOrderTransactions();
            var lstOT = new List<OrderTransactionsModel>();

            foreach (var orderTran in LOt)
            {
                var ltable = new OrderTransactionsModel();

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

                lstOT.Add(ltable);
               
            }
            await App.manager.iPostOrderTransactionSync(lstOT);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                PopList(GroupId);
                await DisplayAlert("Sync", "Database synced!", "Ok");
            });
        }
        async void PopList(int GroupID)
        {
            if (IsFirstLoad == true)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var a = await App.manager.iGetDineInTablesByTableGroup(GroupID);
                    if (a != null)
                    {
                        lstvwMese.ItemsSource = a;
                    }
                    var b = await App.manager.iGetDineInTableGroups();
                    if (b != null)
                    {
                        lstvwGrupMese.ItemsSource = b;
                        lstvwGrupMese.SelectedItem = b[0];
                    }
                }
                else
                {
                    var a =  App.lDatabase.lGetDineInTablesEmpNameByGroupID(GroupID);
                    if(a != null)
                    {
                        lstvwMese.ItemsSource = a;
                    }
                    var b = await App.lDatabase.lGetDineInTableGroups();
                    if(b != null)
                    {
                        lstvwGrupMese.ItemsSource = b;
                        lstvwGrupMese.SelectedItem = b[0];
                    }
                }
                
            }
            else
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var a = await App.manager.iGetDineInTablesByTableGroup(GroupID);
                    if (a != null)
                    {
                        lstvwMese.ItemsSource = a;
                    }
                }
                else
                {
                    var a =  App.lDatabase.lGetDineInTablesEmpNameByGroupID(GroupID);
                    if(a != null)
                    {
                        lstvwMese.ItemsSource = a;
                    }
                }
                
            }
            
        }

        private async void lstvwMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (lstvwMese.SelectedItem != null)
                {
                    var a = e.SelectedItem as DineInTableAndEmpModel;
                    dineIn = a.DineIn;
                    if (dineIn.DineInTableInActive == true)
                    {
                        await Navigation.PushAsync(new ActiveTable(dineIn, EmplFl));
                    }
                    else
                    {
                        await Navigation.PushAsync(new NotActiveTable(dineIn, EmplFl));
                    }
                }
                lstvwMese.SelectedItem = null;
            }
            else
            {
                
                if(lstvwMese.SelectedItem != null)
                {
                    var a = e.SelectedItem as LDineInTableAndEmpModel;

                    ldineIn = a.DineIn;

                    dineIn.DineInTableID = ldineIn.DineInTableID;
                    dineIn.DineInTableText = ldineIn.DineInTableText;
                    dineIn.TableGroupID = ldineIn.TableGroupID;
                    dineIn.DineInTableInActive = ldineIn.DineInTableInActive;

                    if (dineIn.DineInTableInActive == true)
                    {
                        await Navigation.PushAsync(new ActiveTable(dineIn, EmplFl));
                    }
                    else
                    {
                        await Navigation.PushAsync(new NotActiveTable(dineIn, EmplFl));
                    }
                    lstvwMese.SelectedItem = null;
                }
            }

        }

        private void lstvwGrupMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var selIt = e.SelectedItem as DineInTableGroupModel;
                GroupId = selIt.TableGroupID;
                IsFirstLoad = false;
                PopList(GroupId);
            }
            else
            {
                var selIt = e.SelectedItem as LDineInTableGroupsModel;
                GroupId = selIt.TableGroupID;
                IsFirstLoad = false;
                PopList(GroupId);
            }
        }
    }
}