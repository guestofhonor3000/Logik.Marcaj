using Marcaj.Models.DbModels;
using Marcaj.Models.DbModels.LGK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Modals
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientModalPage : ContentPage
	{
		OrderHeadersModel order;
        LGKMClientsModel client;
        List<LGKMClientsModel> clientsList;
        List<LGKMClientsModel> clientIdsList;
        List<OrderHeadersModel> ordersList;
        string ClientName;
		public ClientModalPage(OrderHeadersModel orderHeader, List<OrderHeadersModel> orderHeadersList)
		{
			InitializeComponent ();
            clientsList = new List<LGKMClientsModel> ();
            clientIdsList = new List<LGKMClientsModel> ();
            order = orderHeader;
            ordersList = orderHeadersList;
            multiShow();
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
                Margin = new Thickness(20,10,20,10),
            };
            clientLookup.SetDynamicResource(StyleProperty, "btn");

            Button clientNew = new Button
            {
                Text = "Client Nou",
                Margin = new Thickness(20,10,20,10),
            };
            clientNew.SetDynamicResource(StyleProperty, "btn");

            clientLookup.Clicked += clientLookup_Clicked;
            clientNew.Clicked += clientNew_Clicked;


            async void clientLookup_Clicked(object sender0, EventArgs e0)
            {
                int index = 0;

                multiGrid.Children.Clear();
                ScrollView clientScroll = new ScrollView();

                clientsList = await App.manager.iGetAllClients();
                clientIdsList = clientsList.Where(x => x.ID == clientsList[index].ID).ToList();
                
                foreach (var clients in clientsList) 
                {
                    foreach (var clientIds in clientIdsList)
                    {
                        if (clients.ID == clientIds.ID)
                        {
                            client = new LGKMClientsModel();
                            client.ID = clients.ID;
                            client.ClientName = clients.ClientName;
                            client.ClientDbCode = clients.ClientDbCode;
                            index++;
                        }

                            Frame clientFrame = new Frame();
                            clientFrame.SetDynamicResource(StyleProperty, "mainFrame");

                            Grid clientGrid = new Grid();
                            clientGrid.ColumnDefinitions = new ColumnDefinitionCollection
                            {
                            new ColumnDefinition { Width = GridLength.Star },
                            new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                            new ColumnDefinition { Width = GridLength.Star },
                            };

                            Label clientDbCode = new Label
                            {
                                Text = client.ClientDbCode
                            };
                            clientDbCode.SetDynamicResource(StyleProperty, "mainBtnLabel");

                            Label clientName = new Label
                            {
                                Text = client.ClientName
                            };
                            clientName.Text = client.ClientName.ToString();

                            Label clientId = new Label
                            {
                                Text = client.ID.ToString()
                            };
                            clientId.SetDynamicResource(StyleProperty, "mainBtnLabel");

                            clientGrid.Children.Add(clientDbCode, 0, 0);
                            clientGrid.Children.Add(clientName, 1, 0);
                            clientGrid.Children.Add(clientId, 2, 0);

                            clientFrame.Content = clientGrid;
                            multiGrid.Children.Add(clientFrame, 0, index - 1);                       
                    }
                }
                mainGrid.Children.Remove(multiGrid);
                clientScroll.Content = multiGrid;
                mainGrid.Children.Add(clientScroll,0,1);
            }


            void clientNew_Clicked(object sender0, EventArgs e0)
            {
                multiGrid.Children.Clear();
                multiGrid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                };

                Entry clientNameEntry = new Entry
                {
                    BackgroundColor = Color.Transparent,
                    Placeholder = "Nume Client",
                    //FontFamily = 
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
                    Background= Color.FromHex("#d9d9d9"),
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
                    order.SpecificCustomerName= ClientName;
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