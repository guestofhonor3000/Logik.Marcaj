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
        EmployeeFileModel EmplFl;
        DineInTableGroupModel dineInGroup;
        List<DineInTableModel> dineIns;
        public ConfigurarePozitiaMeselorPage(DineInTableGroupModel groupModel)
        {
            InitializeComponent();
            dineIns = new List<DineInTableModel>();
            dineInGroup = groupModel;
            PopList();
        }

        async void PopList()
        {

            List<string> lstPckr = new List<string>();
            lstPckr.Add("8x6");
            lstPckr.Add("8x9");
            lstPckr.Add("10x8");
           
            gridPicker.ItemsSource = lstPckr;
            if (dineInGroup.GridSize != "")
            {
                Debug.WriteLine(dineInGroup.GridSize);
                var index = lstPckr.IndexOf(dineInGroup.GridSize);
                gridPicker.SelectedIndex = index;
            }
            else
            {
                gridPicker.SelectedIndex = 0;
            }
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


            dineIns = await App.manager.iGetOnlyDineInTablesByTableGroup(dineInGroup.TableGroupID);
            foreach (var dine in dineIns)
            {
                if (dine.DisplayPosition != null)
                {
                    if(Convert.ToInt32(dine.DisplayPosition) <= (col*row))
                    {
                        if (dine.MaxGuests == 2)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                        else if (dine.MaxGuests == 4)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                        else if (dine.MaxGuests == 6)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                        else if (dine.MaxGuests == 8)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                    }
                }
            }

            tblLayoutColl.ItemsSource = tblLayout;
        }

        private async void btnTbl2_Clicked(object sender, EventArgs e)
        {
            var dineIns2 = dineIns.Where(x => x.MaxGuests == 2).ToList();
            foreach (var dineIn in dineIns2)
            {
                var toNull = tblLayout.Where(x => x.TableText == dineIn.DineInTableText).FirstOrDefault();
                if (toNull != null)
                {
                    toNull.Visible = false;
                    toNull.TableText = "";
                    toNull.Text = "";
                }
            }
            if (dineIns2.Count >= tblLayoutColl.SelectedItems.Count)
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
            foreach (var dineIn in dineIns4)
            {
                var toNull = tblLayout.Where(x => x.TableText == dineIn.DineInTableText).FirstOrDefault();
                if (toNull != null)
                {
                    toNull.Visible = false;
                    toNull.TableText = "";
                    toNull.Text = "";
                }
            }
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
            foreach (var dineIn in dineIns6)
            {
                var toNull = tblLayout.Where(x => x.TableText == dineIn.DineInTableText).FirstOrDefault();
                if (toNull != null)
                {
                    toNull.Visible = false;
                    toNull.TableText = "";
                    toNull.Text = "";
                }
            }
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
        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            if (tblLayout.Where(x => x.Text == "").ToList().Count == tblLayout.Count)
            {
                var model = new DineInTableGroupModel();
                model.GridSize = gridPicker.SelectedItem.ToString();
                await App.manager.iPutTableGroups(model, dineInGroup.TableGroupID);
            }
            else
            {
                var modelGr = new DineInTableGroupModel();
                modelGr.GridSize = gridPicker.SelectedItem.ToString();
                await App.manager.iPutTableGroups(modelGr, dineInGroup.TableGroupID);
                List<DineInTableModel> putLst = new List<DineInTableModel>();
                foreach (var tbl in tblLayout.Where(x => x.Text != "").ToList())
                {
                    var model = new DineInTableModel();
                    model.DisplayPosition = tbl.Position;
                    model.DineInTableID = dineIns.Where(x => x.DineInTableText == tbl.TableText).FirstOrDefault().DineInTableID;

                    putLst.Add(model);
                }
                foreach (var dineIn in dineIns)
                {
                    if (tblLayout.Where(x => x.TableText == dineIn.DineInTableText).FirstOrDefault() == null)
                    {
                        var model = new DineInTableModel();
                        model.DisplayPosition = null;
                        model.DineInTableID = dineIn.DineInTableID;

                        putLst.Add(model);
                    }
                }
                await App.manager.iPutDineInTablesPosition(putLst);
            }
            await Navigation.PopModalAsync();
        }
        private async void btnTbl8_Clicked(object sender, EventArgs e)
        {
            var dineIns8 = dineIns.Where(x => x.MaxGuests == 8).ToList();
            foreach (var dineIn in dineIns8)
            {
                var toNull = tblLayout.Where(x => x.TableText == dineIn.DineInTableText).FirstOrDefault();
                if (toNull != null)
                {
                    toNull.Visible = false;
                    toNull.TableText = "";
                    toNull.Text = "";
                }
            }
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

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            foreach(var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "";
                c.TableText = "";
                c.Visible = false;
                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;

            }
            tblLayoutColl.SelectedItems = null;
        }

        private async void gridPicker_SelectedIndexChanged(object sender, EventArgs e)
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
            tblLayoutColl.SelectionMode = SelectionMode.Multiple;
            dineIns = await App.manager.iGetOnlyDineInTablesByTableGroup(dineInGroup.TableGroupID);
            foreach (var dine in dineIns)
            {
                if (dine.DisplayPosition != null)
                {
                    if (Convert.ToInt32(dine.DisplayPosition) <= (col * row))
                    {
                        if (dine.MaxGuests == 2)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table2Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                        else if (dine.MaxGuests == 4)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table4Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                        else if (dine.MaxGuests == 6)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table6Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                        else if (dine.MaxGuests == 8)
                        {
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Visible = true;
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().Text = "Table8Open.png";
                            tblLayout.Where(x => x.Position == dine.DisplayPosition).FirstOrDefault().TableText = dine.DineInTableText;
                        }
                    }
                }
            }

            tblLayoutColl.ItemsSource = tblLayout;
        }

        
    }
}