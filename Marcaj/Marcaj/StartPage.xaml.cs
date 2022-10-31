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
            try
            {
                string device = DeviceInfo.Name;
                string deviceName = " ";
                if (device.Contains("_"))
                {
                    deviceName = device.Replace('_', ' ');
                }
                else
                {
                    deviceName = device;
                }
                if (device.Length > 20)
                {
                    deviceName = device.Remove(19);
                }

                Debug.WriteLine(deviceName);
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var a = await App.manager.iGetStationSettings(deviceName);


                    if (a == null)
                    {
                        var prompt = await DisplayPromptAsync("Code", "Enter the code for the desired db.", "Ok", "Cancel", "Code...",4,Keyboard.Numeric);
                        if (prompt == "1234")
                        {
                            var stations = await App.manager.iGetAllStationSettings();
                            var stationNames = new List<string>();
                            foreach (var st in stations)
                            {
                                stationNames.Add(st.ComputerName);
                            }
                            var action = await DisplayActionSheet("Choose", "Cancel", null, stationNames.ToArray());
                            if (action != null)
                            {
                                var alert = await DisplayAlert("Warning", "About to edit the chosen station", "Ok", "Cancel");
                                if(alert == true)
                                {
                                    var modell = stations.Where(x => x.ComputerName == action).FirstOrDefault();

                                    modell.ComputerName = deviceName;

                                    await App.manager.iPutStationName(modell);

                                }
                            }
                        }
                        else
                        {
                            Post(deviceName);
                        }
                        
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
                    if (b == null)
                    {
                        await DisplayAlert("Error", "No Station Saved", "OK");
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
            }
            
                
        }
        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            try
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
                            if (empLocalModel.Result != null)
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException);
            }
          
        }

      
        async void Post(string deviceName)
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
        private void btn7_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "7";
        }

        private void btn8_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "8";
        }

        private void btn9_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "9";
        }
        private void btn0_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "0";
        }

        private void btn4_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "4";
        }

        private void btn5_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "5";
        }

        private void btn6_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "6";
        }

        private void btn1_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "1";
        }

        private void btn2_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "2";
        }

        private void btn3_Clicked(object sender, EventArgs e)
        {
            codEntry.Text += "3";
        }

        private void btnDel_Clicked(object sender, EventArgs e)
        {
            //string s = codEntry.Text.Substring(0, codEntry.Text.Length - 1);
            codEntry.Text = "";
        }
    }
}