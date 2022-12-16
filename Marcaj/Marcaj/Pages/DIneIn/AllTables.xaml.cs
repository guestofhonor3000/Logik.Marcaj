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
        int GroupId;
        bool isMoving;
        int OrderId;
        string Flag;
        bool showingGroups;
        bool IsFirstLoad = true;
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

            GroupId = 1;
            isMoving = false;
            OrderId = 0;
            Flag = "";
            showingGroups = true;

            PopList(GroupId, Flag);

            Debug.WriteLine(IsFirstLoad);
            Debug.WriteLine(GroupId);

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
                        Debug.WriteLine("group sounce no null");
                        lstvwGrupMese.ItemsSource = b;
                        lstvwGrupMese.SelectedItem = b[0];
                    }

                    switchLayout();

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
                        lstvwGrupMese.SelectedItem = b[0];
                    }
                    switchLayout();
                }
                IsFirstLoad = false;
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
                                    try
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
                                    }
                                   catch
                                    {

                                    }

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

        private void lstvwGrupMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (IsFirstLoad == false)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var selIt = e.SelectedItem as DineInTableGroupModel;
                    GroupId = selIt.TableGroupID;
                    Debug.WriteLine("Net");
                    Debug.WriteLine(GroupId);
                    Debug.WriteLine(Flag);
                    PopList(GroupId, Flag);
                }
                else
                {
                    var selIt = e.SelectedItem as LDineInTableGroupsModel;
                    GroupId = selIt.TableGroupID;
                    Debug.WriteLine("Local");
                    Debug.WriteLine(GroupId);
                    Debug.WriteLine(Flag);
                    PopList(GroupId, Flag);
                }
            }
        }

        async void ShowOrders(int TableID)
        {
            var index = 0;

            if (showingGroups == true)
            {
                showingGroups = false;
                switchLayout();
            }

            tblOrders.Children.Clear();
            gridLists.Children.Clear();

            var b = dineInsAndEmp.Where(x => x.DineIn.DineInTableID == TableID).FirstOrDefault();
            orderHeadersList = await App.manager.iGetOrderHeadersByDineInTableID(TableID);

            Button toActive = new Button
            {
                Text = "Toate",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = new Thickness(8)
            };
            toActive.SetDynamicResource(StyleProperty, "btn");
            toActive.Clicked += async (sender, args) => await Navigation.PushAsync(new ActiveTable(b.DineIn, EmplFl));

            ImageButton addOrder = new ImageButton
            {
                Source = "AddButton.Large.png",
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Aspect = Aspect.AspectFill
            };
            addOrder.Clicked += async (sender, args) => await Navigation.PushAsync(new NotActiveTable(b.DineIn, EmplFl, "opened"));

            Frame addFrame = new Frame
            {
                CornerRadius = 4,
                Margin = new Thickness(5),
                BackgroundColor = Color.FromHex("#d9d9d9"),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill,
            };
            addFrame.Content = addOrder;

            tblOrders.Children.Insert( 0, toActive);
            tblOrders.Children.Insert( 1, addFrame);

            foreach (var orderHeader in orderHeadersList)
            {
                index++;
                gridLists = new StackLayout();
                gridLists.Padding = new Thickness(5);

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
                    Padding = new Thickness(10),

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
                    Margin = new Thickness(0)
                };
                showItems.SetDynamicResource(StyleProperty, "secondBtn");

                Button hideItems = new Button
                {
                    Text = "Ascunde",
                    Padding = new Thickness(8),
                    Margin = new Thickness(0)
                };
                hideItems.SetDynamicResource(StyleProperty, "secondBtn");

                StackLayout itemsControls = new StackLayout();
                itemsControls.Padding = new Thickness(0);

                var orderTraItems = await App.manager.iGetOrderTransactionsByOrderID(orderHeader.OrderID);

                StackLayout items = new StackLayout();

                foreach (var order in orderTraItems)
                {
                    Grid grid = new Grid();

                    grid.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition{Width=GridLength.Star},
                        new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                        new ColumnDefinition{Width=GridLength.Star}
                    };

                    Label qtyLabel = new Label
                    {
                        Text = order.Quantity.ToString(),
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    qtyLabel.SetDynamicResource(StyleProperty, "checkLabel");
                    grid.Children.Add(qtyLabel, 0, 0);

                    Label itemLabel = new Label
                    {
                        Text = order.MenuItemTextOT.ToString(),
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    itemLabel.SetDynamicResource(StyleProperty, "checkLabel");
                    grid.Children.Add(itemLabel, 1, 0);


                    Label extPriceLabel = new Label
                    {
                        Text = order.ExtendedPrice.ToString(),
                        HorizontalTextAlignment = TextAlignment.End,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    extPriceLabel.SetDynamicResource(StyleProperty, "checkLabel");
                    grid.Children.Add(extPriceLabel, 2, 0);

                    Frame itemFrame = new Frame
                    {
                        BackgroundColor = Color.White,
                        BorderColor = Color.Gray,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Fill,
                        Padding = new Thickness(5),
                        Margin = new Thickness(5),
                    };
                    itemFrame.Content = grid;

                    items.Children.Add(itemFrame);
                }
                
                showItems.Clicked += showItems_Clicked;
                hideItems.Clicked += hideItems_Clicked;

                void showItems_Clicked(object sender, EventArgs e)
                {
                    itemsControls.Children.Remove(showItems);
                    itemsControls.Children.Add(items);
                    itemsControls.Children.Add(hideItems);
                };

                void hideItems_Clicked(object sender, EventArgs e)
                {
                    itemsControls.Children.Remove(items);
                    itemsControls.Children.Remove(hideItems);
                    itemsControls.Children.Add(showItems);
                };

                itemsControls.Children.Add(showItems);

                Label Footer = new Label
                {
                    Text = "Total: " + orderHeader.AmountDue.ToString(),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    TextDecorations = TextDecorations.Underline,
                    Padding = new Thickness(10),
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                };

                /*Frame footerFrame = new Frame
                {
                    BackgroundColor = Color.White,
                    BorderColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Fill,
                    Padding = new Thickness(10),

                };
                footerFrame.Content = Footer;*/

                Footer.SetDynamicResource(StyleProperty, "checkLabel");
                txtDateTimeOpenedTable.SetDynamicResource(StyleProperty, "checkLabel");
                txtServer.SetDynamicResource(StyleProperty, "checkLabel");
                Header.SetDynamicResource(StyleProperty, "checkLabel");

                gridLists.Children.Add(headerFrame);
                gridLists.Children.Add(txtServer);
                gridLists.Children.Add(txtDateTimeOpenedTable);
                gridLists.Children.Add(itemsControls);
                gridLists.Children.Add(Footer);

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
                    Margin = new Thickness(0)
                };
                moveOrder.SetDynamicResource(StyleProperty, "secondBtn");

                moveOrder.Clicked += (sender, args) => moveOrder_Clicked(sender, orderHeader.OrderID);

                Button toOrder = new Button
                {
                    Text = "Edit",
                    Padding = new Thickness(8),
                    Margin = new Thickness(0)
                };
                toOrder.SetDynamicResource(StyleProperty, "secondBtn");

                // ! No Time Sync !
                toOrder.Clicked += (sender, args) => toOrder_Clicked(orderHeader.OrderID, TableID);

                controls.Children.Add(moveOrder, 0, 0);
                controls.Children.Add(toOrder, 1, 0);

                gridLists.Children.Add(controls);

                Frame checkFrame = new Frame
                {
                    CornerRadius = 4,
                    Margin = new Thickness(5),
                    BackgroundColor = Color.FromHex("#d9d9d9"),
                    Padding = new Thickness(5),
                };

                checkFrame.Content = gridLists;
                tblOrders.Children.Insert(index + 1, checkFrame);
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

        async void toOrder_Clicked(int OrdId, int TblId)
        {
            var b = dineInsAndEmp.Where(x => x.DineIn.DineInTableID == TblId).FirstOrDefault();           
            orderHeadersList = await App.manager.iGetOrderHeadersByDineInTableID(TblId);
            var orderToEdit = orderHeadersList.Find(x => x.OrderID == OrdId);  
            orderTransactionsList = await App.manager.iGetOrderTransactionsByOrderID(OrdId);
            orderTransactionsListByOrderID = orderTransactionsList.FindAll(x => x.OrderID == OrdId);
            
            await Navigation.PushAsync(new ActiveTableEditPage(orderToEdit, EmplFl, b.DineIn, orderTransactionsListByOrderID));
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
            else if (smokingSwitch.IsToggled == false)
            {
                char a = 's';
                Flag = Flag.Trim(a);
                Flag = OrderFlag(Flag);
            }
            PopList(GroupId, Flag);
        }

        private void windowSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (windowSwitch.IsToggled)
            {
                Flag += 'w';
                Flag = OrderFlag(Flag);
            }
            else if (windowSwitch.IsToggled == false)
            {
                char a = 'w';
                Flag = Flag.Trim(a);
                Flag = OrderFlag(Flag);
            }
            PopList(GroupId, Flag);
        }

        private void boothSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (boothSwitch.IsToggled)
            {
                Flag += 'b';
                Flag = OrderFlag(Flag);
            }
            else if (boothSwitch.IsToggled == false)
            {
                char a = 'b';
                Flag = Flag.Trim(a);
                Flag = OrderFlag(Flag);
            }
            PopList(GroupId, Flag);
        }

        static string OrderFlag(string flag)
        {
            char[] flags = flag.ToArray();
            Array.Sort(flags);
            return new string(flags);
        }

        private void switchLayout()
        {
            if (showingGroups)
            {
                pageGrid.ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{ Width = new GridLength(4, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(25, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                };

                tblGroups.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = new GridLength (8, GridUnitType.Star) },
                    new RowDefinition{ Height = new GridLength (2, GridUnitType.Star) },
                };

                tblGroups.Children.Remove(showGroups);
                showGroups.IsVisible = false;
                tblGroups.Children.Remove(lstvwGrupMese);

                lstvwGrupMese.ItemTemplate = new DataTemplate(() =>
                {
                    ViewCell view = new ViewCell();

                    Label tblGroupText = new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    tblGroupText.SetDynamicResource(StyleProperty, "mainBtnLabel");
                    tblGroupText.SetBinding(Label.TextProperty, "TableGroupText");

                    Frame tblGroupFrame = new Frame();
                    tblGroupFrame.SetDynamicResource(StyleProperty, "mainFrame");
                    tblGroupFrame.Content = tblGroupText;

                    view.View = tblGroupFrame;
                    return view;
                });

                tblGroups.Children.Add(lstvwGrupMese, 0, 0);
                tblGroups.Children.Add(filtersGrid, 0, 1);
                tblGroups.SetDynamicResource(StyleProperty, "secondGrid");

                pageGrid.Children.Add(tblGroups, 0, 0);
                pageGrid.Children.Remove(ordersScrollView);
            } 
            else if (showingGroups == false)
            {              
                pageGrid.ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{ Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition{ Width = new GridLength(25, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) },
                };

                tblGroups.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = new GridLength (2, GridUnitType.Star) },
                    new RowDefinition{ Height = new GridLength (18, GridUnitType.Star) },
                };

                tblGroups.Children.Remove(filtersGrid);
                tblGroups.Children.Remove(lstvwGrupMese);

                lstvwGrupMese.ItemTemplate = new DataTemplate(() =>
                {
                    ViewCell view = new ViewCell();

                    Label tblGroupText = new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    tblGroupText.SetDynamicResource(StyleProperty, "mainBtnLabel");
                    tblGroupText.SetBinding(Label.TextProperty, "TableGroupID");

                    Frame tblGroupFrame = new Frame();
                    tblGroupFrame.SetDynamicResource(StyleProperty, "mainFrame");
                    tblGroupFrame.Content = tblGroupText;

                    view.View = tblGroupFrame;
                    return view;
                });
                tblGroups.Children.Add(showGroups, 0, 0);
                showGroups.IsVisible = true;
                tblGroups.Children.Add(lstvwGrupMese, 0, 1);
                tblGroups.SetDynamicResource(StyleProperty, "mainGrid");
                tblOrders.SetDynamicResource(StyleProperty, "secondStack");

                pageGrid.Children.Add(tblGroups, 0, 0);
                pageGrid.Children.Add(ordersScrollView, 2, 0);
            };
        }

        private void showGroups_Clicked(object sender, EventArgs e)
        {
            showingGroups = true;
            switchLayout();
        }
    }

}

