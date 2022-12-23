using Marcaj.Models.CustomModels;
using Marcaj.Pages.Settings.Mese;
using Marcaj.Pages.Settings.Statie;
using Marcaj.Pages.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsHomePage : ContentPage
    {
        public ObservableCollection<OptionsModel> meseOpt;
        public ObservableCollection<OptionsModel> stationOpt;
        public ObservableCollection<OptionsModel> menuBtnList;
        public SettingsHomePage()
        {
            InitializeComponent();
            meseOpt = new ObservableCollection<OptionsModel>
            {
            new OptionsModel { Text="Mese Active/Inactive"},
            new OptionsModel { Text="Grupuri" }
            };
            stationOpt = new ObservableCollection<OptionsModel>
            {
            new OptionsModel {Text ="Statie"},
            new OptionsModel {Text ="Tema"}
            };
            optionsList.ItemsSource = meseOpt;

            menuBtnList = new ObservableCollection<OptionsModel>
            {
            new OptionsModel { Text="Setari Mese", Image="DineInIcon.png"},
            new OptionsModel { Text="Setari Statie", Image="PlaceholderIcon.png" }
            };
            menuBtnLst.ItemsSource = menuBtnList;
        }


        private async void optionsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {


            var current = e.SelectedItem as OptionsModel;

            if (((ListView)sender).SelectedItem == null)
                return;


            if (current.Text == "Mese Active/Inactive")
            {
                await Navigation.PushAsync(new ConfigurareMesePage());

            }
            else if (current.Text == "Grupuri")
            {
                await Navigation.PushAsync(new ConfigurareGrupuriPage());

            }
            else if (current.Text == "Statie")
            {
                await Navigation.PushAsync(new StationSettingsMainPage());

            }
            else if (current.Text == "Tema")
            {
                await Navigation.PushAsync(new ThemePage());

            }

                   ((ListView)sender).SelectedItem = null;
        }

        private void menuBtnLst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {


            var current = e.SelectedItem as OptionsModel;

            if (((ListView)sender).SelectedItem == null)
                return;


            if (current.Text == "Setari Mese")
            {
                optionsList.ItemsSource = meseOpt;

            }
            else if (current.Text == "Setari Statie")
            {
                optionsList.ItemsSource = stationOpt;

            }

                  ((ListView)sender).SelectedItem = null;
        }

    
    }
}