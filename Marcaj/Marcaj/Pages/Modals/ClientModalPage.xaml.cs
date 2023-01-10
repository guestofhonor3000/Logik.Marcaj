using Marcaj.Models.DbModels;
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
        List<OrderHeadersModel> ordersList;
        string ClientName;
		public ClientModalPage(OrderHeadersModel orderHeader, List<OrderHeadersModel> orderHeadersList)
		{
			InitializeComponent ();
			order= orderHeader;
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
                Margin = new Thickness(0),
            };
            clientLookup.SetDynamicResource(StyleProperty, "btn");

            Button clientNew = new Button
            {
                Text = "Client Nou",
                Margin = new Thickness(0),
            };
            clientNew.SetDynamicResource(StyleProperty, "btn");

            clientLookup.Clicked += clientLookup_Clicked;
            clientNew.Clicked += clientNew_Clicked;

            void clientLookup_Clicked(object sender0, EventArgs e0)
            {
                int index = 0;

                multiGrid.Children.Clear();
                multiGrid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                };

                Label Title = new Label
                {
                    Text = "Lista Clienti:"
                };
                Title.SetDynamicResource(StyleProperty, "mainBtnLabel");

                foreach( var ord in ordersList) 
                {
                    index++;
                    Grid clientGrid = new Grid();
                    clientGrid.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                        new ColumnDefinition { Width = GridLength.Star },
                    };

                    Label clientName = new Label
                    {
                        Text = ord.SpecificCustomerName
                    };
                    clientName.SetDynamicResource(StyleProperty, "mainBtnLabel");

                    Label clientId = new Label
                    {
                        Text = ord.CustomerID.ToString()
                    };
                    clientId.SetDynamicResource(StyleProperty, "mainBtnLabel");

                    clientGrid.Children.Add(clientName,0 ,0);
                    clientGrid.Children.Add(clientId,1 ,0);

                    multiGrid.Children.Add((Grid)clientGrid,0 ,index);
                }
                multiGrid.Children.Add(Title,0 ,0);
            }


            void clientNew_Clicked(object sender0, EventArgs e0)
            {
                multiGrid.Children.Clear();
                multiGrid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                };

                Label Title = new Label
                {
                    Text = "Nume Client:"
                };
                Title.SetDynamicResource(StyleProperty, "mainBtnLabel");

                Entry clientNameEntry = new Entry
                {
                    BackgroundColor = Color.Transparent,
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
                    Margin = 0
                };
                clientNameFrame.Content = clientNameEntry;

                Button Done = new Button 
                {
                    Text = "Salveaza",
                    Margin = new Thickness(0),
                    BackgroundColor= Color.FromHex("#50e9da"),
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

                multiGrid.Children.Add(Title, 0, 0);
                multiGrid.Children.Add(clientNameFrame, 0, 1);
                multiGrid.Children.Add(Done, 0, 2);
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