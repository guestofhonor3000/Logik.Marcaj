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
            new TableLayoutModel { Position="1", Text="" },
            new TableLayoutModel { Position="2" , Text=""},
            new TableLayoutModel { Position="3" , Text=""},
            new TableLayoutModel { Position="4" , Text=""},
            new TableLayoutModel { Position="5" , Text=""},
            new TableLayoutModel { Position="6" , Text = ""},
            new TableLayoutModel { Position="7" , Text = ""},
            new TableLayoutModel { Position="8" , Text = ""},
            new TableLayoutModel { Position="9" , Text = ""},
            new TableLayoutModel { Position="10" , Text = ""},
            new TableLayoutModel { Position="11" , Text = ""},
            new TableLayoutModel { Position="12" , Text = ""},
            new TableLayoutModel { Position="13" , Text = ""},
            new TableLayoutModel { Position="14" , Text = ""},
            new TableLayoutModel { Position="15" , Text = ""},
            new TableLayoutModel { Position="16" , Text = ""},
            new TableLayoutModel { Position="17" , Text=""},
            new TableLayoutModel { Position="18" , Text=""},
            new TableLayoutModel { Position="19" , Text="" },
            new TableLayoutModel { Position="20", Text="" }

            
        };
            /*private static void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
           {
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
           
            tblVect.Text = "";
            foreach(var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "2";
                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;  
                tblVect.Text +=" "+b.Position ;
            }

            tblLayoutColl.SelectedItems = null;
        }

        private void btnTbl4_Clicked(object sender, EventArgs e)
        {
            tblVect.Text = "";
            foreach (var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "4";
                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;
                tblVect.Text += " " + b.Position;
            }
            tblLayoutColl.SelectedItems = null;

        }

        private void btnTbl6_Clicked(object sender, EventArgs e)
        {
            tblVect.Text = "";
            foreach (var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "6";
                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;
                tblVect.Text += " " + b.Position;
            }
            tblLayoutColl.SelectedItems = null;

        }
        private void btnTbl8_Clicked(object sender, EventArgs e)
        {
            tblVect.Text = "";
            foreach (var a in tblLayoutColl.SelectedItems)
            {
                var b = a as TableLayoutModel;
                var c = tblLayout.Where(x => x.Position == b.Position).FirstOrDefault();
                c.Text = "8";
                tblLayoutColl.ItemsSource = null;
                tblLayoutColl.ItemsSource = tblLayout;
                tblVect.Text += " " + b.Position;
            }
            tblLayoutColl.SelectedItems = null;


        }

        private void btnTblGrid_Clicked(object sender, EventArgs e)
        {

        }

        private void tblVect_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    } 
}