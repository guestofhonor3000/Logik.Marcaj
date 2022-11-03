using Marcaj.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Settings.Statie
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemePage : ContentPage
    {
        public ThemePage()
        {
            InitializeComponent();
        }

        private void btnLight_Clicked(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new LightThemeStyle());


        }

        private void btnDark_Clicked(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new DarkThemeStyle());

        }
    }
}