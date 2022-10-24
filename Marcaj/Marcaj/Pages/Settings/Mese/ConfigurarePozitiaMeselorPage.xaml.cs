using Marcaj.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ConfigurarePozitiaMeselorPage(int tblGroupId)
        {
            InitializeComponent();
            GroupId = tblGroupId;
            PopList();
        }

        async void PopList()
        {


            //tblLayoutColl.ItemsLayout = { new GridItemsLayout {Span=11 } };



            //await App.manager.iGetDineInTablesByTableGroup(GroupId);
            List<string> lstPckr = new List<string>();
            lstPckr.Add("10x8");
            lstPckr.Add("20x10");
            gridPicker.ItemsSource = lstPckr;
            gridPicker.SelectedIndex = 0;
            tblLayout = new ObservableCollection<TableLayoutModel>();
            int col = Convert.ToInt32(gridPicker.SelectedItem.ToString().Split('x')[0]);
            int row = Convert.ToInt32(gridPicker.SelectedItem.ToString().Split('x')[1]);
            int nrPos = col * row;
            for (int i=0; i<Convert.ToInt32(nrPos);i++)
            {
                var model = new TableLayoutModel();

                model.Position = (i + 1).ToString();
                model.Text = "";
                model.Visible = false;

                tblLayout.Add(model);
            }
            tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical);
            tblLayoutColl.ItemsSource=tblLayout;
        }

        private void btnTbl2_Clicked(object sender, EventArgs e)
        {

        }

        private void btnTbl4_Clicked(object sender, EventArgs e)
        {

        }

        private void btnTbl6_Clicked(object sender, EventArgs e)
        {

        }

        private void btnTbl8_Clicked(object sender, EventArgs e)
        {

        }

        private void btnDone_Clicked(object sender, EventArgs e)
        {

        }

        private void gridPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            tblLayoutColl.ItemsSource = null;
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
            tblLayoutColl.ItemsLayout = new GridItemsLayout(col, ItemsLayoutOrientation.Vertical);

            tblLayoutColl.ItemsSource = tblLayout;
        }
    }
}