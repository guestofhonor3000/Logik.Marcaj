using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Marcaj.Models.LocalDbModels;
using Marcaj.Pages.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Grid;

namespace Marcaj.Pages.DIneIn
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageActive : ContentPage
    {
        EmployeeFileModel EmpFile;
        DineInTableModel DineIn;
        List<OrderHeadersModel> orderHeadersList;
        List<OrderTransactionsModel> orderTransactionsList;
        List<OrderTransactionsModel> orderTransactionsListByOrderID;
        public ObservableCollection<OptionsModel> menuBtnList;
        int NumberOfClicksNext = 0;
        OrderHeadersModel orderHeader;
        public PageActive(DineInTableModel dineIn, EmployeeFileModel empFile)
        {
            InitializeComponent();
            orderHeader = new OrderHeadersModel();
            orderHeadersList = new List<OrderHeadersModel>();
            orderTransactionsList = new List<OrderTransactionsModel>();
            orderTransactionsListByOrderID = new List<OrderTransactionsModel>();
            EmpFile = empFile;
            DineIn = dineIn;
            PopList();
            MessagingCenter.Subscribe<NotActiveTable>(this, "Up", (sender) =>
            {
                NumberOfClicksNext = 0;
                PopList();
                GoBack();

            });
            MessagingCenter.Subscribe<ActiveTableEditPage>(this, "Up", (sender) =>
            {
                NumberOfClicksNext = 0;
                PopList();
                GoBack();
            });
        }

        async void GoBack()
        {
            await Navigation.PopAsync();
        }

     

        async void PopList()
        {
            orderHeadersList = await App.manager.iGetOrderHeadersByDineInTableID(DineIn.DineInTableID);

            
            if (orderHeadersList.Count > 3)
            {
                btnNextPage.IsEnabled = true;
            }

            gridLists.Children.Clear();

            foreach (var orderHeader in orderHeadersList)
            {
                index = index + 1;
                if (index <= orderHeadersList.Count)
                {

                    lstvwOrderHeader.ClassId = orderHeader.OrderID.ToString();

                    txtServer.Text = EmpFile.FirstName;
                    txtStation.Text = "Station: " + orderHeader.StationID.ToString();
                    txtOrderName.Text = "Order: " + orderHeader.OrderID.ToString();
                    txtTableName.Text = "Masa: " + DineIn.DineInTableText;
                    txtDateTimeOpenedTable.Text = orderHeader.OrderDateTime.ToString();
                    txtAmountDue.Text = orderHeader.AmountDue.ToString();

                    var orderTraItems = await App.manager.iGetOrderTransactionsByOrderID(orderHeader.OrderID);
                    foreach (var orderTraItem in orderTraItems)
                    {
                        orderTransactionsList.Add(orderTraItem);
                    }
                    lstvwOrderHeader.ItemsSource = orderTraItems;

                    gridLists.Children.Add(lstvwOrderHeader, index - 1, 0);

                }
            }
        }

        private async void btnNewOrder_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotActiveTable(DineIn, EmpFile, "opened"));
        }

        private void btnNextPage_Clicked(object sender, EventArgs e)
        {
            NumberOfClicksNext = NumberOfClicksNext + 1;
      
        }

        private void btnPrevPage_Clicked(object sender, EventArgs e)
        {
            NumberOfClicksNext = NumberOfClicksNext - 1;
      
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllTables(EmpFile));
        }
        int index;
        private async void ListView_Focused(object sender, FocusEventArgs e)
        {
            index++;
            if (index == 1)
            {
                var lstvw = sender as ListView;
                var orderId = lstvw.ClassId;
                var orderHeaderFocused = orderHeadersList.Find(x => x.OrderID == Convert.ToInt32(orderId));
                orderHeader = orderHeaderFocused;
                orderTransactionsListByOrderID = orderTransactionsList.FindAll(x => x.OrderID == Convert.ToInt32(orderId));
        
                if (orderHeader.SynchVer != DateTime.MinValue)
                {
                    await DisplayAlert("Error", "Order is already opened on another station!", "Ok");
                }
                else
                {
                    await Navigation.PushAsync(new ActiveTableEditPage(orderHeader, EmpFile, DineIn, orderTransactionsListByOrderID));
                }
            }
        }
    }
}