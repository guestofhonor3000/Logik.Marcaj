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
            optionsListColl.ItemsSource = meseOpt;
        }
        private async void btnMeseConf_Clicked(object sender, EventArgs e)
        {
            optionsListColl.ItemsSource = meseOpt;
        }

        
        private async void optionsListColl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
            string current = (e.CurrentSelection.FirstOrDefault() as MeseOptionsModel)?.Text;
                   if (current == "Mese Active/Inactive")
                    {
                        await Navigation.PushAsync(new ConfigurareMesePage());
                    }
                    else if(current == "Grupuri")
                    {
                        await Navigation.PushAsync(new ConfigurareGrupuriPage());
                    }
             
        }
    }
}