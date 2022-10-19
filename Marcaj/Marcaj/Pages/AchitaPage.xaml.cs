﻿using Marcaj.Models.DbModels;
using Marcaj.Models.LocalDbModels;
using Marcaj.Pages.Tables;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchitaPage : ContentPage
    {
        EmployeeFileModel EmplFl;
        public AchitaPage(EmployeeFileModel emplFl)
        {
            InitializeComponent();
            EmplFl = emplFl;
            PopList("Default");
        }

        async void PopList(string type)
        {
            if(type == "Default")
            {
                var listActive = await App.manager.iGetActiveOrderHeaders();
                tabList.ItemsSource = listActive;
            }
            if(type == "Bar")
            {
                var listBar = await App.manager.iGetActiveOrderHeadersBar();
                tabList.ItemsSource = listBar;
            }
            if(type == "Emp")
            {
                var listEmp = await App.manager.iGetActiveOrderHeadersByEmpId(EmplFl.EmployeeID);
                tabList.ItemsSource = listEmp;
            }
            if(type == "Restaurant")
            {
                var listRest = await App.manager.iGetActiveOrderHeadersRestaurant();
                tabList.ItemsSource = listRest;
            }
        }

        private async void btnMese_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllTables(EmplFl));
        }

        private async void btnAchita_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchitaPage(EmplFl));
        }

        private void btnToate_Clicked(object sender, EventArgs e)
        {
            PopList("Default");
        }

        private void btnDineIn_Clicked(object sender, EventArgs e)
        {
            PopList("Restaurant");
        }

        private void btnBar_Clicked(object sender, EventArgs e)
        {
            PopList("Bar");
        }

        private void btnEmp_Clicked(object sender, EventArgs e)
        {
            PopList("Emp");
        }
        /*private void btnBar_Clicked(object sender, EventArgs e)
        {
            PopList("Bar");
        }*/

    }


}