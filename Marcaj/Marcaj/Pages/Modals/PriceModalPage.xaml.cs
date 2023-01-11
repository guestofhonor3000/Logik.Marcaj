using Marcaj.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PriceModalPage : ContentPage
    {
        OrderTransactionsModel item;
        //Button priceBtn;
        List<int> digits = new List<int>();
        bool discount;
        bool surcharge;
        float ModPrice;
        string sModPrice;
        View numpad;
        public PriceModalPage( OrderTransactionsModel orderItem )
        {
            InitializeComponent();
            item = orderItem;
            ModPrice = item.MenuItemUnitPrice;
            //priceBtn = a;
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
            multiShow();
        }


        private async void multiShow()
        {
            multiGrid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                };

            Button Discount = new Button
            {
                Text = "Discount",
                Margin = new Thickness(20, 10, 20, 10),
            };
            Discount.SetDynamicResource(StyleProperty, "btn");

            Button Surcharge = new Button
            {
                Text = "Suprataxa",
                Margin = new Thickness(20, 10, 20, 10),
            };
            Surcharge.SetDynamicResource(StyleProperty, "btn");

            Discount.Clicked += Discount_Clicked;
            Surcharge.Clicked += Surcharge_Clicked;

            void Discount_Clicked(object sender0, EventArgs e0)
            {
                surcharge = false;
                discount = true;
                multiGrid.Children.Clear();
                multiGrid.RowDefinitions = new RowDefinitionCollection
                    {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    };

                Button PerId = new Button
                {
                    Text = "Procent ID",
                    Margin = new Thickness(20, 10, 20, 10),
                };
                PerId.SetDynamicResource(StyleProperty, "btn");

                Button Cash = new Button
                {
                    Text = "Cash",
                    Margin = new Thickness(20, 10, 20, 10),
                };
                Cash.SetDynamicResource(StyleProperty, "btn");

                PerId.Clicked += PerId_Clicked;
                Cash.Clicked += Cash_Clicked;

                void PerId_Clicked(object sender1, EventArgs e1)
                {
                    multiGrid.Children.Clear();

                    Label IdsList = new Label
                    {
                        Text = "Lista Discount:",
                    };
                    IdsList.SetDynamicResource(StyleProperty, "mainBtnLabel");

                    multiGrid.Children.Add(IdsList);
                }

                void Cash_Clicked(object sender2, EventArgs e2)
                {
                    multiGrid.Children.Clear();
                    multiGrid.RowDefinitions = new RowDefinitionCollection
                        {
                        new RowDefinition { Height = GridLength.Star },
                        new RowDefinition { Height = new GridLength (4, GridUnitType.Star) },
                        };

                    makeNumpad();
                    price.Placeholder = "Discount in LEI";
                    priceFrame.IsVisible = true;
                    multiGrid.Children.Add(priceFrame, 0, 0);
                    multiGrid.Children.Add(numpad, 0, 1);
                }

                multiGrid.Children.Add(PerId, 0, 0);
                multiGrid.Children.Add(Cash, 0, 1);
            }

            void Surcharge_Clicked(object sender3, EventArgs e3)
            {
                discount = false;
                surcharge = true;
                multiGrid.Children.Clear();
                multiGrid.RowDefinitions = new RowDefinitionCollection
                    {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    };

                Button PerId = new Button
                {
                    Text = "Procent ID",
                    Margin = new Thickness(20, 10, 20, 10),
                };
                PerId.SetDynamicResource(StyleProperty, "btn");

                Button Cash = new Button
                {
                    Text = "Cash",
                    Margin = new Thickness(20, 10, 20, 10),
                };
                Cash.SetDynamicResource(StyleProperty, "btn");

                PerId.Clicked += PerId_Clicked;
                Cash.Clicked += Cash_Clicked;

                void PerId_Clicked(object sender1, EventArgs e1)
                {
                    multiGrid.Children.Clear();

                    Label IdsList = new Label
                    {
                        Text = "Lista Suprataxe:",
                    };
                    IdsList.SetDynamicResource(StyleProperty, "mainBtnLabel");

                    multiGrid.Children.Add(IdsList);
                }

                void Cash_Clicked(object sender2, EventArgs e2)
                {
                    multiGrid.Children.Clear();
                    multiGrid.RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = GridLength.Star },
                        new RowDefinition { Height = new GridLength (4, GridUnitType.Star) },
                    };

                    makeNumpad();
                    price.Placeholder = "Suprataxa in LEI";
                    priceFrame.IsVisible = true;
                    multiGrid.Children.Add(priceFrame, 0, 0);
                    multiGrid.Children.Add(numpad, 0, 1);
                }

                multiGrid.Children.Add(PerId, 0, 0);
                multiGrid.Children.Add(Cash, 0, 1);
            }
            multiGrid.Children.Add(Discount, 0, 0);
            multiGrid.Children.Add(Surcharge, 0, 1);
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
                sModPrice += numPressed.ToString();
                price.Text = sModPrice;
                Debug.WriteLine(sModPrice);
                Debug.WriteLine(ModPrice);
            }
            else if (numPressed == 11)
            {
                sModPrice = "0";
                price.Text = sModPrice;
            }
            else if (numPressed == 12)
            {
                ModPrice = Convert.ToSingle(sModPrice);

                if (discount)
                {
                    if (ModPrice != 0 && ModPrice < item.MenuItemUnitPrice)
                    {
                        //item.DiscountAmount = ModPrice;
                        item.ExtendedPrice = (item.MenuItemUnitPrice - ModPrice) * item.Quantity;
                        
                    }
                    else if (ModPrice == 0) item.ExtendedPrice = item.ExtendedPrice;
                }
                else if (surcharge)
                {
                    if (ModPrice != 0)
                    {
                        item.ExtendedPrice = (item.MenuItemUnitPrice + ModPrice) * item.Quantity;
                        
                    }
                    else if (ModPrice == 0) item.ExtendedPrice = item.ExtendedPrice;
                }
                //priceBtn.Text = item.ExtendedPrice.ToString();
                await Navigation.PopModalAsync();
            }       
        }

        private async void closeModal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}