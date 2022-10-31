using Marcaj.Models.DbModels;
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
    public partial class StationSettingsMainPage : ContentPage
    {
        StationSettingsModel statie;
        public StationSettingsMainPage()
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
            statie = await App.manager.iGetStationSettings(deviceName);
            if(statie.PopUpBool !=null)
            {
                chckboxPopUp.IsChecked = (bool)statie.PopUpBool;
            }
        }
        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            await App.manager.iPutPopUpSetting(statie, chckboxPopUp.IsChecked);
        }
    }
}