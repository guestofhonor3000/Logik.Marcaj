using Marcaj.Models.DbModels;
using Marcaj.Models.LocalDbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
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
        List<MenuItemsModel> menuItems;
        List<MenuItemsModel> allMenuItems;
        LStationSettingsModel StationModel;
        int Qty = 1;
        string sQty;
        string _Type;
        public NotActiveTable(DineInTableModel dineIn, EmployeeFileModel empFile, string type)
        {
            InitializeComponent();
            _Type = type;
            menuItems = new List<MenuItemsModel>();
            allMenuItems = new List<MenuItemsModel>();
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
                txtServer.Text = EmpFile.FirstName;
                txtStation.Text = station.ComputerName;
                txtOrderName.Text = "NewOrder";
                txtTableName.Text = "Masa: " + DineIn.DineInTableText;
                txtDateTimeOpenedTable.Text = DateTime.Now.ToString();
                txtAmountDue.Text = "Total: 0";
                tableName.Text = DineIn.DineInTableText;
                allMenuItems = await App.manager.iGetMenuItems();
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {

                    var a = await App.manager.iGetMenuGroups();
                    if (a != null)
                    {
                        if (a.Count > 0)
                        {
                            lstvwMenuGroups.ItemsSource = a;
                            lstvwMenuGroups.SelectedItem = a[0];
                            menuItems = allMenuItems.Where(x => x.MenuGroupID == a[0].MenuGroupID).ToList();
                            if (menuItems != null)
                            {
                                lstvwMenuItems.ItemsSource = menuItems;
                            }
                        }
                    }
                }
                else
                {
                    var a = await App.lDatabase.lGetMenuGroups();
                    if (a != null)
                    {
                        if (a.Count > 0)
                        {
                            lstvwMenuGroups.ItemsSource = a;
                            lstvwMenuGroups.SelectedItem = a[0];
                            var b = await App.lDatabase.lGetMenuItemsByGroupID(a[0].MenuGroupID);
                            if (b != null)
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
                    if (b != null)
                    {
                        lstvwMenuItems.ItemsSource = b;
                    }
                }
            }
        }
        private void lstvwMenuGroups_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (IsFirstLoad != true)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {

                    var selIt = e.CurrentSelection.FirstOrDefault() as MenuGroupsModel;
                    IsFirstLoad = false;
                    PopList(IsFirstLoad, selIt.MenuGroupID);
                }
                else
                {
                    var selIt = e.CurrentSelection.FirstOrDefault() as LMenuGroupsModel;
                    IsFirstLoad = false;
                    PopList(IsFirstLoad, selIt.MenuGroupID);
                }
            }
        }

        private void lstvwMenuItems_ItemSelected(object sender, SelectionChangedEventArgs e)
        {

            if (e.CurrentSelection.FirstOrDefault() != null)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var selIt = e.CurrentSelection.FirstOrDefault() as MenuItemsModel;
                    var orderTra = new OrderTransactionsModel();

                    var exOrderTra = orderTraList.FirstOrDefault(x => x.MenuItemID == selIt.MenuItemID);
                    if (exOrderTra == null)
                    {
                        orderTra = new OrderTransactionsModel();
                        orderTra.MenuItemID = selIt.MenuItemID;
                        orderTra.MenuItemTextOT = selIt.MenuItemText;
                        orderTra.Quantity = Qty;
                        orderTra.MenuItemUnitPrice = selIt.DefaultUnitPrice;
                        orderTra.ExtendedPrice = selIt.DefaultUnitPrice * Qty;
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
                    txtAmountDue.Text = "Total: " + extPrice.ToString();
                }
                else
                {
                    var selIt = e.CurrentSelection.FirstOrDefault() as LMenuItemsModel;
                    var orderTra = new LOrderTransactionsModel();

                    var exOrderTra = lorderTraList.FirstOrDefault(x => x.MenuItemID == selIt.MenuItemID);
                    if (exOrderTra == null)
                    {
                        orderTra = new LOrderTransactionsModel();
                        orderTra.MenuItemID = selIt.MenuItemID;
                        orderTra.MenuItemTextOT = selIt.MenuItemText;
                        orderTra.Quantity = Qty;
                        orderTra.MenuItemUnitPrice = selIt.DefaultUnitPrice;
                        orderTra.ExtendedPrice = selIt.DefaultUnitPrice * Qty;
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
                    txtAmountDue.Text = "Amount Due:" + extPrice.ToString();
                }

            }
            lstvwMenuItems.SelectedItem = null;
        }

        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (orderTraList.Count > 0)
                {

                    var ordHd = new OrderHeadersModel();
                    var stat = await App.manager.iGetStationSettings(DeviceInfo.Name);
                    ordHd.OrderDateTime = DateTime.Now;
                    ordHd.EmployeeID = EmpFile.EmployeeID;
                    ordHd.StationID = stat.StationID;
                    ordHd.OrderType = "1";
                    ordHd.DineInTableID = DineIn.DineInTableID;
                    ordHd.OrderStatus = "1";
                    ordHd.AmountDue = Convert.ToSingle(txtAmountDue.Text.Split(' ')[1]);
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
                if (lorderTraList.Count > 0)
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
                    ordHd.AmountDue = Convert.ToSingle(txtAmountDue.Text.Split(' ')[1]);
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

                /*int BackCount = 2;
                for (var counter = 1; counter < BackCount; counter++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
                await Navigation.PopAsync();*/
                await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PopAsync();
                //await Navigation.PopAsync();
            }
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
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


        private void btnMore_Clicked(object sender, EventArgs e)
        {

        }

        private void btnQty_Clicked(object sender, EventArgs e)
        {
            basse.Children.Clear();

            Grid numpad = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                },

            };
            qtyFrame.IsVisible = true;
            qty.IsVisible = true;
            numpad.Children.Add(qtyFrame, 0, 0);
            Grid.SetColumnSpan(qtyFrame, 4);

            Button btn1 = new Button
            {
                Text = "1",
            };
            btn1.Clicked += Btn1_Clicked;
            btn1.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn1, 0, 1);
            Button btn2 = new Button
            {
                Text = "2",
            };
            btn2.Clicked += Btn2_Clicked;
            btn2.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn2, 1, 1);
            Button btn3 = new Button
            {
                Text = "3",
            };
            btn3.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn3, 2, 1);
            btn3.Clicked += Btn3_Clicked;
            Button btn4 = new Button
            {
                Text = "4",
            };
            btn4.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn4, 0, 2);
            btn4.Clicked += Btn4_Clicked;
            Button btn5 = new Button
            {
                Text = "5",
            };
            btn5.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn5, 1, 2);
            btn5.Clicked += Btn5_Clicked;
            Button btn6 = new Button
            {
                Text = "6",
            };
            btn6.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn6, 2, 2);
            btn6.Clicked += Btn6_Clicked;
            Button btn7 = new Button
            {
                Text = "7",
            };
            btn7.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn7, 0, 3);
            btn7.Clicked += Btn7_Clicked;
            Button btn8 = new Button
            {
                Text = "8",
            };
            btn8.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn8, 1, 3);
            btn8.Clicked += Btn8_Clicked;
            Button btn9 = new Button
            {
                Text = "9",
            };
            btn9.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn9, 2, 3);
            btn9.Clicked += Btn9_Clicked;

            Button btn0 = new Button
            {
                Text = "0",
            };
            btn0.SetDynamicResource(StyleProperty, "secondBtn");
            numpad.Children.Add(btn0, 3, 1);
            Grid.SetRowSpan(btn0, 2);
            btn0.Clicked += Btn0_Clicked;
            Button btnOk = new Button
            {
                Text = "Ok",
                BackgroundColor = Color.FromHex("#478547")
            };
            btnOk.SetDynamicResource(StyleProperty, "secondBtn");
            btnOk.Clicked += BtnOk_Clicked;
            numpad.Children.Add(btnOk, 3, 3);

            basse.Children.Add(numpad);

        }

        #region Buttons
        private void BtnOk_Clicked(object sender, EventArgs e)
        {
            Qty = 1;
            sQty = "";
            qty.Text = "";
        }
        private void BtnDel_Clicked(object sender, EventArgs e)
        {
            sQty = "";
            qty.Text = "";
            Qty = 1;
        }

        private void Btn0_Clicked(object sender, EventArgs e)
        {
            sQty += "0";
            qty.Text += "0";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn9_Clicked(object sender, EventArgs e)
        {
            sQty += "9";
            qty.Text += "9";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn8_Clicked(object sender, EventArgs e)
        {
            sQty += "8";
            qty.Text += "8";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn7_Clicked(object sender, EventArgs e)
        {
            sQty += "7";
            qty.Text += "7";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn6_Clicked(object sender, EventArgs e)
        {
            sQty += "6";
            qty.Text += "6";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn5_Clicked(object sender, EventArgs e)
        {
            sQty += "5";
            qty.Text += "5";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn4_Clicked(object sender, EventArgs e)
        {
            sQty += "4";
            qty.Text += "4";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn3_Clicked(object sender, EventArgs e)
        {
            sQty += "3";
            qty.Text += "3";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn2_Clicked(object sender, EventArgs e)
        {
            sQty += "2";
            qty.Text += "2";
            Qty = Convert.ToInt32(sQty);
        }
        private void Btn1_Clicked(object sender, EventArgs e)
        {
            sQty += "1";
            qty.Text += "1";
            Qty = Convert.ToInt32(sQty);
        }

        private void btnOpts_Clicked(object sender, EventArgs e)
        {

        }

        private void btnScale_Clicked(object sender, EventArgs e)
        {

        }

        private void btnSettle_Clicked(object sender, EventArgs e)
        {

        }

        private void btnCheckEdit_Clicked(object sender, EventArgs e)
        {

        }
        #endregion

        int entries_ = 0;
        void Search(string text)
        {
            entries_++;
            if(entries_==entries)
            {
                Debug.WriteLine("cauta");
                if (entrySearch.Text != "")
                {
                    var a = allMenuItems.Where(x => x.MenuItemText.ToLower().Contains(text.ToLower()) == true).ToList();
                    if (a.Count > 0)
                    {
                        lstvwMenuItems.ItemsSource = a;
                    }
                }
                else
                {
                    lstvwMenuItems.ItemsSource = menuItems;
                }
            }
            
        }

        private CancellationTokenSource _tokenSource;
        int entries = 0;
        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            entries++;
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
            }
            _tokenSource = new CancellationTokenSource();

            await Task.Delay(1000);
            Search(entrySearch.Text);
        }
    }
}