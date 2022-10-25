using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Settings.Mese
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurarePozitiaMeselorPage : ContentPage
    {
        public ObservableCollection<TableLayoutModel> tblLayout;
        int GroupId;
        List<DineInTableModel> dineIns;
        public ConfigurarePozitiaMeselorPage(int tblGroupId)
        {
            InitializeComponent();
            dineIns = new List<DineInTableModel>();
            GroupId = tblGroupId;
            PopList();
        }

        async void PopList()
        {
            List<string> lstPckr = new List<string>();
            lstPckr.Add("10x8");
            lstPckr.Add("8x9");
            lstPckr.Add("20x10");
            gridPicker.ItemsSource = lstPckr;
           
            gridPicker.SelectedIndex = 0;
            tblLayout = new ObservableCollection<TableLayoutModel>();
            int col = Convert.ToInt32(gridPicker.SelectedItem.ToString().Split('x')[0]);
            int row = Convert.ToInt32(gridPicker.SelectedItem.ToString().Split('x')[1]);
            int nrPos = col * row;
            for (int i = 0; i < Convert.ToInt32(nrPos); i++)
            {
                var model = new TableLayoutModel();

                model.Position = (i + 1).ToString();
                model.Text = "";
                model.Visible = false;

                tblLayout.Add(model);
            }
            tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical)
            {
                VerticalItemSpacing = 5,
                HorizontalItemSpacing = 5
            };
            tblLayoutColl.ItemsSource = tblLayout;
            dineIns = await App.manager.iGetOnlyDineInTablesByTableGroup(GroupId);
        }

        private async void btnTbl2_Clicked(object sender, EventArgs e)
        {
            var dineIns2 = dineIns.Where(x => x.MaxGuests == 2).ToList();
            if(dineIns2.Count >= tblLayoutColl.SelectedItems.Count)
            {
                int i = 0;
                foreach (var a in tblLayoutColl.SelectedItems)
                {
                    
                    var b = a as TableLayoutModel;
                    var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                    c.Text = "Table2Open.png";
                    c.TableText = dineIns2[i].DineInTableText;
                    c.Visible = true;
                    tblLayoutColl.ItemsSource = null;
                    tblLayoutColl.ItemsSource = tblLayout;
                    i++;
                }

                tblLayoutColl.SelectedItems = null;
            }
            else
            {
                await DisplayAlert("Error", "Too many tables selected. Try Again!", "Ok");
            }
        }

        private async void btnTbl4_Clicked(object sender, EventArgs e)
        {
            var dineIns4 = dineIns.Where(x => x.MaxGuests == 4).ToList();
            if (dineIns4.Count >= tblLayoutColl.SelectedItems.Count)
            {
                int i = 0;
                foreach (var a in tblLayoutColl.SelectedItems)
                {

                    var b = a as TableLayoutModel;
                    var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                    c.Text = "Table4Open.png";
                    c.TableText = dineIns4[i].DineInTableText;
                    c.Visible = true;
                    tblLayoutColl.ItemsSource = null;
                    tblLayoutColl.ItemsSource = tblLayout;
                    i++;
                }

                tblLayoutColl.SelectedItems = null;
            }
            else
            {
                await DisplayAlert("Error", "Too many tables selected. Try Again!", "Ok");
            }
        }

        private async void btnTbl6_Clicked(object sender, EventArgs e)
        {
            var dineIns6 = dineIns.Where(x => x.MaxGuests == 6).ToList();
            if (dineIns6.Count >= tblLayoutColl.SelectedItems.Count)
            {
                int i = 0;
                foreach (var a in tblLayoutColl.SelectedItems)
                {

                    var b = a as TableLayoutModel;
                    var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                    c.Text = "Table6Open.png";
                    c.TableText = dineIns6[i].DineInTableText;
                    c.Visible = true;
                    tblLayoutColl.ItemsSource = null;
                    tblLayoutColl.ItemsSource = tblLayout;
                    i++;
                }

                tblLayoutColl.SelectedItems = null;
            }
            else
            {
                await DisplayAlert("Error", "Too many tables selected. Try Again!", "Ok");
            }
        }

        private async void btnTbl8_Clicked(object sender, EventArgs e)
        {
            var dineIns8 = dineIns.Where(x => x.MaxGuests == 8).ToList();
            if (dineIns8.Count >= tblLayoutColl.SelectedItems.Count)
            {
                int i = 0;
                foreach (var a in tblLayoutColl.SelectedItems)
                {

                    var b = a as TableLayoutModel;
                    var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                    c.Text = "Table8Open.png";
                    c.TableText = dineIns8[i].DineInTableText;
                    c.Visible = true;
                    tblLayoutColl.ItemsSource = null;
                    tblLayoutColl.ItemsSource = tblLayout;
                    i++;
                }

                tblLayoutColl.SelectedItems = null;
            }
            else
            {
                await DisplayAlert("Error", "Too many tables selected. Try Again!", "Ok");
            }
        }

        private void btnDone_Clicked(object sender, EventArgs e)
        {

        }

        private void gridPicker_SelectedIndexChanged(object sender, EventArgs e)
        { 
            tblLayout = new ObservableCollection<TableLayoutModel>();
            int col = Convert.ToInt32(gridPicker.SelectedItem.ToString().Split('x')[0]);
            int row = Convert.ToInt32(gridPicker.SelectedItem.ToString().Split('x')[1]);
            int nrPos = col * row;
            for (int i = 0; i < Convert.ToInt32(nrPos); i++)
            {
                var model = new TableLayoutModel();

                model.Position = (i + 1).ToString();
                model.Text = "";
                model.Visible = false;

                tblLayout.Add(model);
            }
            tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical)
            {
                VerticalItemSpacing = 5,
                HorizontalItemSpacing = 5
            };
            tblLayoutColl.ItemsSource = tblLayout;
            tblLayoutColl.SelectionMode = SelectionMode.Multiple;
        }

        private void tblLayoutColl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("clicked");
        }
    }
}