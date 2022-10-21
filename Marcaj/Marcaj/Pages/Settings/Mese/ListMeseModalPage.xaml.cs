using Marcaj.Models.DbModels;
using Marcaj.Pages.Tables;
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
    public partial class ListMeseModalPage : ContentPage
    {
       
        public ListMeseModalPage(int groupId)
        {
            InitializeComponent();
            PopList(groupId);
        }

        async void PopList(int GroupID)
        {
            var a = await App.manager.iGetDineInTablesAllByTableGroup(GroupID);
            if (a != null)
            {
                lstvwMese.ItemsSource = a;
            }
        }
        private async void lstvwMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var result = await DisplayAlert("Add", "Are you sure?", "Ok", "Cancel");
            var a = e.SelectedItem as DineInTableModel;
            if (result == true)
            {
                MessagingCenter.Send<ListMeseModalPage, string>(this, "Up", a.DineInTableText);
                await Navigation.PopModalAsync();
            }

        }
    }
}