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

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            var tblName = await DisplayPromptAsync("Add Table", "Insert name", "Next", "Cancel", "Table Text", -1, null, "");
            if(tblName!="")
            {
                var maxGuests = await DisplayPromptAsync("Add Table", "Insert Max Guests", "Next", "Cancel", "Guests", -1, Keyboard.Numeric, "");
                if(maxGuests!="")
                {
                    var model = new DineInTableModel();

                    model.MaxGuests = Convert.ToInt32(maxGuests);
                    model.DineInTableText = tblName;
                    model.TableGroupID =GroupId;
                    model.DineInTableInActive = false;
                    model.DisplayIndex = 0;
                    model.RowGUID = Guid.NewGuid().ToString();

                    await App.manager.iPostDineInTable(model);

                    PopList(GroupId);
                }  
            }
        }
    }
}