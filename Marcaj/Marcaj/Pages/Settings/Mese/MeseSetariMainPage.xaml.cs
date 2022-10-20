using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Settings.Mese
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeseSetariMainPage : ContentPage
    {
        public MeseSetariMainPage()
        {
            InitializeComponent();
        }

        private async void btnMeseConf_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfigurareMesePage());
        }

        private async void btnGrupuriConf_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfigurareGrupuriPage());
        }
    }
}