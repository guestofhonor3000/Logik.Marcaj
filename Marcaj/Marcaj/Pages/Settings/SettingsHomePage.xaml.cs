using Marcaj.Models.CustomModels;
using Marcaj.Pages.Settings.Mese;
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
        public ObservableCollection<MeseOptionsModel> meseOpt;
        public SettingsHomePage()
        {
            InitializeComponent();
            meseOpt = new ObservableCollection<MeseOptionsModel>
            {
            new MeseOptionsModel { Text="Mese Active/Inactive"},
            new MeseOptionsModel { Text="Grupuri" }
        };
            optionsList.ItemsSource = meseOpt;
        }
        private async void btnMeseConf_Clicked(object sender, EventArgs e)
        {
            optionsList.ItemsSource = meseOpt;
        }

        
        private async void optionsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            
                   var current = e.SelectedItem as MeseOptionsModel;
                
                  if (((ListView)sender).SelectedItem == null)
                  return;
            
               
                   if (current.Text == "Mese Active/Inactive")
                    {
                        await Navigation.PushAsync(new ConfigurareMesePage());
                
                    }
                    else if(current.Text == "Grupuri")
                    {
                        await Navigation.PushAsync(new ConfigurareGrupuriPage());
        
                    }

                   ((ListView)sender).SelectedItem = null;
        }
    }
}