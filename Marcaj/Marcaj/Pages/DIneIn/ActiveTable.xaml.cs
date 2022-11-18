using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
			InitializeComponent();
			orderHeader = new OrderHeadersModel();
			orderHeadersList = new List<OrderHeadersModel>();
			orderTransactionsList = new List<OrderTransactionsModel>();
			orderTransactionsListByOrderID = new List<OrderTransactionsModel>();
			EmpFile = empFile;
			DineIn = dineIn;
            tableName.Text = DineIn.DineInTableText;
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
			
			gridLists.Children.Clear();
			int index = 0;
			var rowIndex = 0;
			var collIndex = 0;

			foreach (var orderHeader in orderHeadersList)
			{
				ListView lstvwOrderHeader = new ListView
				{
					Header = new Grid
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

							},

					},

					Footer = new Label
					{
						Text = "Total: " + orderHeader.AmountDue.ToString(),
						HorizontalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.Center,
					},


				};

				
				lstvwOrderHeader.SelectionMode = ListViewSelectionMode.None;
				lstvwOrderHeader.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

				Grid gridHeader = lstvwOrderHeader.Header as Grid;
				Label lblFooter = lstvwOrderHeader.Footer as Label;

				lblFooter.SetDynamicResource(StyleProperty, "checkLabel");

				Label txtServer = new Label
				{
					Text = "Server: " + "Name",
					HorizontalOptions = LayoutOptions.Start,
					HorizontalTextAlignment = TextAlignment.Start
				};
				txtServer.SetDynamicResource(StyleProperty, "checkLabel");
				gridHeader.Children.Add(txtServer, 0, 0);


				Label txtStation = new Label
				{
					Text = "Station: " + orderHeader.StationID.ToString(),
					HorizontalOptions = LayoutOptions.Start,
					HorizontalTextAlignment = TextAlignment.Start
				};
				txtStation.SetDynamicResource(StyleProperty, "checkLabel");
				gridHeader.Children.Add(txtStation, 0, 1);

				Frame orderFrame = new Frame
				{
					BackgroundColor = Color.White,
					BorderColor = Color.Gray,
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center,
					Padding = new Thickness(0),

				};
				gridHeader.Children.Add(orderFrame, 1, 0);
				Grid.SetRowSpan(orderFrame, 2);
				Label txtOrderName = new Label
				{
					Text = "Order: " + orderHeader.OrderID.ToString(),
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
					Padding = new Thickness(10)
				};
				txtOrderName.SetDynamicResource(StyleProperty, "checkLabel");
				orderFrame.Content = txtOrderName;


				Label txtTableName = new Label
				{
					Text = "Table: " + DineIn.DineInTableText,
					HorizontalOptions = LayoutOptions.End,
					HorizontalTextAlignment = TextAlignment.End
				};
				txtTableName.SetDynamicResource(StyleProperty, "checkLabel");
				gridHeader.Children.Add(txtTableName, 2, 0);


				Label txtDateTimeOpenedTable = new Label
				{
					Text = orderHeader.OrderDateTime.ToString(),
					HorizontalOptions = LayoutOptions.End,
					HorizontalTextAlignment = TextAlignment.End
				};
				txtDateTimeOpenedTable.SetDynamicResource(StyleProperty, "checkLabel");
				gridHeader.Children.Add(txtDateTimeOpenedTable, 2, 1);



				Grid itemListHeader = new Grid
				{
					ColumnDefinitions =
							{
								new ColumnDefinition{Width = GridLength.Star},
								new ColumnDefinition{Width=GridLength.Star},
								new ColumnDefinition{Width = GridLength.Star}
							}
				};

				gridHeader.Children.Add(itemListHeader, 0, 2);
				Grid.SetColumnSpan(itemListHeader, 3);


				Label txtQty = new Label
				{
					Text = "Qty",
					HorizontalTextAlignment = TextAlignment.Start
				};
				txtQty.SetDynamicResource(StyleProperty, "checkLabel");

				itemListHeader.Children.Add(txtQty, 0, 0);


				Label txtItem = new Label
				{
					Text = "Item",
					HorizontalTextAlignment = TextAlignment.Center,
				};
				txtItem.SetDynamicResource(StyleProperty, "checkLabel");
				itemListHeader.Children.Add(txtItem, 1, 0);


				Label txtValue = new Label
				{
					Text = "$",
					HorizontalTextAlignment = TextAlignment.End,
				};
				txtValue.SetDynamicResource(StyleProperty, "checkLabel");
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
						HorizontalTextAlignment = TextAlignment.Start,
					};
					qtyLabel.SetDynamicResource(StyleProperty, "checkLabel");
					qtyLabel.SetBinding(Label.TextProperty, "Quantity");
					grid.Children.Add(qtyLabel, 0, 0);


					Label itemLabel = new Label
					{
						HorizontalTextAlignment = TextAlignment.Center,
					};
					itemLabel.SetDynamicResource(StyleProperty, "checkLabel");
					itemLabel.SetBinding(Label.TextProperty, "MenuItemTextOT");
					grid.Children.Add(itemLabel, 1, 0);


					Label extPriceLabel = new Label
					{
						HorizontalTextAlignment = TextAlignment.End,
					};
					extPriceLabel.SetDynamicResource(StyleProperty, "checkLabel");
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

				Frame checkFrame = new Frame
				{
					CornerRadius = 10,
					Margin = 10,
					BackgroundColor = Color.FromHex("#d9d9d9")
				};

				checkFrame.Content = lstvwOrderHeader;
				checkFrame.HeightRequest = 400;

				
               //lstvwOrderHeader.Focused += new EventHandler<FocusEventArgs>(ListView_Focused);

				checkFrame.Focused += new EventHandler<FocusEventArgs>(ListView_Focused);
                lstvwOrderHeader.ClassId = orderHeader.OrderID.ToString();
                checkFrame.AutomationId = lstvwOrderHeader.ClassId;
				

                gridLists.Children.Add(checkFrame);

				index = index + 1;

				if (index % 5 != 0)
				{
					gridLists.Children.Add(checkFrame, collIndex, rowIndex);
					collIndex++;

				}
				else if (index % 5 == 0)
				{
					rowIndex++;
					collIndex = 0;
					gridLists.Children.Add(checkFrame, collIndex, rowIndex);
					collIndex++;
				}

				if (index % 9 == 0)
				{
					gridGridLists.Children.Add(gridLists = new Grid
					{
						RowDefinitions = new RowDefinitionCollection
						{
							new RowDefinition{Height = GridLength.Star},
						},

						ColumnDefinitions = new ColumnDefinitionCollection
						{
							new ColumnDefinition{Width=GridLength.Star},
							new ColumnDefinition{Width=GridLength.Star},
							new ColumnDefinition{Width=GridLength.Star},
							new ColumnDefinition{Width=GridLength.Star}
						}
					});
					index =	0;
					collIndex = 0;
					rowIndex = 0;
					gridLists.Children.Add(checkFrame, collIndex, rowIndex);
				}
			}


		}

		private async void btnNewOrder_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new NotActiveTable(DineIn, EmpFile, "opened"));
		}



		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AllTables(EmpFile));
		}
		int index;
		private async void ListView_Focused(object sender, EventArgs e)
		{
			index++;
			if (index == 1)
			{
				var lstFrame = sender as Frame;
				var orderId = lstFrame.AutomationId;
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