using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.LocalDbModels;
using Marcaj.Models.DbModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace Marcaj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            PopStationSettings();
        }


        async void PopStationSettings()
        {
            string device = DeviceInfo.Name;
            string deviceName = " ";
            if(device.Contains("_"))
            {
                deviceName = device.Replace('_', ' ');
            }
            else
            {
                deviceName = device;
            }
            if(device.Length>20)
            {
                deviceName =device.Remove(19);
            }
         
            Debug.WriteLine(deviceName);
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var a = await App.manager.iGetStationSettings(deviceName);
               
                
                if (a == null)
                {
                    var model = new StationSettingsModel();
                    
                    model.ComputerName = deviceName;
                    model.ReceiptPrinterPort = "No Printer Attached";
                    model.ReceiptPrinterType = 1;
                    model.CashDrawerAttached = false;
                    model.KitchenPrinter1Title = "Bucatarie";
                    model.KitchenPrinter1Port = "No Printer Attached";
                    model.KitchenPrinter1Type = 1;
                    model.KitchenPrinter1Name = "No Printer Attached";
                    model.DedicatedToCashier = false;
                    model.UserInterfaceLocale = "3";
                    model.ReceiptPrinterName = "No Printer Attached";
                    model.FastBar = false;
                    model.BarTab = false;
                    model.OrderRecallToBrowse = false;
                    model.DefaultToMenuGroupsInOrderEntry = false;
                    model.HasEDC = false;
                    model.DriveThruStation = false;
                    model.KeepInOrderEntryAfterSent = false;
                    model.RowGUID = Guid.NewGuid().ToString();

                    
                   
                    await App.manager.iPostStationSettings(model);

                    var lmodel = new LStationSettingsModel();

                    lmodel.ComputerName = deviceName;
                    lmodel.ReceiptPrinterPort = "No Printer Attached";
                    lmodel.ReceiptPrinterType = 1;
                    lmodel.CashDrawerAttached = false;
                    lmodel.KitchenPrinter1Title = "Bucatarie";
                    lmodel.KitchenPrinter1Port = "No Printer Attached";
                    lmodel.KitchenPrinter1Type = 1;
                    lmodel.KitchenPrinter1Name = "No Printer Attached";
                    lmodel.DedicatedToCashier = false;
                    lmodel.UserInterfaceLocale = "3";
                    lmodel.ReceiptPrinterName = "No Printer Attached";
                    lmodel.FastBar = false;
                    lmodel.BarTab = false;
                    lmodel.OrderRecallToBrowse = false;
                    lmodel.DefaultToMenuGroupsInOrderEntry = false;
                    lmodel.HasEDC = false;
                    lmodel.DriveThruStation = false;
                    lmodel.KeepInOrderEntryAfterSent = false;
                    lmodel.RowGUID = Guid.NewGuid().ToString();

                    await App.lDatabase.lPostStationSettings(lmodel);
                }

                var la = await App.lDatabase.lGetStationSettings(deviceName);
                if (la == null)
                {
                    var lmodel = new LStationSettingsModel();

                    lmodel.ComputerName = deviceName;
                    lmodel.ReceiptPrinterPort = "No Printer Attached";
                    lmodel.ReceiptPrinterType = 1;
                    lmodel.CashDrawerAttached = false;
                    lmodel.KitchenPrinter1Title = "Bucatarie";
                    lmodel.KitchenPrinter1Port = "No Printer Attached";
                    lmodel.KitchenPrinter1Type = 1;
                    lmodel.KitchenPrinter1Name = "No Printer Attached";
                    lmodel.DedicatedToCashier = false;
                    lmodel.UserInterfaceLocale = "3";
                    lmodel.ReceiptPrinterName = "No Printer Attached";
                    lmodel.FastBar = false;
                    lmodel.BarTab = false;
                    lmodel.OrderRecallToBrowse = false;
                    lmodel.DefaultToMenuGroupsInOrderEntry = false;
                    lmodel.HasEDC = false;
                    lmodel.DriveThruStation = false;
                    lmodel.KeepInOrderEntryAfterSent = false;
                    lmodel.RowGUID = Guid.NewGuid().ToString();

                    await App.lDatabase.lPostStationSettings(lmodel);
                    
                }

            }
            else
            {
                var b = await App.lDatabase.lGetStationSettings(deviceName);
                if(b == null)
                {
                    await DisplayAlert("Error", "No Station Saved", "OK");
                }
            }
                
        }
        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (codEntry.Text != "")
                {
                    var emplFl = await App.manager.iGetEmployeeFiles(codEntry.Text);
                    if (emplFl != null)
                    {
                        var empFiles = await App.manager.iGetAllEmployeeFiles();
                      
                        var empLocalModel = App.lDatabase.lGetLastIdEmployeeFiles();
                        int empIdLocal = 0;
                        if(empLocalModel.Result != null)
                        {
                            empIdLocal = empLocalModel.Result.EmployeeID;
                        }
                        var empIdAzure = empFiles.OrderByDescending(x => x.EmployeeID).FirstOrDefault().EmployeeID;
                        
                        if (empIdAzure > empIdLocal)
                        {
                            await App.lDatabase.lDeleteEmployeeFiles();
                            foreach (var emp in empFiles)
                            {
                                var empLocal = new LEmployeeFileModel();
                                empLocal.EmployeeID = emp.EmployeeID;
                                empLocal.AccessCode = emp.AccessCode;
                                empLocal.FirstName = emp.FirstName;
                                empLocal.LastName = emp.LastName;
                                await App.lDatabase.lPostEmployeeFiles(empLocal);
                            }
                        }
                        await Navigation.PushAsync(new HomePage(emplFl));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Codul este gresit!", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Codul este gresit!", "OK");
                }
            }
            else
            {
                if (codEntry.Text != "")
                {
                    var emplFl = await App.lDatabase.lGetEmployeeFiles(codEntry.Text);
                    if (emplFl != null)
                    {
                        var emplFlAzure = new EmployeeFileModel();
                        emplFlAzure.EmployeeID = emplFl.EmployeeID;
                        emplFlAzure.FirstName = emplFl.FirstName;
                        emplFlAzure.AccessCode = emplFl.AccessCode;
                        emplFlAzure.LastName = emplFl.LastName;
                        MessagingCenter.Send<StartPage>(this, "NoCon");
                        await Navigation.PushAsync(new HomePage(emplFlAzure));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Codul este gresit!", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Codul este gresit!", "OK");
                }
            }
        }
    }
}