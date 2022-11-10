using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Tables
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActiveTable : ContentPage
	{
		EmployeeFileModel EmpFile;
        DineInTableModel DineIn;
		List<OrderHeadersModel> orderHeadersList;
		List<OrderTransactionsModel> orderTransactionsList;
		List<OrderTransactionsModel> orderTransactionsListByOrderID;
        public ObservableCollection<OptionsModel> menuBtnList;
        int NumberOfClicksNext = 0;
		OrderHeadersModel orderHeader;
		public ActiveTable(DineInTableModel dineIn, EmployeeFileModel empFile)
		{
			InitializeComponent ();
			orderHeader = new OrderHeadersModel();
			orderHeadersList = new List<OrderHeadersModel>();
			orderTransactionsList = new List<OrderTransactionsModel>();
			orderTransactionsListByOrderID = new List<OrderTransactionsModel>();
			EmpFile = empFile;
			DineIn = dineIn;
			PopList();
			MessagingCenter.Subscribe<NotActiveTable>(this, "Up", (sender) => {
				NumberOfClicksNext = 0;
				PopList();
				GoBack();

            });
			MessagingCenter.Subscribe<ActiveTableEditPage>(this, "Up", (sender) => {
				NumberOfClicksNext = 0;
				PopList();
				GoBack();
            });
      
        }

		async void GoBack()
		{
			await Navigation.PopAsync();
		}
		async void PopListNext(int skipIf)
        {
			gridLists.Children.Clear();
			int index = 0;
			int skip = 0;

			if(orderHeadersList.Count<(skipIf+1)*3)
            {
				btnNextPage.IsEnabled = false;
            }
			else
            {
				btnNextPage.IsEnabled = true;
            }
			if(skipIf*3==0)
            {
				Debug.WriteLine(skipIf);
				btnPrevPage.IsEnabled = false;
            }
			else
            {
				btnPrevPage.IsEnabled = true;
			}
			foreach(var orderHeader in orderHeadersList)
            {
				
				skip = skip + 1;
				if(skip>skipIf*3)
                {
					
					index = index + 1;
					if (index < 4)
					{
						ListView lstvwOrderHeader = new ListView
						{
							Header = new Grid
							{
								ColumnDefinitions =
								{
								new ColumnDefinition { Width = GridLength.Star },
								new ColumnDefinition { Width = GridLength.Star }
								},
								RowDefinitions =
								{
								new RowDefinition { Height = GridLength.Star },
								new RowDefinition { Height = GridLength.Star },
								new RowDefinition { Height = GridLength.Star },
								new RowDefinition { Height = GridLength.Star }
								}
							}
						};

						lstvwOrderHeader.ClassId = orderHeader.OrderID.ToString();
						lstvwOrderHeader.Focused += new EventHandler<FocusEventArgs>(ListView_Focused);
						lstvwOrderHeader.SelectionMode = ListViewSelectionMode.None;

						Grid gridHeader = lstvwOrderHeader.Header as Grid;

						Label txtServer = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = "Server: " + "Name"
						};

						gridHeader.Children.Add(txtServer, 0, 0);

						Label txtStation = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = "Station: " + orderHeader.StationID.ToString()
						};

						gridHeader.Children.Add(txtStation, 1, 0);

						Label txtOrderName = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = "Order: " + orderHeader.OrderID.ToString()
						};

						gridHeader.Children.Add(txtOrderName, 0, 1);

						Label txtTableName = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = "Table: " + DineIn.DineInTableText
						};

						gridHeader.Children.Add(txtTableName, 1, 1);

						Label txtDateTimeOpenedTable = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = orderHeader.OrderDateTime.ToString()
						};

						gridHeader.Children.Add(txtDateTimeOpenedTable, 0, 2);
						Grid.SetColumnSpan(txtDateTimeOpenedTable, 2);

						Grid itemListHeader = new Grid
						{
							ColumnDefinitions =
							{
								new ColumnDefinition{Width = GridLength.Star},
								new ColumnDefinition{Width=GridLength.Star},
								new ColumnDefinition{Width = GridLength.Star}
							}
						};

						gridHeader.Children.Add(itemListHeader, 0, 3);
						Grid.SetColumnSpan(itemListHeader, 2);

						Label txtQty = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = "Qty",
							FontAttributes = FontAttributes.Bold
						};

						itemListHeader.Children.Add(txtQty, 0, 0);

						Label txtItem = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = "Item",
							FontAttributes = FontAttributes.Bold
						};

						itemListHeader.Children.Add(txtItem, 1, 0);

						Label txtValue = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
							Text = "$",
							FontAttributes = FontAttributes.Bold
						};

						itemListHeader.Children.Add(txtValue, 2, 0);

						lstvwOrderHeader.ItemTemplate = new DataTemplate(() =>
						{
							ViewCell cell = new ViewCell();

							Grid grid = new Grid();

							grid.ColumnDefinitions = new ColumnDefinitionCollection
						{
							new ColumnDefinition{Width=GridLength.Star},
							new ColumnDefinition{Width=GridLength.Star},
							new ColumnDefinition{Width=GridLength.Star}
						};

							Label qtyLabel = new Label
							{
								VerticalOptions = LayoutOptions.Center,
								HorizontalOptions = LayoutOptions.Center,
								TextColor = Color.Black
							};

							qtyLabel.SetBinding(Label.TextProperty, "Quantity");
							grid.Children.Add(qtyLabel, 0, 0);

							Label itemLabel = new Label
							{
								VerticalOptions = LayoutOptions.Center,
								HorizontalOptions = LayoutOptions.Center,
								TextColor = Color.Black
							};

							itemLabel.SetBinding(Label.TextProperty, "MenuItemTextOT");
							grid.Children.Add(itemLabel, 1, 0);

							Label extPriceLabel = new Label
							{
								VerticalOptions = LayoutOptions.Center,
								HorizontalOptions = LayoutOptions.Center,
								TextColor = Color.Black,
							};

							extPriceLabel.SetBinding(Label.TextProperty, "ExtendedPrice");
							grid.Children.Add(extPriceLabel, 2, 0);

							
							cell.View = grid;
							return cell;
						});

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
		}
		async void PopList()
		{
			orderHeadersList = await App.manager.iGetOrderHeadersByDineInTableID(DineIn.DineInTableID);
			if (orderHeadersList.Count>3)
            {
				btnNextPage.IsEnabled = true;
            }
			gridLists.Children.Clear();
			int index = 0;
			foreach (var orderHeader in orderHeadersList)
			{
				index = index + 1;
				if (index < 4)
				{
					ListView lstvwOrderHeader = new ListView {
						Header = new Grid
						{

							ColumnDefinitions =
							{
								new ColumnDefinition { Width = GridLength.Star },
								new ColumnDefinition { Width = GridLength.Star }
							},
							RowDefinitions =
							{
								new RowDefinition { Height = GridLength.Star },
								new RowDefinition { Height = GridLength.Star },
								new RowDefinition { Height = GridLength.Star },
								new RowDefinition { Height = GridLength.Star }
							},
						},
					};

					lstvwOrderHeader.ClassId = orderHeader.OrderID.ToString();
					lstvwOrderHeader.Focused += new EventHandler<FocusEventArgs>(ListView_Focused);
					lstvwOrderHeader.SelectionMode = ListViewSelectionMode.None;

					

					Grid gridHeader = lstvwOrderHeader.Header as Grid;

					Label txtServer = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = "Server: "+"Name"
					};

					gridHeader.Children.Add(txtServer, 0, 0);

					Label txtStation = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = "Station: "+orderHeader.StationID.ToString()
					};

					gridHeader.Children.Add(txtStation, 1, 0);

					Label txtOrderName = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = "Order: "+orderHeader.OrderID.ToString()
					};

					gridHeader.Children.Add(txtOrderName, 0, 1);

					Label txtTableName = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = "Table: "+DineIn.DineInTableText
					};

					gridHeader.Children.Add(txtTableName, 1, 1);

					Label txtDateTimeOpenedTable = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = orderHeader.OrderDateTime.ToString()
					};

					gridHeader.Children.Add(txtDateTimeOpenedTable, 0, 2);
					Grid.SetColumnSpan(txtDateTimeOpenedTable, 2);
					
					Grid itemListHeader = new Grid
					{
						ColumnDefinitions =
							{
								new ColumnDefinition{Width = GridLength.Star},
								new ColumnDefinition{Width=GridLength.Star},
								new ColumnDefinition{Width = GridLength.Star}
							}
					};

					gridHeader.Children.Add(itemListHeader, 0, 3);
					Grid.SetColumnSpan(itemListHeader, 2);

					Label txtQty = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = "Qty",
						FontAttributes = FontAttributes.Bold
					};

					itemListHeader.Children.Add(txtQty, 0, 0);

					Label txtItem = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = "Item",
						FontAttributes = FontAttributes.Bold
					};

					itemListHeader.Children.Add(txtItem, 1, 0);

					Label txtValue = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = Color.Black,
						Text = "$",
						FontAttributes = FontAttributes.Bold
					};

					itemListHeader.Children.Add(txtValue, 2, 0);

					lstvwOrderHeader.ItemTemplate = new DataTemplate(() =>
					{
						ViewCell cell = new ViewCell();
						
						Grid grid = new Grid();

						grid.ColumnDefinitions = new ColumnDefinitionCollection
						{
							new ColumnDefinition{Width=GridLength.Star},
							new ColumnDefinition{Width=GridLength.Star},
							new ColumnDefinition{Width=GridLength.Star}
						};

						Label qtyLabel = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black
						};

						qtyLabel.SetBinding(Label.TextProperty, "Quantity");
						grid.Children.Add(qtyLabel, 0, 0);

						Label itemLabel = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black
						};

						itemLabel.SetBinding(Label.TextProperty, "MenuItemTextOT");
						grid.Children.Add(itemLabel, 1, 0);

						Label extPriceLabel = new Label
						{
							VerticalOptions = LayoutOptions.Center,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = Color.Black,
						};

						extPriceLabel.SetBinding(Label.TextProperty, "ExtendedPrice");
						grid.Children.Add(extPriceLabel, 2, 0);


						cell.View = grid;
						return cell;
					});

					var orderTraItems = await App.manager.iGetOrderTransactionsByOrderID(orderHeader.OrderID);
					foreach(var orderTraItem in orderTraItems)
                    {
						orderTransactionsList.Add(orderTraItem);
                    }
					lstvwOrderHeader.ItemsSource = orderTraItems;

					gridLists.Children.Add(lstvwOrderHeader, index-1, 0);
				}
			}
		}

        private async void btnNewOrder_Clicked(object sender, EventArgs e)
        {
			await Navigation.PushAsync(new NotActiveTable(DineIn, EmpFile,"opened"));
        }

		private void btnNextPage_Clicked(object sender, EventArgs e)
        {
			NumberOfClicksNext = NumberOfClicksNext+1;
			PopListNext(NumberOfClicksNext);
		}

        private void btnPrevPage_Clicked(object sender, EventArgs e)
        {
			NumberOfClicksNext = NumberOfClicksNext - 1;
			PopListNext(NumberOfClicksNext);
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllTables(EmpFile));
        }
		int index;
        private async void ListView_Focused(object sender, FocusEventArgs e)
        {
			index++;
			if(index==1)
			{
                var lstvw = sender as ListView;
                var orderId = lstvw.ClassId;
                var orderHeaderFocused = orderHeadersList.Find(x => x.OrderID == Convert.ToInt32(orderId));
                orderHeader = orderHeaderFocused;
                orderTransactionsListByOrderID = orderTransactionsList.FindAll(x => x.OrderID == Convert.ToInt32(orderId));
				Debug.WriteLine(orderHeader.SynchVer);
				if(orderHeader.SynchVer != DateTime.MinValue)
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