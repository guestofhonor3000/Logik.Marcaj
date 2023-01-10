using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Marcaj.Pages.Modals;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace Marcaj.Pages.Tables
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActiveTableEditPage : ContentPage
	{
		OrderHeadersModel OrderHeader;
		EmployeeFileModel EmpFile;
		DineInTableModel DineIn;
        OrderTransactionsModel OrderTra;
        List<OrderHeadersModel> orderHeaderList;
        List<MenuItemsModel> menuItems;
        List<MenuItemsModel> allMenuItems;
        List<OrderTransactionsModel> orderTraList;

        int Qty = 1;
        //string sQty;
        //float ModPrice;
        //string sModPrice;
        //string InStock;
        //bool discount = true;
        //bool surcharge = false;
        bool chgQty = false;
        bool chgPrice = false;
        bool chgMods = false;
        bool IsFirstLoad = true;
        bool itemEdit = false;
        string DateTimeOpened;
        //bool isEditable = false;
        int GroupId = 0;
        float total;
		public ActiveTableEditPage (OrderHeadersModel orderHeader, EmployeeFileModel empFile, DineInTableModel dineIn, List<OrderTransactionsModel> orderTransactions)
		{
			InitializeComponent ();

            //Clock.Text = DateTime.Now.ToString();
            OrderHeader = orderHeader;
            EmpFile = empFile;
            DineIn = dineIn;
            orderTraList = orderTransactions;
            
            Debug.WriteLine(orderTraList.Count);
            PopList(IsFirstLoad, GroupId);
		}



            public new bool IsEnabled { get; set; }
            


        async void PopList(bool isFirstLoad, int groupId)
        {
            if (isFirstLoad == true)
            {
                OrderHeader.SynchVer = DateTime.Now;
                await App.manager.iPutSynchVerOrderHeaders(OrderHeader, OrderHeader.OrderID);
                txtServer.Text = "Deschis de: " + EmpFile.FirstName;
                txtClient.Text = "Client:" + OrderHeader.SpecificCustomerName;
                if (OrderHeader.SpecificCustomerName == null) txtClient.Text = "Client: Nou";
                txtStation.Text = "Statie: " + OrderHeader.StationID;
                txtOrderName.Text = "#" + OrderHeader.OrderID.ToString();
                txtTableName.Text = "Masa: " + DineIn.DineInTableText;
                DateTimeOpened = OrderHeader.OrderDateTime.ToString();
                txtDateOpenedTable.Text = DateTimeOpened.Split(' ')[0];
                txtTimeOpenedTable.Text = DateTimeOpened.Split(' ')[1];
                txtAmountDue.Text = OrderHeader.AmountDue.ToString();
                //tableName.Text = DineIn.DineInTableText;
                allMenuItems = await App.manager.iGetMenuItems();
                total = Convert.ToSingle(txtAmountDue.Text);
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
                        lstvwOrderTransactions.ItemsSource = orderTraList;
                    }
                }
                IsFirstLoad = false;
            }
            else
            {
                var b = await App.manager.iGetMenuItemsByGroupID(groupId);
                if(b!=null)
                {
                    lstvwMenuItems.ItemsSource = b;
                }
            }

        }
        
        private void lstvwMenuGroups_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (IsFirstLoad != true)
            {
                var selIt = e.CurrentSelection.FirstOrDefault() as MenuGroupsModel;
                IsFirstLoad = false;
                PopList(IsFirstLoad, selIt.MenuGroupID);
            }         
        }
   
        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            OrderHeader.SynchVer = DateTime.MinValue;
            Debug.WriteLine(OrderHeader.OrderID);
            await App.manager.iPutSynchVerOrderHeaders(OrderHeader, OrderHeader.OrderID);
            int BackCount = 2;
            for (var counter = 1; counter < BackCount; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            await Navigation.PopAsync();
        }

        private void lstvwMenuItems_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() != null)
            {
                var selIt = e.CurrentSelection.FirstOrDefault() as MenuItemsModel;
                var orderTra = new OrderTransactionsModel();

                var exOrderTra = orderTraList.FirstOrDefault(x => x.MenuItemID == selIt.MenuItemID);
                if (exOrderTra == null)
                {
                    orderTra = new OrderTransactionsModel();
                    orderTra.OrderID = OrderHeader.OrderID;
                    orderTra.MenuItemID = selIt.MenuItemID;
                    orderTra.MenuItemTextOT = selIt.MenuItemText;
                    orderTra.Quantity = orderTra.Quantity + Qty;
                    orderTra.MenuItemUnitPrice = selIt.DefaultUnitPrice;
                    orderTra.ExtendedPrice = orderTra.MenuItemUnitPrice * orderTra.Quantity;
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
                total = extPrice;
                txtAmountDue.Text = total.ToString();
                Debug.WriteLine(total.ToString());

                /*multiGrid.Children.Clear();
                qtyFrame.IsVisible = false;

                StackLayout Detalii= new StackLayout();
                Xamarin.Forms.Image itemPic = new Xamarin.Forms.Image
                {
                    Source = selIt.PictureName,
                    Aspect = Aspect.AspectFill,
                };
                Label itemName = new Label
                {
                    Text = selIt.MenuItemText,
                };
                itemName.SetDynamicResource(StyleProperty, "mainBtnLabel");
                Label itemDetails = new Label
                {
                    Text = selIt.MenuItemDescription,
                };
                itemDetails.SetDynamicResource(StyleProperty, "mainBtnLabel");

                Detalii.Children.Add(itemPic);
                Detalii.Children.Add(itemName);
                Detalii.Children.Add(itemDetails);

                multiGrid.Children.Add(Detalii);
                */
            }

            ((CollectionView)sender).SelectedItem = null;
        }

        int entries_ = 0;
        
        void Search(string text)
        {
            entries_++;
            if (entries_ == entries)
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

        
        private async void lstvwOrderTransactions_ItemSelected(object sender, SelectionChangedEventArgs e)
        {

            if (e.CurrentSelection.FirstOrDefault() != null)
            {
                var selIt = e.CurrentSelection.FirstOrDefault() as OrderTransactionsModel;
                OrderTra = selIt;
                Debug.WriteLine(selIt.MenuItemTextOT);

                if (chgMods) 
                {
                    var menuItem = menuItems.FirstOrDefault(x => x.MenuItemID == selIt.MenuItemID);
                    var modifsModal = new ModifiersModalPage(selIt , menuItem);
                    await Navigation.PushModalAsync(modifsModal);
                    chgMods= false;
                    btnModifs.BorderWidth = 0;
                }
                else if (itemEdit == false) 
                { 
                    itemEdit= true;
                }
            }
            ((CollectionView)sender).SelectedItem = null;
        }

        private void itemFrame_Focused(object sender, FocusEventArgs e)
        {
            var a = sender as Frame;
            //if (a.AutomationId == OrderTra.MenuItemTextOT) isEditable= true;
        }


        private async void ItemQty_Clicked(object sender, EventArgs e)
        {
            var a = sender as Button;
            var b = orderTraList.Where(x => x.MenuItemTextOT == a.AutomationId).FirstOrDefault();

            if (itemEdit)
            {
                var qtyModal = new QtyModalPage(a, b);
                await Navigation.PushModalAsync(qtyModal);
                itemEdit = false;
                //total += b.ExtendedPrice;

                //orderTraList.Add(b);
                //total += b.ExtendedPrice * b.Quantity;
                //txtAmountDue.Text = total.ToString();
            }
        }


        private async void ItemPrice_Clicked(object sender, EventArgs e)
        {
            var a = sender as Button;
            var b = orderTraList.Where(x => x.MenuItemTextOT == a.AutomationId).FirstOrDefault();

            if (itemEdit)
            {

                var priceModal = new PriceModalPage(a, b);
                await Navigation.PushModalAsync(priceModal);
                itemEdit = false;
                //total += b.ExtendedPrice * b.Quantity;
                //txtAmountDue.Text = total.ToString();
            }
        }

        private void txtAmountDue_Clicked(object sender, EventArgs e)
        {
            var a = sender as Button;
            //a.Text = total.ToString();
        }


        private void btnQty_Clicked(object sender, EventArgs e)
        {
            if (chgQty == false) chgQty = true;
            Debug.WriteLine(chgQty);
        }


        private void btnDis_Sur_Clicked(object sender, EventArgs e)
        {
            if (chgPrice == false) chgPrice = true;
            Debug.WriteLine(chgQty);
        }


        private void btnModifs_Clicked(object sender, EventArgs e)
        {
            if (chgMods == false)
            {
                chgMods = true;
                btnModifs.BorderWidth= 5;
                btnModifs.BorderColor = Color.FromHex("#4db290");
            }
            Debug.WriteLine(chgMods);
        }

        private async void btnClient_Clicked(object sender, EventArgs e)
        {
            var a = sender as Button;
            var clientModal = new ClientModalPage(OrderHeader, orderHeaderList);
            await Navigation.PushModalAsync(clientModal);
            txtClient.Text = "Client:" + OrderHeader.SpecificCustomerName;
        }

        private void btnVoid_Clicked(object sender, EventArgs e)
        {

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

        private async void btnSettle_Clicked(object sender, EventArgs e)
        {
            var ordHd = new OrderHeadersModel();
            ordHd.AmountDue = Convert.ToSingle(txtAmountDue);
            ordHd.SubTotal = ordHd.AmountDue;
            ordHd.OrderStatus = "2";
            ordHd.SynchVer = DateTime.MinValue;
            Debug.WriteLine(ordHd.SubTotal);
            await App.manager.iPutOrderHeaders(ordHd, OrderHeader.OrderID);
            
            int BackCount = 2;
            for (var counter = 1; counter < BackCount; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            MessagingCenter.Send<ActiveTableEditPage>(this, "Up");
            await Navigation.PopAsync();

        }

        private async void btnAnuleazaOrder_Clicked(object sender, EventArgs e)
        {
            var ordHd = new OrderHeadersModel();
            ordHd.AmountDue = 0;
            ordHd.SubTotal = 0;
            ordHd.OrderStatus = "3";
            ordHd.SynchVer = DateTime.MinValue;
            Debug.WriteLine(ordHd.SubTotal);
            await App.manager.iPutOrderHeaders(ordHd, OrderHeader.OrderID);

            int BackCount = 2;
            for (var counter = 1; counter < BackCount; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            MessagingCenter.Send<ActiveTableEditPage>(this, "Up");
            await Navigation.PopAsync();
        }

        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            var ordHd = new OrderHeadersModel();
            ordHd.AmountDue = total;
            ordHd.SubTotal = ordHd.AmountDue;
            ordHd.SynchVer = DateTime.MinValue;
            Debug.WriteLine(ordHd.SubTotal);
            await App.manager.iPutOrderHeaders(ordHd, OrderHeader.OrderID);

            var ordTraListToPost = new List<OrderTransactionsModel>();

            foreach (var ordTraItem in orderTraList)
            {
                var ordTra = new OrderTransactionsModel();

                ordTra.OrderID = ordTraItem.OrderID;
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

            await App.manager.iPostOrderTransactionActive(ordTraListToPost, DineIn.DineInTableID);


            MessagingCenter.Send<ActiveTableEditPage>(this, "Up");
            int BackCount = 1;
            for (var counter = 1; counter < BackCount; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            }
            await Navigation.PopAsync();
        }
    }
}