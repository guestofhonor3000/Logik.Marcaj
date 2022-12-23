using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
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
        List<MenuItemsModel> menuItems;
        List<MenuItemsModel> allMenuItems;
        List<OrderTransactionsModel> orderTraList;
        List<int> digits = new List<int>();

        int digit;
        View numpad;
        int Qty = 1;
        string sQty;
        float ModPrice;
        string sModPrice;
        bool discount = true;
        bool surcharge = false;
        bool IsFirstLoad = true;
        int GroupId = 0;
        float total;
		public ActiveTableEditPage (OrderHeadersModel orderHeader, EmployeeFileModel empFile, DineInTableModel dineIn, List<OrderTransactionsModel> orderTransactions)
		{
			InitializeComponent ();

            //Clock.Text = DateTime.Now.ToString();
            #region digits
            digits.Add(1);
            digits.Add(2);
            digits.Add(3);
            digits.Add(4);
            digits.Add(5);
            digits.Add(6);
            digits.Add(7);
            digits.Add(8);
            digits.Add(9);
            digits.Add(11);
            digits.Add(0);
            digits.Add(12);
            #endregion
            OrderHeader = orderHeader;
            EmpFile = empFile;
            DineIn = dineIn;
            orderTraList = orderTransactions;
            Debug.WriteLine(orderTraList.Count);
            PopList(IsFirstLoad, GroupId);
		}

        async void PopList(bool isFirstLoad, int groupId)
        {
            if (isFirstLoad == true)
            {
                OrderHeader.SynchVer = DateTime.Now;
                await App.manager.iPutSynchVerOrderHeaders(OrderHeader, OrderHeader.OrderID);
                txtServer.Text = "Deschis de: " + EmpFile.FirstName;
                txtStation.Text = "Statie: " + OrderHeader.StationID;
                txtOrderName.Text = "#" + OrderHeader.OrderID.ToString();
                txtTableName.Text = "Masa: " + DineIn.DineInTableText;
                txtDateTimeOpenedTable.Text = OrderHeader.OrderDateTime.ToString();
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

                multiGrid.Children.Clear();
                qtyFrame.IsVisible = false;

                StackLayout Detalii= new StackLayout();
                Xamarin.Forms.Image itemPic = new Xamarin.Forms.Image
                {
                    Source = selIt.PictureName,
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 100
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

         private void lstvwOrderTransactions_ItemSelected(object sender, SelectedItemChangedEventArgs e) 
        {
            /*if (IsFirstLoad != true)
            {
                var selIt = e.SelectedItem as OrderTransactionsModel;
                IsFirstLoad = false;
                
            }*/
        }

        private void ItemQty_Clicked(object sender, EventArgs e)
        {
            var a = sender as Button;
            var b = orderTraList.Where(x => x.MenuItemTextOT == a.AutomationId).FirstOrDefault();

            if (qtyFrame.IsVisible == false) 
            {
                 btnQty_Clicked(a, e);  
            }
            if (Qty != 1) 
            { 
                b.Quantity = Qty;
                b.ExtendedPrice = b.MenuItemUnitPrice * b.Quantity;
                total += b.ExtendedPrice;
                a.Text = b.Quantity.ToString();
            }
            //orderTraList.Add(b);
            txtAmountDue.Text = total.ToString();


        }

        private void ItemPrice_Clicked(object sender, EventArgs e)
        {
            var a = sender as Button;
            var b = orderTraList.Where(x => x.MenuItemTextOT == a.AutomationId).FirstOrDefault();


            if (priceFrame.IsVisible == false)
            {
                btnDis_Sur_Clicked(a, e);               
            }

            if (discount)
            {
                if (ModPrice != 0 && ModPrice < b.MenuItemUnitPrice)
                {
                    b.ExtendedPrice = (b.MenuItemUnitPrice - ModPrice) * b.Quantity;
                    total += b.ExtendedPrice;
                }
                else if (ModPrice == 0) b.ExtendedPrice = b.ExtendedPrice;
            }
            else if (surcharge) 
            {
                if (ModPrice != 0)
                {
                    b.ExtendedPrice = (b.MenuItemUnitPrice + ModPrice) * b.Quantity;
                    total += b.ExtendedPrice;
                }
                else if (ModPrice == 0) b.ExtendedPrice = b.ExtendedPrice;
            }

            a.Text = b.ExtendedPrice.ToString();
            txtAmountDue.Text = total.ToString();
        }

        private void txtAmountDue_Clicked(object sender, EventArgs e)
        {
            var a = sender as Button;

            if (priceFrame.IsVisible == false)
            {
                btnDis_Sur_Clicked(a, e);
                a.Text = total.ToString();
            }

            if (discount)
            {
                if (ModPrice != 0 && ModPrice < Convert.ToSingle(a.Text))
                {
                    total = Convert.ToSingle(a.Text) - ModPrice;
                }
            }
            else if (surcharge)
            {
                if (ModPrice != 0)
                {
                    total = Convert.ToSingle(a.Text) + ModPrice;
                }
            }

            a.Text = total.ToString();
        }

        private View makeNumpad()
        {
            int row = 0;
            int col = -1;
            Grid Numpad = new Grid
            {
                ColumnDefinitions =
                {
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

            foreach (var digit in digits) 
            {
                col++;
                if (col == 3)
                { 
                    row++; 
                    col = 0; 
                }

                Button digitBtn = new Button
                {
                    Text = digit.ToString(),
                    Margin = new Thickness(0),
                };
                digitBtn.SetDynamicResource(StyleProperty, "btn");
                digitBtn.Clicked += (sender, EventArgs) => digitBtn_Clicked(digit);

                if (digit == 11) 
                {
                    digitBtn.Text = "DEL";
                }

                if (digit == 12)
                {
                    digitBtn.Text = "OK";
                    digitBtn.BackgroundColor = Color.FromHex("#4db290");
                }

                Numpad.Children.Add(digitBtn, col, row);
            }
            numpad = Numpad;
            return numpad;
        }

        private async void digitBtn_Clicked(int numPressed) 
        {
            if (numPressed < 10) 
            {
                if (qtyFrame.IsVisible)
                {
                    sQty += numPressed.ToString();
                    Qty = Convert.ToInt32(sQty);
                    qty.Text = sQty;
                    Debug.WriteLine(sQty);
                    Debug.WriteLine(Qty);
                }
                else if (priceFrame.IsVisible) 
                {                    
                    sModPrice+= numPressed.ToString();
                    ModPrice= Convert.ToSingle(sModPrice);
                    if (discount) price.Text = "-" + sModPrice;
                    else price.Text = sModPrice;
                    Debug.WriteLine(sModPrice);
                    Debug.WriteLine(ModPrice);
                }
            }
            else if (numPressed == 11) 
            {
                if (qtyFrame.IsVisible)
                {
                    sQty = "";
                    Qty = 1;
                    qty.Text = qty.Placeholder;
                } 
                else if (priceFrame.IsVisible) 
                {
                    sModPrice = "";
                    ModPrice = 0;
                    price.Text = price.Placeholder;
                }
            }
            else if (numPressed == 12)
            {
                if (qtyFrame.IsVisible)
                {
                    if (sQty != "")
                    {
                        Qty = Convert.ToInt32(sQty);
                        qty.Text = sQty;
                    }
                    else
                    {
                        Qty = 1;
                    }
                }
                else if (priceFrame.IsVisible) 
                {
                    if (sModPrice != "")
                    {
                        ModPrice = Convert.ToSingle(sModPrice);
                        if (discount) price.Text = "-" + sModPrice;
                        else price.Text = sModPrice;
                    }
                    else
                    {
                        ModPrice = 0;
                    }
                }
            }
            
        }

        private void btnQty_Clicked(object sender, EventArgs e)
        {
            multiGrid.Children.Clear();
            priceFrame.IsVisible = false;

            multiGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = new GridLength (4, GridUnitType.Star) },
            };

            makeNumpad();
            qtyFrame.IsVisible= true;
            multiGrid.Children.Add(qtyFrame, 0, 0);
            multiGrid.Children.Add(numpad, 0, 1);
        }

        private void btnDis_Sur_Clicked(object sender, EventArgs e)
        {
            multiGrid.Children.Clear();
            qtyFrame.IsVisible = false;

            if (discount)
            {
                discount = false;
                surcharge = true;
            }
            else if (surcharge)
            {
                surcharge = false;
                discount = true;
            }

            multiGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = new GridLength (4, GridUnitType.Star) },
            };

            makeNumpad();
            priceFrame.IsVisible = true;
            multiGrid.Children.Add(priceFrame, 0, 0);
            multiGrid.Children.Add(numpad, 0, 1);
        }

        private void btnOpts_Clicked(object sender, EventArgs e)
        {

        }

        private void btnScale_Clicked(object sender, EventArgs e)
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