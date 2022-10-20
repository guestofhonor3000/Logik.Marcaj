using Marcaj.Pages.Settings.Mese;
using System;
using System.Collections.Generic;
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
        public SettingsHomePage()
        {
            InitializeComponent();
        }
        private async void btnMeseConf_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MeseSetariMainPage());
        }
    }
}