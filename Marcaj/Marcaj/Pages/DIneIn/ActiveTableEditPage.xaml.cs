using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.DbModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Tables
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActiveTableEditPage : ContentPage
	{
		OrderHeadersModel OrderHeader;
		EmployeeFileModel EmpFile;
		DineInTableModel DineIn;
        List<OrderTransactionsModel> orderTraList;

        bool IsFirstLoad = true;
        int GroupId = 0;
		public ActiveTableEditPage (OrderHeadersModel orderHeader, EmployeeFileModel empFile, DineInTableModel dineIn, List<OrderTransactionsModel> orderTransactions)
		{
			InitializeComponent ();
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
                txtServer.Text = "Server: " + OrderHeader.EmployeeName;
                txtStation.Text = "Station: " + OrderHeader.StationID;
                txtOrderName.Text = "Order: " + OrderHeader.OrderID.ToString();
                txtTableName.Text = "Table: " + DineIn.DineInTableText;
                txtDateTimeOpenedTable.Text = OrderHeader.OrderDateTime.ToString();
                txtAmountDue.Text = "Amount Due: " + OrderHeader.AmountDue.ToString();
                tableName.Text = DineIn.DineInTableText;
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

            lstvwMenuItems.SelectedItem = null;
        }

        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            var ordHd = new OrderHeadersModel();
            ordHd.AmountDue = Convert.ToSingle(txtAmountDue.Text.Split(' ')[2]);
            ordHd.SubTotal = ordHd.AmountDue;
            ordHd.SynchVer = null;
            ordHd.SynchVer = DateTime.MinValue;
            Debug.WriteLine(ordHd.SubTotal);
            await App.manager.iPutOrderHeaders(ordHd,OrderHeader.OrderID);

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


            // MessagingCenter.Send<ActiveTableEditPage>(this, "Up");
            int BackCount = 2;
            for (var counter = 1; counter < BackCount; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            await Navigation.PopAsync();

        }

    }
}