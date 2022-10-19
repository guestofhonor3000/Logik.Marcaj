using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();


            /*private static void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
           {
               var previous = e.PreviousSelection;
               var current = e.CurrentSelection;
           }*/
        }
    } 
}