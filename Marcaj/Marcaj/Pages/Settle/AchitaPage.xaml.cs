using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Marcaj.Models.LocalDbModels;
using Marcaj.Pages.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchitaPage : ContentPage
    {
        EmployeeFileModel EmplFl;
        public ObservableCollection<OptionsModel> menuBtnList;
        public AchitaPage(EmployeeFileModel emplFl)
        {
            InitializeComponent();
            EmplFl = emplFl;
            PopList("Default");
            menuBtnList = new ObservableCollection<OptionsModel>
            {
            new OptionsModel { Text="DineIn" , Image="DineInIcon.png"},
            new OptionsModel { Text="Bar", Image="BarIcon.png" },
            new OptionsModel { Text="Achita", Image="PaymentIcon.png"},
            new OptionsModel { Text="Anulare", Image="VoidIcon.png"},
            new OptionsModel { Text="Rechemare", Image="Recallicon.png"},
            new OptionsModel { Text="No Sale", Image="NoSaleIcon.png"},
            new OptionsModel { Text="Payback", Image="PaybackIcon.png"},
            new OptionsModel { Text="Placeholder", Image="PlaceholderIcon.png"},
            };
            menuBtnLst.ItemsSource = menuBtnList;
        }

        async void PopList(string type)
        {
            if(type == "Default")
            {
                var listActive = await App.manager.iGetActiveOrderHeaders();
                tabList.ItemsSource = listActive;
            }
            if(type == "Bar")
            {
                var listBar = await App.manager.iGetActiveOrderHeadersBar();
                tabList.ItemsSource = listBar;
            }
            if(type == "Emp")
            {
                var listEmp = await App.manager.iGetActiveOrderHeadersByEmpId(EmplFl.EmployeeID);
                tabList.ItemsSource = listEmp;
            }
            if(type == "Restaurant")
            {
                var listRest = await App.manager.iGetActiveOrderHeadersRestaurant();
                tabList.ItemsSource = listRest;
            }
        }

        private async void btnMese_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllTables(EmplFl));
        }

        private async void btnAchita_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchitaPage(EmplFl));
        }

        private void btnToate_Clicked(object sender, EventArgs e)
        {
            PopList("Default");
        }

        private void btnDineIn_Clicked(object sender, EventArgs e)
        {
            PopList("Restaurant");
        }

        private void btnBar_Clicked(object sender, EventArgs e)
        {
            PopList("Bar");
        }

        private void btnEmp_Clicked(object sender, EventArgs e)
        {
            PopList("Emp");
        }

        private void orderNr_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn7_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "7";
        }

        private void btn8_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "8";
        }

        private void btn9_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "9";
        }
        private void btn0_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "0";
        }

        private void btn4_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "4";
        }

        private void btn5_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "5";
        }

        private void btn6_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "6";
        }

        private void btn1_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "1";
        }

        private void btn2_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "2";
        }

        private void btn3_Clicked(object sender, EventArgs e)
        {
            orderNr.Text += "3";
        }

        private void btnDel_Clicked(object sender, EventArgs e)
        {
            string s = orderNr.Text.Substring(0, orderNr.Text.Length - 1);
            orderNr.Text = s;
        }
        private void btnOk_Clicked(object sender, EventArgs e)
        {

        }

        private async void menuBtnLst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {


            var current = e.SelectedItem as OptionsModel;

            if (((ListView)sender).SelectedItem == null)
                return;


            if (current.Text == "DineIn")
            {
                await Navigation.PushAsync(new AllTables(EmplFl));

            }
            else if (current.Text == "Bar")
            {


            }
            else if (current.Text == "Achita")
            {
                await Navigation.PushAsync(new AchitaPage(EmplFl));

            }
            else if (current.Text == "Anulare")
            {


            }
            else if (current.Text == "Rechemare")
            {


            }
            else if (current.Text == "No Sale")
            {


            }
            else if (current.Text == "Placeholder")
            {


            }

                  ((ListView)sender).SelectedItem = null;
        }

    }


}