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

        private void btnTbl2_Clicked(object sender, EventArgs e)
        {
            foreach (var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "Table2Open.png";
                c.Visible = true;
                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;
            }

            tblLayoutColl.SelectedItems = null;
        }

        private void btnTbl4_Clicked(object sender, EventArgs e)
        {
            foreach (var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "Table4Open.png"; c.Visible = true;

                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;
            }
            tblLayoutColl.SelectedItems = null;
        }

        private void btnTbl6_Clicked(object sender, EventArgs e)
        {
            foreach (var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "Table6Open.png"; c.Visible = true;

                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;
            }
            tblLayoutColl.SelectedItems = null;
        }

        private void btnTbl8_Clicked(object sender, EventArgs e)
        {
            foreach (var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "Table8Open.png"; c.Visible = true;

                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;
            }
            tblLayoutColl.SelectedItems = null;
        }

        private void btnDone_Clicked(object sender, EventArgs e)
        {

        }

        private void gridPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            //tblLayoutColl.SelectionMode = SelectionMode.Multiple;
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
        }

        private void tblLayoutColl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("clicked");
        }
    }
}