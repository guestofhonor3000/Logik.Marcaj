using Marcaj.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Settings.Mese
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurareGrupuriPage : ContentPage
    {
        public ConfigurareGrupuriPage()
        {
            InitializeComponent();
            PopList();
        }

        async void PopList()
        {
            var b = await App.manager.iGetDineInTableGroups();
            if (b != null)
            {
                lstvwGrupMese.ItemsSource = b;
            }
        }

        private async void lstvwGrupMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var a = e.SelectedItem as DineInTableGroupModel;
            var result = await DisplayActionSheet("What do you want to do?", "Cancel", "Close", "Delete", "Edit");
            if (result == "Edit")
            {
                var edit = await DisplayPromptAsync("Edit", "Choose a Name", "OK", "Cancel", a.TableGroupText);
                if (edit != null)
                {
                    if (edit != "Cancel")
                    {
                        a.TableGroupText = edit;
                        await App.manager.iPutTableGroups(a, a.TableGroupID);
                        PopList();
                    }
                }
            }
            else if (result == "Delete")
            {
                var prompt = await DisplayAlert("Delete", "Are you sure?", "OK", "Cancel");
                if (prompt == true)
                {
                    await App.manager.iDeleteTableGroup(a.TableGroupID);
                    PopList();
                }
            }
        }
    }
}