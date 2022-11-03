using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.DbModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Marcaj.Pages.Tables;
using Xamarin.Essentials;
using Marcaj.Models.LocalDbModels;
using Marcaj.Models.CustomModels;
using System.Collections.ObjectModel;

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
        List<DineInTableModel> dineIns;
        List<DineInTableGroupModel> dineInGroups;
        ObservableCollection<TableLayoutModel> tblLayout;
        public AllTables(EmployeeFileModel emplFl)
        {
            InitializeComponent();

            EmplFl = emplFl;
           
            dineIn= new DineInTableModel();
            ldineIn = new LDineInTablesModel();
            dineInGroups = new List<DineInTableGroupModel>();
            dineIns = new List<DineInTableModel>();
            PopList(GroupId);
            MessagingCenter.Subscribe<NotActiveTable>(this, "Up", (sender) =>
            {
                PopList(GroupId);
            });
            MessagingCenter.Subscribe<App>(this, "ConOk", async (sender) =>
            {
               // SyncPage();
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
            tblLayout = new ObservableCollection<TableLayoutModel>();


            if (IsFirstLoad == true)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var b = await App.manager.iGetDineInTableGroups();
                    dineInGroups = b;

                    if (b != null)
                    {
                        lstvwGrupMese.ItemsSource = b;
                        lstvwGrupMese.SelectedItem = b[0];
                    }

                    var dineInss = await App.manager.iGetDineInTablesByTableGroup(GroupID);
                    if (dineInss != null)
                    {

                        var dineInGroup = dineInGroups.Where(x => x.TableGroupID == GroupID).FirstOrDefault();

                        int col = Convert.ToInt32(dineInGroup.GridSize.ToString().Split('x')[0]);
                        int row = Convert.ToInt32(dineInGroup.GridSize.ToString().Split('x')[1]);
                        int nrPos = col * row;

                        for (int i = 0; i < nrPos; i++)
                        {
                            var model = new TableLayoutModel();

                            model.Position = (i + 1).ToString();
                            model.Text = "";
                            model.Visible = false;

                            tblLayout.Add(model);
                        }
                        tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical)
                        {
                            VerticalItemSpacing = 5,
                            HorizontalItemSpacing = 5

                        };

                        

                        foreach (var dine in dineInss)
                        {
                            if (dine.DineIn.DisplayPosition != null)
                            {
                                if (Convert.ToInt32(dine.DineIn.DisplayPosition) <= nrPos)
                                {
                                    Debug.WriteLine(dine.Opened);
                                    Debug.WriteLine(dine.EmpName);

                                    if (dine.DineIn.MaxGuests == 2)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                    else if (dine.DineIn.MaxGuests == 4)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                    else if (dine.DineIn.MaxGuests == 6)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                    else if (dine.DineIn.MaxGuests == 8)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                }
                            }
                        }
                        tblLayoutColl.ItemsSource = tblLayout;
                        IsFirstLoad = false;
                    }
                    
                    
                }
                else
                {
                    var a =  App.lDatabase.lGetDineInTablesEmpNameByGroupID(GroupID);
                    if(a != null)
                    {
                        tblLayoutColl.ItemsSource = tblLayout;
                    }
                    
                    var b = await App.lDatabase.lGetDineInTableGroups();

                    if (b != null)
                    {
                        lstvwGrupMese.ItemsSource = b;
                    }
                }
                
            }
            else
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var dineInss = await App.manager.iGetDineInTablesByTableGroup(GroupID);
                    if (dineInss != null)
                    {

                        var dineInGroup = dineInGroups.Where(x => x.TableGroupID == GroupID).FirstOrDefault();

                        int col = Convert.ToInt32(dineInGroup.GridSize.ToString().Split('x')[0]);
                        int row = Convert.ToInt32(dineInGroup.GridSize.ToString().Split('x')[1]);
                        int nrPos = col * row;

                        for (int i = 0; i < nrPos; i++)
                        {
                            var model = new TableLayoutModel();

                            model.Position = (i + 1).ToString();
                            model.Text = "";
                            model.Visible = false;

                            tblLayout.Add(model);
                        }
                        tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical)
                        {
                            VerticalItemSpacing = 5,
                            HorizontalItemSpacing = 5

                        };



                        foreach (var dine in dineInss)
                        {
                            if (dine.DineIn.DisplayPosition != null)
                            {
                                if (Convert.ToInt32(dine.DineIn.DisplayPosition) <= nrPos)
                                {
                                    Debug.WriteLine(dine.Opened);
                                    Debug.WriteLine(dine.EmpName);

                                    if (dine.DineIn.MaxGuests == 2)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                    else if (dine.DineIn.MaxGuests == 4)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                    else if (dine.DineIn.MaxGuests == 6)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                    else if (dine.DineIn.MaxGuests == 8)
                                    {
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                    }
                                }
                            }
                        }
                        tblLayoutColl.ItemsSource = tblLayout;
                    }
                }
                else
                {
                    var a =  App.lDatabase.lGetDineInTablesEmpNameByGroupID(GroupID);
                    if(a != null)
                    {
                        tblLayoutColl.ItemsSource = tblLayout;
                    }
                }
                
            }


        }

        private async void lstvwMese_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (tblLayoutColl.SelectedItem != null)
                {
                    var a = e.CurrentSelection as DineInTableAndEmpModel;
                    dineIn = a.DineIn;
                    if (a.Opened == true)
                    {
                        await Navigation.PushAsync(new ActiveTable(dineIn, EmplFl));
                    }
                    else
                    {
                        await Navigation.PushAsync(new NotActiveTable(dineIn, EmplFl));
                    }
                }
                tblLayoutColl.SelectedItem = null;
            }
            else
            {
                
                if(tblLayoutColl.SelectedItem != null)
                {
                    var a = e.CurrentSelection as LDineInTableAndEmpModel;

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
                    tblLayoutColl.SelectedItem = null;
                }
            }

        }

        private void lstvwGrupMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(IsFirstLoad != true)
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


    private async void btnMese_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllTables(EmplFl));
        }

        private async void btnAchita_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchitaPage(EmplFl));
        }

        private async void btnTblLayoutPage_Clicked(object sender, EventArgs e)
        {
           
        }
    }
}