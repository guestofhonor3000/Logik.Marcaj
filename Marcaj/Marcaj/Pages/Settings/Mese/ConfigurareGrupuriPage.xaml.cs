using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
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

namespace Marcaj.Pages.Settings.Mese
{
   
    public class GridDefinitions
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurareGrupuriPage : ContentPage
    {
        int GroupId;
        Button btnAssigned;
        public ObservableCollection<GridDefinitions> gridDefs;
        public ObservableCollection<TableLayoutModel> tblLayout;
        public ConfigurareGrupuriPage()
        {
            InitializeComponent();
            PopList();
            
       

            MessagingCenter.Subscribe<ListMeseModalPage, string>(this, "Up", (sender, result) =>
            {
                btnAssigned.Text = result;
            });
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
            var result = await DisplayActionSheet("What do you want to do?", "Cancel", "Close", "Delete", "Edit", "Assign");
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
            else if(result == "Assign")
            {
                await Navigation.PushModalAsync(new ConfigurarePozitiaMeselorPage(a));
                //GroupId = a.TableGroupID;
            }
        }

        private async void btnAddTableGroup_Clicked(object sender, EventArgs e)
        {
            var name = await DisplayPromptAsync("Add", "Choose a Name", "Ok", "Cancel", "Table Group Name");
            if(name != null)
            {
                if(name != "Cancel")
                {
                    var model = new DineInTableGroupModel();

                    model.TableGroupText=name;
                    model.RowGUID = Guid.NewGuid().ToString();

                    await App.manager.iPostTableGroup(model);
                    PopList();
                }
            }
        }

        private async void aaCell_Clicked(object sender, EventArgs e)
        {
            btnAssigned = sender as Button;
            
            await Navigation.PushModalAsync(new ListMeseModalPage(GroupId));
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            Debug.WriteLine("Swiped");
        }

        private void btnGroupNameEdit_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnAssignTables_Clicked(object sender, EventArgs e)
        {
            var a = lstvwGrupMese.SelectedItem as DineInTableGroupModel;
            await Navigation.PushModalAsync(new ConfigurarePozitiaMeselorPage(a));
        }

        private async void btnDeleteGroup_Clicked(object sender, EventArgs e)
        {
            var a = lstvwGrupMese.SelectedItem as DineInTableGroupModel;
            var prompt = await DisplayAlert("Delete", "Are you sure?", "OK", "Cancel");
            if (prompt == true)
            {
                await App.manager.iDeleteTableGroup(a.TableGroupID);
                PopList();
            }
        }
    }
}