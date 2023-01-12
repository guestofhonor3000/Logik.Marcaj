using Marcaj.Models.DbModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels.LGK;

namespace Marcaj.Services
{
    public class RService : iRService
    {
        #region Init Variables
        HttpClient client;
        JsonSerializerOptions serializerOptions;
        List<DineInTableAndEmpModel> dineInFull;
        List<DineInTableModel> dineInTables;
        List<DineInTableGroupModel> dineInGroups;
        List<MenuGroupsModel> menuGroups;
        List<MenuItemsModel> menuItems;
        List<OrderTransactionsModel> orderTransactions;
        List<OrderHeadersModel> orderHeaders;
        List<EmployeeFileModel> employeeFiles;
        List<StationSettingsModel> stationModels;
        List<LGKMClientsModel> clientsList;
        List<InventoryClients> inventoryClients;
        #endregion

        #region Init
        public RService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 2500000;
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        #endregion

        #region EmployeeFiles
        //Get Emp File by Security Code
        public async Task<EmployeeFileModel> GetEmployeeFiles(string securityCode)
        {
            HttpClient client = new HttpClient();
            var a = new EmployeeFileModel();
            Uri uri = new Uri(string.Format(Constants.GetUriEmployeeFilesByCode + securityCode, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            a = JsonConvert.DeserializeObject<EmployeeFileModel>(content);
            return a;
        }

        //Get All Emp Files
        public async Task<List<EmployeeFileModel>> GetAllEmployeeFiles()
        {
            HttpClient client = new HttpClient();
            employeeFiles = new List<EmployeeFileModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriEmployeeFiles, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            employeeFiles = JsonConvert.DeserializeObject<List<EmployeeFileModel>>(content);
            return employeeFiles;
        }

        //Get Last Id Emp Files
        public async Task<int> GetLastIdEmployeeFiles()
        {
            HttpClient client = new HttpClient();
            int a = 0;
            Uri uri = new Uri(string.Format(Constants.GetUriEmployeeFilesLastId, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            a = JsonConvert.DeserializeObject<int>(content);
            return a;
        }

        //Put Employee Files
        public async Task PutEmployeeFile(EmployeeFileModel item, int id)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriEmployeeFiles + id, string.Empty));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<EmployeeFileModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"EmployeeFile updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Post Employee Files
        public async Task PostEmployeeFiles(List<EmployeeFileModel> user)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriEmployeeFiles, string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.WriteLine(@"EmployeeFiles added");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
        #endregion

        #region DineInTables

        //Get All Tables
        public async Task<List<DineInTableModel>> GetDineInTables()
        {
            HttpClient client = new HttpClient();
            dineInTables = new List<DineInTableModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriDineInTables, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            dineInTables = JsonConvert.DeserializeObject<List<DineInTableModel>>(content);
            return dineInTables;
        }

        //Get Tables & Emp By Table Group
        public async Task<List<DineInTableAndEmpModel>> GetDineInTablesByTableGroup(int groupID)
        {
            HttpClient client = new HttpClient();
            dineInFull = new List<DineInTableAndEmpModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriDineInTablesByTableGroupIDEmp + groupID + "&type=active", string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            dineInFull = JsonConvert.DeserializeObject<List<DineInTableAndEmpModel>>(content);
            return dineInFull;
        }

        //Get Tables by Group ID
        public async Task<List<DineInTableModel>> GetOnlyDineInTablesByTableGroup(int groupID)
        {
            HttpClient client = new HttpClient();
            dineInTables = new List<DineInTableModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriDineInTablesByTableGroupID + groupID , string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            dineInTables = JsonConvert.DeserializeObject<List<DineInTableModel>>(content);
            return dineInTables;
        }

        public async Task<List<DineInTableAndEmpModel>> GetDineInTablesAllByTableGroup(int groupID)
        {
            HttpClient client = new HttpClient();
            dineInFull = new List<DineInTableAndEmpModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriDineInTablesByTableGroupIDEmp + groupID+"&type=all", string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            dineInFull = JsonConvert.DeserializeObject<List<DineInTableAndEmpModel>>(content);
            return dineInFull;
        }
        //Post Table
        public async Task PostDineInTable(DineInTableModel user)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriDineInTable, string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.WriteLine(@"DineInTable added");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Put Table Position
        public async Task PutDineInTablesPosition(List<DineInTableModel> items)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriDineInTablesPosition, string.Empty));
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize<List<DineInTableModel>>(items, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"DineInTable Position updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Put Table Activity
        public async Task PutDineInTable(DineInTableModel item, int id)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriDineInTables + id, string.Empty));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<DineInTableModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"DineInTable updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
        #endregion

