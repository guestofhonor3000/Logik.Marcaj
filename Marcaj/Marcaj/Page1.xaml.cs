using Marcaj.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.GTKSpecific;
using Xamarin.Forms.Xaml;
using static Marcaj.Page1;

namespace Marcaj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public ObservableCollection<TableLayoutModel> tblLayout;
        public Page1()
        {
            InitializeComponent();
            tblLayout = new ObservableCollection<TableLayoutModel>
            {
            new TableLayoutModel { Position="1", Text="" , Visible=false},
            new TableLayoutModel { Position="2" , Text="", Visible=false},
            new TableLayoutModel { Position="3" , Text="", Visible=false},
            new TableLayoutModel { Position="4" , Text="", Visible=false},
            new TableLayoutModel { Position="5" , Text="", Visible = false},
            new TableLayoutModel { Position="6" , Text = "", Visible = false},
            new TableLayoutModel { Position="7" , Text = "", Visible = false},
            new TableLayoutModel { Position="8" , Text = "", Visible = false},
            new TableLayoutModel { Position="9" , Text = "", Visible = false},
            new TableLayoutModel { Position="10" , Text = "", Visible = false},
            new TableLayoutModel { Position="11" , Text = "", Visible = false},
            new TableLayoutModel { Position="12" , Text = "", Visible = false},
            new TableLayoutModel { Position="13" , Text = "", Visible = false},
            new TableLayoutModel { Position="14" , Text = "", Visible = false},
            new TableLayoutModel { Position="15" , Text = "", Visible = false},
            new TableLayoutModel { Position="16" , Text = "", Visible=false    },
            new TableLayoutModel { Position="17" , Text=""  , Visible=false},
            new TableLayoutModel { Position="18" , Text="", Visible=false},
            new TableLayoutModel { Position="19" , Text="" , Visible=false},
            new TableLayoutModel { Position="20", Text="", Visible=false }

            
        };
            /*private static void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
           {aoleuaoleu
               var previous = e.PreviousSelection;
               var current = e.CurrentSelection;
           }*/
            tblLayoutColl.ItemsSource = tblLayout;
        }

        private void getIndex(TableLayoutModel item)
        {
            int index = tblLayout.IndexOf(item);
        }

        private void btnTbl2_Clicked(object sender, EventArgs e)
        {
           
            foreach(var a in tblLayoutColl.SelectedItems)
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

        private void btnTblGrid_Clicked(object sender, EventArgs e)
        {

        }

        private void tblVect_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            foreach(var i in tblLayout)
            {
                Debug.WriteLine(i.Text);
                Debug.WriteLine(i.Visible);
                Debug.WriteLine(i.Position);
            }
        }
    } 
}