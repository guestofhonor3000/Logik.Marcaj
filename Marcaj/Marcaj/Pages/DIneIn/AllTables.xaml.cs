using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Marcaj.Models.LocalDbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Tables
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllTables : ContentPage
    {
        EmployeeFileModel EmplFl;
        int GroupId = 1;
        bool isMoving = false;
        int OrderId = 0;
        string Flag = "";
        bool IsFirstLoad = true;
        View itemsToShow;
        List<DineInTableModel> dineIns;
        List<DineInTableAndEmpModel> dineInsAndEmp;
        List<DineInTableGroupModel> dineInGroups;
        ObservableCollection<TableLayoutModel> tblLayout;
        OrderHeadersModel orderHeader;
        List<OrderHeadersModel> orderHeadersList;
        List<OrderTransactionsModel> orderTransactionsList;
        List<OrderTransactionsModel> orderTransactionsListByOrderID;


        public ObservableCollection<OptionsModel> menuBtnList;
        public AllTables(EmployeeFileModel emplFl)
        {
            InitializeComponent();

            orderHeader = new OrderHeadersModel();
            orderHeadersList = new List<OrderHeadersModel>();
            orderTransactionsList = new List<OrderTransactionsModel>();
            orderTransactionsListByOrderID = new List<OrderTransactionsModel>();

            EmplFl = emplFl;
            dineInGroups = new List<DineInTableGroupModel>();
            dineIns = new List<DineInTableModel>();
            dineInsAndEmp = new List<DineInTableAndEmpModel>();

            Clock.Text = DateTime.Now.ToString();

            PopList(GroupId, Flag);
            MessagingCenter.Subscribe<NotActiveTable>(this, "Up", (sender) =>
            {
                PopList(GroupId, Flag);
            });
            MessagingCenter.Subscribe<App>(this, "ConOk", async (sender) =>
            {
                // SyncPage();
            });
            MessagingCenter.Subscribe<ActiveTableEditPage>(this, "Up", (sender) =>
            {
                PopList(GroupId, Flag);
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

            Debug.WriteLine("LoHCountBefore: " + LOh.Count.ToString());

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
                PopList(GroupId, Flag);
                await DisplayAlert("Sync", "Database synced!", "Ok");
            });
        }


        async void PopList(int GroupID, string flag)
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
                    dineInsAndEmp = dineInss;
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
                            model.EmpName = "";
                            model.Fumatori = false;
                            model.Fereastra = false;
                            model.Cabina = false;

                            tblLayout.Add(model);
                        }
                        tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical)
                        {
                            VerticalItemSpacing = 5,
                            HorizontalItemSpacing = 5
                        };

                        foreach (var dine in dineInss)
                        {
                            string tblFlags = "";

                            if (dine.DineIn.DisplayPosition != null)
                            {
                                if (Convert.ToInt32(dine.DineIn.DisplayPosition) <= nrPos)
                                {
                                    if ((bool)dine.DineIn.Booth)
                                    {
                                        tblFlags += "b";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Cabina = true;
                                    }
                                    if ((bool)dine.DineIn.Smoking) 
                                    {   
                                        tblFlags += "s";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Fumatori = true;
                                    }
                                    if ((bool)dine.DineIn.Window)
                                    {
                                        tblFlags += "w";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Fereastra = true;
                                    }
                                    
                                    Debug.WriteLine(tblFlags);
                                    var filter = tblFlags.Contains(flag);

                                    if (dine.DineIn.MaxGuests == 2)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
                                    }
                                    else if (dine.DineIn.MaxGuests == 4)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
                                    }
                                    else if (dine.DineIn.MaxGuests == 6)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
                                    }
                                    else if (dine.DineIn.MaxGuests == 8)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
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
                    var a = App.lDatabase.lGetDineInTablesEmpNameByGroupID(GroupID);
                    if (a != null)
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
                    dineInsAndEmp = dineInss;

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
                            model.EmpName = "";
                            model.Fumatori = false;
                            model.Fereastra = false;
                            model.Cabina = false;

                            tblLayout.Add(model);
                        }
                        tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical)
                        {
                            VerticalItemSpacing = 5,
                            HorizontalItemSpacing = 5

                        };

                        foreach (var dine in dineInss)
                        {
                            string tblFlags = "";

                            if (dine.DineIn.DisplayPosition != null)
                            {
                                if (Convert.ToInt32(dine.DineIn.DisplayPosition) <= nrPos)
                                {
                                    //Debug.WriteLine(dine.Opened);
                                    //Debug.WriteLine(dine.EmpName);

                                    if ((bool)dine.DineIn.Booth)
                                    {
                                        tblFlags += "b";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Cabina = true;
                                    }
                                    if ((bool)dine.DineIn.Smoking)
                                    {
                                        tblFlags += "s";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Fumatori = true;
                                    }
                                    if ((bool)dine.DineIn.Window)
                                    {
                                        tblFlags += "w";
                                        tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Fereastra = true;
                                    }
                                    
                                    Debug.WriteLine(tblFlags);
                                    var filter = tblFlags.Contains(flag);

                                    if (dine.DineIn.MaxGuests == 2)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
                                    }
                                    else if (dine.DineIn.MaxGuests == 4)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
                                    }
                                    else if (dine.DineIn.MaxGuests == 6)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
                                    }
                                    else if (dine.DineIn.MaxGuests == 8)
                                    {
                                        if (dine.Opened)
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Occupied.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Occupied.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().EmpName = dine.EmpName;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpened = ((int)DateTime.Now.Subtract(dine.TimeOpened).TotalHours).ToString() + ":" + DateTime.Now.Subtract(dine.TimeOpened).Minutes.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (filter)
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                                            }
                                            else if (flag == "")
                                            {
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Visible = true;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TableText = dine.DineIn.DineInTableText;
                                                tblLayout.Where(x => x.Position == dine.DineIn.DisplayPosition).FirstOrDefault().TimeOpenedVisible = false;
                                            }
                                        }
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
                    var a = App.lDatabase.lGetDineInTablesEmpNameByGroupID(GroupID);
                    if (a != null)
                    {
                        tblLayoutColl.ItemsSource = tblLayout;
                    }
                }
            }
        }

        async void ShowOrders(int TableID)
        {
            gridLists.Children.Clear();
            var b = dineInsAndEmp.Where(x => x.DineIn.DineInTableID == TableID).FirstOrDefault();
            orderHeadersList = await App.manager.iGetOrderHeadersByDineInTableID(TableID);

            Button toActive = new Button
            {
                Text = "Toate",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = new Thickness(0, 20, 0, 20),
                Margin = new Thickness(2.5, 10, 2.5, 0),
            };
            toActive.SetDynamicResource(StyleProperty, "btn");
            toActive.Clicked += async (sender, args) => await Navigation.PushAsync(new ActiveTable(b.DineIn, EmplFl));

            ImageButton addOrder = new ImageButton
            {
                Source = "AddIcon.png",
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,

            };
            addOrder.Clicked += async (sender, args) => await Navigation.PushAsync(new NotActiveTable(b.DineIn, EmplFl, "opened"));

            Frame addFrame = new Frame
            {
                CornerRadius = 4,
                Margin = new Thickness(5),
                BackgroundColor = Color.FromHex("#d9d9d9"),
            };
            addFrame.Content = addOrder;

            gridLists.Children.Add(toActive);
            gridLists.Children.Add(addFrame);

            foreach (var orderHeader in orderHeadersList)
            {
                StackLayout OrderHeader = new StackLayout();

                Label Header = new Label
                {
                    Text = "#" + orderHeader.OrderID.ToString(),
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };

                Frame headerFrame = new Frame
                {
                    BackgroundColor = Color.White,
                    BorderColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(5),

                };
                headerFrame.Content = Header;

                Label txtServer = new Label
                {
                    Text = "Deschis de: " + b.EmpName,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Center
                };


                Label txtDateTimeOpenedTable = new Label
                {
                    Text = orderHeader.OrderDateTime.ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                Button showItems = new Button
                {
                    Text = "Arata",
                    Padding = new Thickness(8),
                };
                showItems.SetDynamicResource(StyleProperty, "secondBtn");

                StackLayout itemsStack = new StackLayout();

                showItems.Clicked += ShowItems_Clicked;

                async void ShowItems_Clicked(object sender, EventArgs e)
                {
                    await ShowItemsInOrder(orderHeader.OrderID);
                    OrderHeader.Children.Remove(showItems);
                    itemsStack.Children.Add(itemsToShow);
                };

                Label Footer = new Label
                {
                    Text = "Total: " + orderHeader.AmountDue.ToString(),
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                };

                Footer.SetDynamicResource(StyleProperty, "checkLabel");
                txtDateTimeOpenedTable.SetDynamicResource(StyleProperty, "checkLabel");
                txtServer.SetDynamicResource(StyleProperty, "checkLabel");
                Header.SetDynamicResource(StyleProperty, "checkLabel");

                OrderHeader.Children.Add(headerFrame);
                OrderHeader.Children.Add(txtServer);
                OrderHeader.Children.Add(txtDateTimeOpenedTable);
                OrderHeader.Children.Add(showItems);
                OrderHeader.Children.Add(itemsStack);
                OrderHeader.Children.Add(Footer);

                Grid controls = new Grid
                {
                    ColumnDefinitions =
                    {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    }
                };

                Button moveOrder = new Button
                {
                    Text = "Muta",
                    Padding = new Thickness(8),
                };
                moveOrder.SetDynamicResource(StyleProperty, "secondBtn");


                moveOrder.Clicked += (sender, args) => moveOrder_Clicked(sender, orderHeader.OrderID);

                Button toOrder = new Button
                {
                    Text = "Edit",
                    Padding = new Thickness(8),
                };
                toOrder.SetDynamicResource(StyleProperty, "secondBtn");

                orderTransactionsListByOrderID = orderTransactionsList.FindAll(x => x.OrderID == Convert.ToInt32(TableID));

                // ! No Time Sync !
                toOrder.Clicked += async (sender, args) => await Navigation.PushAsync(new ActiveTableEditPage(orderHeader, EmplFl, b.DineIn, orderTransactionsListByOrderID));
                controls.Children.Add(moveOrder, 0, 0);
                controls.Children.Add(toOrder, 1, 0);

                OrderHeader.Children.Add(controls);

                Frame checkFrame = new Frame
                {
                    CornerRadius = 4,
                    Margin = new Thickness(5),
                    BackgroundColor = Color.FromHex("#d9d9d9"),
                };

                checkFrame.AutomationId = OrderHeader.ClassId;
                checkFrame.Content = OrderHeader;
                OrderHeader.ClassId = orderHeader.OrderID.ToString();

                gridLists.Children.Add(checkFrame);
            }
        }

        async Task<View> ShowItemsInOrder(int OrderId)
        {
            var orderTraItems = await App.manager.iGetOrderTransactionsByOrderID(OrderId);

            Xamarin.Forms.ListView itemsList = new Xamarin.Forms.ListView();
            itemsList.ItemsSource = orderTraItems;
            itemsList.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

            itemsList.ItemTemplate = new DataTemplate(() =>
            {
                ViewCell view = new ViewCell();

                Grid grid = new Grid();

                grid.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition{Width=GridLength.Star},
                        new ColumnDefinition{Width=GridLength.Star},
                        new ColumnDefinition{Width=GridLength.Star}
                    };

                Label qtyLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Start,
                };
                qtyLabel.SetDynamicResource(StyleProperty, "checkLabel");
                qtyLabel.SetBinding(Label.TextProperty, "Quantity");
                grid.Children.Add(qtyLabel, 0, 0);


                Label itemLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                };
                itemLabel.SetDynamicResource(StyleProperty, "checkLabel");
                itemLabel.SetBinding(Label.TextProperty, "MenuItemTextOT");
                grid.Children.Add(itemLabel, 1, 0);


                Label extPriceLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.End,
                    VerticalTextAlignment = TextAlignment.Start,
                };
                extPriceLabel.SetDynamicResource(StyleProperty, "checkLabel");
                extPriceLabel.SetBinding(Label.TextProperty, "ExtendedPrice");
                grid.Children.Add(extPriceLabel, 2, 0);


                view.View = grid;
                return view;
            });

            itemsToShow = itemsList;
            return itemsList;
        }

        private void lstvwGrupMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (IsFirstLoad != true)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var selIt = e.SelectedItem as DineInTableGroupModel;
                    GroupId = selIt.TableGroupID;
                    IsFirstLoad = false;
                    PopList(GroupId, Flag);
                }
                else
                {
                    var selIt = e.SelectedItem as LDineInTableGroupsModel;
                    GroupId = selIt.TableGroupID;
                    IsFirstLoad = false;
                    PopList(GroupId, Flag);
                }
            }

        }


        async void ImageButton_Clicked(object sender, EventArgs e)
        {
            var a = sender as ImageButton;

            var b = dineInsAndEmp.Where(x => x.DineIn.DineInTableText == a.AutomationId).FirstOrDefault();

            if (isMoving)
            {
                var c = await App.manager.iGetDineInTables();
                dineIns = c;
                var table = dineIns.Where(x => x.DineInTableID == b.DineIn.DineInTableID).FirstOrDefault();
                var headerToMove = orderHeadersList.Where(x => x.OrderID == OrderId).FirstOrDefault();
                headerToMove.DineInTableID = table.DineInTableID;
                await App.manager.iPutOrderHeadersDineInTableId(headerToMove, headerToMove.OrderID);
                isMoving = false;

                ShowOrders(table.DineInTableID);
                PopList(GroupId, Flag);
            }
            else
            {
                if (b.Opened)
                {
                    var Id = b.DineIn.DineInTableID;
                    ShowOrders(Id);
                }
                else
                {
                    await Navigation.PushAsync(new NotActiveTable(b.DineIn, EmplFl, "closed"));
                }
            }
        }

        private void moveOrder_Clicked(object sender, int id)
        {
            isMoving = true;
            OrderId = id;
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(EmplFl));
        }

        private void smokingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (smokingSwitch.IsToggled)
            {
                Flag += 's';
                Flag = OrderFlag(Flag);
            }
            else if (smokingSwitch.IsToggled == false && IsFirstLoad == false)
            {
                char a = 's';
                Flag = Flag.Trim(a);
                Flag = OrderFlag(Flag);
            }
            Debug.WriteLine(Flag);
            PopList(GroupId, Flag);
        }

        private void windowSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (windowSwitch.IsToggled)
            {
                Flag += 'w';
                Flag = OrderFlag(Flag);
            }
            else if (windowSwitch.IsToggled == false && IsFirstLoad == false)
            {
                char a = 'w';
                Flag = Flag.Trim(a);
                Flag = OrderFlag(Flag);
            }
            Debug.WriteLine(Flag);
            PopList(GroupId, Flag);
        }

        private void boothSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (boothSwitch.IsToggled)
            {
                Flag += 'b';
                Flag = OrderFlag(Flag);
            }
            else if(boothSwitch.IsToggled == false && IsFirstLoad == false)
            {
                char a = 'b';
                Flag = Flag.Trim(a);
                Flag = OrderFlag(Flag);
            }
            Debug.WriteLine(Flag);
            PopList(GroupId, Flag);
        }

        static string OrderFlag(string flag)
        {
            char[] flags = flag.ToArray();
            Array.Sort(flags);
            return new string(flags);
        }
    }

}

