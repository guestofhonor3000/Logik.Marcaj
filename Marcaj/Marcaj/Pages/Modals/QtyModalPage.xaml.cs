using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Modals
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QtyModalPage : ContentPage
	{
        OrderTransactionsModel item;
        //Button qtyBtn;
        List<int> digits = new List<int>();

        View numpad;
        float Qty;
        string sQty;

        public QtyModalPage (OrderTransactionsModel orderItem)
		{
			InitializeComponent();
            item = orderItem;
            Qty = item.Quantity;
            //qtyBtn = a;
            qty.Placeholder = "x" + item.Quantity.ToString();
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
            makeNumpad();
            multiGrid.Children.Add(numpad, 0, 1);
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
                sQty += numPressed.ToString();
                qty.Text = "x" + sQty;
                Debug.WriteLine(sQty);
                Debug.WriteLine(Qty);
            }
            else if (numPressed == 11)
            {
                sQty = "1";
                qty.Text = "x" + sQty;
            }
            else if (numPressed == 12)
            {
                Qty = Convert.ToSingle(sQty);

                if (Qty != item.Quantity && Qty != 0)
                {
                    item.Quantity = Qty;
                    item.ExtendedPrice = item.MenuItemUnitPrice * item.Quantity;
                    //qtyBtn.Text = item.Quantity.ToString();
                }
                else if (Qty == 0)
                {
                    Qty = 1;
                    item.Quantity = Qty;
                    item.ExtendedPrice = item.MenuItemUnitPrice * item.Quantity;
                    //qtyBtn.Text = item.Quantity.ToString();
                }
                else if (Qty == item.Quantity) 
                {
                    item.ExtendedPrice = item.MenuItemUnitPrice * item.Quantity;
                    //qtyBtn.Text = item.Quantity.ToString();
                }

                await Navigation.PopModalAsync();
            }       
        }

        private async void closeModal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}