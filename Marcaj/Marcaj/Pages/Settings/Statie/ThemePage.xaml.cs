using Marcaj.Models.DbModels;
using Marcaj.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Settings.Statie
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemePage : ContentPage
    {
        StationSettingsModel model;
        public ThemePage()
        {
            InitializeComponent();
            PopSettings();
        }

        async void PopSettings()
        {
            string device = DeviceInfo.Name;
            string deviceName = " ";
            if (device.Contains("_"))
            {
                deviceName = device.Replace('_', ' ');
            }
            else
            {
                deviceName = device;
            }
            if (device.Length > 20)
            {
                deviceName = device.Remove(19);
            }
            model = await App.manager.iGetStationSettings(deviceName);
        }
        private async void btnLight_Clicked(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new LightThemeStyle());

            model.Theme = "Light";

            await App.manager.iPutStationName(model);

        }

        private async void btnDark_Clicked(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new DarkThemeStyle());
            model.Theme = "Dark";

            await App.manager.iPutStationName(model);
        }
    }
}