        #region DineInTableGroups

        //Get TableGroups
        public async Task<List<DineInTableGroupModel>> GetDineInTableGroups()
        {
            HttpClient client = new HttpClient();
            dineInGroups = new List<DineInTableGroupModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriDineInTablesGroups, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            dineInGroups = JsonConvert.DeserializeObject<List<DineInTableGroupModel>>(content);
            return dineInGroups;
        }

        //Put TableGroups
        public async Task PutTableGroups(DineInTableGroupModel item, int id)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriDineInTableGroups + id, string.Empty));
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize<DineInTableGroupModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"Table Groups updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Post TableGroups
        public async Task PostTableGroup(DineInTableGroupModel model)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriDineInTableGroups , string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.WriteLine(@"TableGroup added");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Delete TableGroups
        public async Task DeleteTableGroup(int id)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.DeleteUriDineInTableGroup + id, string.Empty)); ;
            try
            {
                HttpResponseMessage response = null;
                response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\Menu Group deleted.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
        #endregion

        #region MenuItems
        public async Task<List<MenuItemsModel>> GetMenuItemsByGroupID(int id)
        {
            HttpClient client = new HttpClient();
            menuItems = new List<MenuItemsModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriMenuItemsByMenuGroupID + id, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            menuItems = JsonConvert.DeserializeObject<List<MenuItemsModel>>(content);
            return menuItems;
        }
        public async Task<List<MenuItemsModel>> GetMenuItems()
        {
            HttpClient client = new HttpClient();
            menuItems = new List<MenuItemsModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriMenuItems , string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            menuItems = JsonConvert.DeserializeObject<List<MenuItemsModel>>(content);
            return menuItems;
        }
        #endregion

        #region MenuItemsGroups

        //Get Menu Groups
        public async Task<List<MenuGroupsModel>> GetMenuGroups()
        {
            HttpClient client = new HttpClient();
            menuGroups = new List<MenuGroupsModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriMenuGroups, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            menuGroups = JsonConvert.DeserializeObject<List<MenuGroupsModel>>(content);
            return menuGroups;
        }

    

        #endregion

        #region OrderTransactions

        //Get OrderTransactions By Order ID
        public async Task<List<OrderTransactionsModel>> GetOrderTransactionsByOrderID(int id)
        {
            HttpClient clien = new HttpClient();
            orderTransactions = new List<OrderTransactionsModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriOrderTransactionsByOrderID + id, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderTransactions = JsonConvert.DeserializeObject<List<OrderTransactionsModel>>(content);
            return orderTransactions;
        }

        //Get OrderTransactions By List of Order IDs
        public async Task<List<OrderTransactionsModel>> GetOrderTransactionsByListOfOrderIDs(int[] ids)
        {
            HttpClient clien = new HttpClient();
            orderTransactions = new List<OrderTransactionsModel>();
            string url = Constants.GetUriOrderTransactionsByListOfOrderIDs;
            int index = 0;
            foreach(var id in ids)
            {
                index++;
                if(index==1)
                {
                    url = url + "getOrderIDList=" + id.ToString();
                }
                else
                {
                    url = url + "&getOrderIDList=" + id.ToString();
                }
            }
            Uri uri = new Uri(string.Format(url, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderTransactions = JsonConvert.DeserializeObject<List<OrderTransactionsModel>>(content);
            return orderTransactions;
        }

        //Post OrderTransactionsNotActive
        public async Task PostOrderTransactionNotActive(List<OrderTransactionsModel> user, int tableId)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriOrderTransactionsNotActive + tableId, string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.WriteLine(@"OrderTransaction added");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
        //Post OrderTransactionsActive
        public async Task PostOrderTransactionActive(List<OrderTransactionsModel> user, int tableId)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriOrderTransactionsActive + tableId, string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.WriteLine(@"OrderTransaction added");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Post OrderTransactionsSync
        public async Task PostOrderTransactionSync(List<OrderTransactionsModel> user)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriOrderTransactionsSync, string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.WriteLine(@"OrderTransaction added");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
        #endregion

        #region OrderHeaders

        //Put Order DineInTableId
        public async Task PutOrderHeadersDineInTableId(OrderHeadersModel item, int id)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriOrderHeadersDineInTableMove + id, string.Empty));
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize<OrderHeadersModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"OrderHeader updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Put OrderHeaders
        public async Task PutOrderHeaders(OrderHeadersModel item, int id)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriOrderHeaders + id, string.Empty));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<OrderHeadersModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"OrderHeader updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
        //Put SynchVer OH
        public async Task PutSynchVerOrderHeaders(OrderHeadersModel item, int id)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriOrderHeaders + id+"&type=synchver", string.Empty));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<OrderHeadersModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"OrderHeader updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Post OrderHeader
        public async Task PostOrderHeader(OrderHeadersModel user)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriOrderHeaders, string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"OrderHeader added");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}",  ex.InnerException);
            }
        }
        //Get OrderHeaders Sync(By Date)
        public async Task<List<OrderHeadersModel>> GetOrderHeadersSync()
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriOrderHeadersSync, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            return orderHeaders;
        }
        //Get Active OrderHeaders

        public async Task<List<OrderHeadersModel>> GetActiveOrderHeaders()
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriActiveOrderHeaders, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            return orderHeaders;

        }
        //Get Active OrderHeaders Bar

        public async Task<List<OrderHeadersModel>> GetActiveOrderHeadersBar()
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriActiveOrderHeadersBar, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            return orderHeaders;

        }
        //Get Active OrderHeaders Restaurant

        public async Task<List<OrderHeadersModel>> GetActiveOrderHeadersRestaurant()
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriActiveOrderHeadersBar, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            return orderHeaders;

        }
        //Get Active OrderHeaders by Emp Id
        public async Task<List<OrderHeadersModel>> GetActiveOrderHeadersByEmpId(int empId)
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriActiveOrderHeadersByEmp+empId, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            return orderHeaders;

        }
        //Get OrderHeaders
        public async Task<List<OrderHeadersModel>> GetOrderHeaders()
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriOrderHeaders, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            return orderHeaders;
        }
        //Get OrderHeaders By DineInTableID
        public async Task<List<OrderHeadersModel>> GetOrderHeadersByDineInTableID(int id)
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriOrderHeadersByTableID + id, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            return orderHeaders;
        }

        //Get OrderHeaders By OrderID
        public async Task<OrderHeadersModel> GetOrderHeaderByID(int id)
        {
            HttpClient clien = new HttpClient();
            orderHeaders = new List<OrderHeadersModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriOrderHeadersByTableID + id, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            orderHeaders = JsonConvert.DeserializeObject<List<OrderHeadersModel>>(content);
            if (orderHeaders.Count > 0)
            {
                return orderHeaders[0];
            }
            else
            {
                return null;
            }
            //L@g1kS3rv3r
        }
        #endregion

        #region StationSettings
        //Post StationSettings
        public async Task PostStationSettings(StationSettingsModel model)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(Constants.PostUriStationSettings, string.Empty));
            try
            {
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"StationSettings added");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Put DeviceName
        public async Task PutStationName(StationSettingsModel item)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriStationSettings, string.Empty));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<StationSettingsModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"Station Name updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        //Put PopUpSettings
        public async Task PutPopUpSetting(StationSettingsModel model, bool popUp)
        {
            Uri uri = new Uri(string.Format(Constants.PutUriStationSettingsPop + popUp, string.Empty));
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize<StationSettingsModel>(model, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"Station Settings updated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
     
        //Get StationSettings
        public async Task<List<StationSettingsModel>> GetAllStationSettings()
        {
            HttpClient client = new HttpClient();
            stationModels= new List<StationSettingsModel>();
            
            Uri uri = new Uri(string.Format(Constants.GetUriAllStationSettings, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            stationModels = JsonConvert.DeserializeObject<List<StationSettingsModel>>(content);
            return stationModels;
        }
        //Get StationSettings
        public async Task<StationSettingsModel> GetStationSettings(string name)
        {
            HttpClient client = new HttpClient();
            var a = new StationSettingsModel();
            Uri uri = new Uri(string.Format(Constants.GetUriStationSettings + name, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            a = JsonConvert.DeserializeObject<StationSettingsModel>(content);
            return a;
        }
        #endregion

        #region LGKMClients
        
        //Get Clients
        public async Task<List<LGKMClientsModel>> GetAllClients()
        {
            HttpClient client = new HttpClient();
            clientsList = new List<LGKMClientsModel>();
            Uri uri = new Uri(string.Format(Constants.GetUriAllStationSettings, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            clientsList = JsonConvert.DeserializeObject<List<LGKMClientsModel>>(content);
            return clientsList;
        }
        #endregion

        #region InventoryClients
        public async Task<List<InventoryClients>>  GetAllInventoryClients()
        {
            HttpClient client = new HttpClient();
            inventoryClients = new List<InventoryClients>();
            Uri uri = new Uri(string.Format(Constants.GetUriAllInventoryClients, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            inventoryClients = JsonConvert.DeserializeObject<List<InventoryClients>>(content);
            return inventoryClients;
        }
        #endregion
    }
}
