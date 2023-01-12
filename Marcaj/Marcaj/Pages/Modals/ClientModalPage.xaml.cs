using Marcaj.Models.DbModels;
using Marcaj.Models.DbModels.LGK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientModalPage : ContentPage
    {
        OrderHeadersModel order;
        List<InventoryClients> clientsList;
        InventoryClients Client;
        List<OrderHeadersModel> ordersList;
        string ClientName;
        bool isSearch;
        public ClientModalPage(OrderHeadersModel orderHeader, List<OrderHeadersModel> orderHeadersList)
        {
            InitializeComponent();
            order = orderHeader;
            ordersList = orderHeadersList;
            isSearch = false;
            multiShow();
        }

        int entries_ = 0;
        CancellationTokenSource _tokenSource;
        int entries = 0;
        private async void Search(string text)
        {
            entries_++;
            if (entries_ == entries)
            {
                Debug.WriteLine("cauta");
                if (search.Text != "")
                {
                    var a = await App.manager.iGetAllInventoryClients();
                    var b = a.Where(x => x.InventoryClientText.ToLower().Contains(text.ToLower()) == true).ToList();
                    if (b.Count > 0)
                    {
                        clientsListView.ItemsSource = b;
                        clientsListView.SelectedItem = b[0];
                    }
                    else
                    {
                        clientsListView.ItemsSource = a;
                        clientsListView.SelectedItem = a[0];
                    }
                }
            }
        }
        private async void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            entries++;
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
            }
            _tokenSource = new CancellationTokenSource();

            await Task.Delay(500);
            Search(search.Text);
        }

        private async void clientsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as InventoryClients;

            if (selected != null) 
            {
                var chgOrdHd = new OrderHeadersModel();
                chgOrdHd.CustomerID = selected.InventoryClientID;
                chgOrdHd.SpecificCustomerName = selected.InventoryClientText;

                await App.manager.iPutOrderHeaders(chgOrdHd, order.OrderID);
                await Navigation.PopModalAsync();
            }
        }

        private async void multiShow()
        {
            multiGrid.RowDefinitions = new RowDefinitionCollection
            {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
            };

            Button clientLookup = new Button
            {
                Text = "Selectare Client",
                Margin = new Thickness(20, 10, 20, 10),
            };
            clientLookup.SetDynamicResource(StyleProperty, "btn");

            Button clientNew = new Button
            {
                Text = "Client Nou",
                Margin = new Thickness(20, 10, 20, 10),
            };
            clientNew.SetDynamicResource(StyleProperty, "btn");

            clientLookup.Clicked += clientLookup_Clicked;
            clientNew.Clicked += clientNew_Clicked;


            async void clientLookup_Clicked(object sender0, EventArgs e0)
            {
                multiGrid.Children.Clear();

                multiGrid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = new GridLength(6, GridUnitType.Star) },
                };

                clientsList = await App.manager.iGetAllInventoryClients();
                clientsListView.ItemsSource = clientsList;
                
                searchFrame.IsVisible= true;
                clientsListView.IsVisible= true;
                multiGrid.Children.Add(searchFrame, 0, 0);
                multiGrid.Children.Add(clientsListView, 0, 1);
            }


            void clientNew_Clicked(object sender0, EventArgs e0)
            {
                multiGrid.Children.Clear();
                multiGrid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(.5, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(.5, GridUnitType.Star) },
                };

                Entry clientNameEntry = new Entry
                {
                    BackgroundColor = Color.Transparent,
                    Placeholder = "Nume Client",
                    FontFamily = "Verdana",
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    PlaceholderColor = Color.Black,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Margin = -30
                };

                Frame clientNameFrame = new Frame
                {
                    CornerRadius = 4,
                    Background = Color.FromHex("#d9d9d9"),
                    Margin = new Thickness(20, 10, 20, 10),
                };
                clientNameFrame.Content = clientNameEntry;

                Button Done = new Button
                {
                    Text = "Salveaza",
                    Margin = new Thickness(20, 10, 20, 10),
                    BackgroundColor = Color.FromHex("#4db290"),
                };
                Done.SetDynamicResource(StyleProperty, "btn");
                Done.Clicked += Done_Clicked;

                async void Done_Clicked(object sender, EventArgs e)
                {
                    ClientName = clientNameEntry.Text;
                    order.SpecificCustomerName = ClientName;
                    //var modOrder = new OrderHeadersModel();
                    //modOrder.SpecificCustomerName= ClientName;
                    //await App.manager.iPutOrderHeaders(modOrder, order.OrderID);

                    await Navigation.PopModalAsync();
                }

                multiGrid.Children.Add(clientNameFrame, 0, 1);
                multiGrid.Children.Add(Done, 0, 3);
            }
            multiGrid.Children.Add(clientLookup, 0, 0);
            multiGrid.Children.Add(clientNew, 0, 1);
        }

        private async void closeModal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}