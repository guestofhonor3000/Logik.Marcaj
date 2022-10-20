using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Marcaj.Pages.Tables;
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
    public partial class ConfigurareMesePage : ContentPage
    {
        int GroupId = 1;
        bool IsFirstLoad = true;
        public ConfigurareMesePage()
        {
            InitializeComponent();
            PopList(GroupId);
          
        }

        private void lstvwGrupMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selIt = e.SelectedItem as DineInTableGroupModel;
            GroupId = selIt.TableGroupID;
            IsFirstLoad = false;
            PopList(GroupId);
        }

        private async void lstvwMese_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var a = e.SelectedItem as DineInTableAndEmpModel;
            await App.manager.iPutDineInTable(a.DineIn, a.DineIn.DineInTableID);
            PopList(GroupId);
        }

        async void PopList(int GroupID)
        {
            if(IsFirstLoad==true)
            {
                var a = await App.manager.iGetDineInTablesAllByTableGroup(GroupID);
                if (a != null)
                {
                    lstvwMese.ItemsSource = a;
                }
                var b = await App.manager.iGetDineInTableGroups();
                if (b != null)
                {
                    lstvwGrupMese.ItemsSource = b;
                    lstvwGrupMese.SelectedItem = b[0];
                }
            }
            else
            {
                var a = await App.manager.iGetDineInTablesAllByTableGroup(GroupID);
                if (a != null)
                {
                    lstvwMese.ItemsSource = a;
                }
            }
          
        }
    }
}