using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
using Marcaj.Models.DbModels.LGK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Marcaj.Services
{
    public class ServiceManager
    {
        iRService Service;

        public ServiceManager(iRService service)
        {
            Service = service;
        }

        public Task iPostDineInTable(DineInTableModel user)
        {
            return Service.PostDineInTable(user);
        }
        public Task iPostEmployeeFiles(List<EmployeeFileModel> user)
        {
            return Service.PostEmployeeFiles(user);
        }
        public Task iPutEmployeeFile(EmployeeFileModel item, int id)
        {
            return Service.PutEmployeeFile(item, id);
        }
        public Task iPostTableGroup(DineInTableGroupModel model)
        {
            return Service.PostTableGroup(model);
        }
        public Task iDeleteTableGroup(int id)
        {
            return Service.DeleteTableGroup(id);
        }
        public Task iPutSynchVerOrderHeaders(OrderHeadersModel item, int id)
        {
            return Service.PutSynchVerOrderHeaders(item, id);
        }
        public Task<List<LGKMClientsModel>> iGetAllClients()
        {
            return Service.GetAllClients();
        }
        public Task<List<InventoryClients>> iGetAllInventoryClients()
        {
            return Service.GetAllInventoryClients();
        }
        public Task iPutTableGroups(DineInTableGroupModel item, int id)
        {
            return Service.PutTableGroups(item, id);
        }
        public Task<List<EmployeeFileModel>> iGetAllEmployeeFiles()
        {
            return Service.GetAllEmployeeFiles();
        }
        public Task<int> iGetLastIdEmployeeFiles()
        {
            return Service.GetLastIdEmployeeFiles();
        }
        public Task<EmployeeFileModel> iGetEmployeeFiles(string securityCode)
        {
            return Service.GetEmployeeFiles(securityCode);
        }

        public Task<List<DineInTableModel>> iGetDineInTables()
        {
            return Service.GetDineInTables();
        }

        public Task iPutDineInTable(DineInTableModel item, int id)
        {
            return Service.PutDineInTable(item, id);
        }

        public Task<List<DineInTableGroupModel>> iGetDineInTableGroups()
        {
            return Service.GetDineInTableGroups();
        }

        public Task<List<DineInTableAndEmpModel>> iGetDineInTablesByTableGroup(int groupID)
        {
            return Service.GetDineInTablesByTableGroup(groupID);
        }

        public Task<List<OrderHeadersModel>> iGetActiveOrderHeadersByEmpId(int empId)
        {
            return Service.GetActiveOrderHeadersByEmpId(empId);
        }
        public Task<List<OrderHeadersModel>> iGetActiveOrderHeadersBar()
        {
            return Service.GetActiveOrderHeadersBar();
        }
        public Task<List<OrderHeadersModel>> iGetActiveOrderHeadersRestaurant()
        {
            return Service.GetActiveOrderHeadersRestaurant();
        }
        public Task<List<OrderHeadersModel>> iGetActiveOrderHeaders()
        {
            return Service.GetActiveOrderHeaders();
        }
        public Task iPutPopUpSetting(StationSettingsModel model, bool popUp)
        {
            return Service.PutPopUpSetting(model, popUp);
        }
        public Task<List<StationSettingsModel>> iGetAllStationSettings()
        {
            return Service.GetAllStationSettings();
        }

        public Task iPutStationName(StationSettingsModel item)
        {
            return Service.PutStationName(item);
        }
        public Task<List<DineInTableAndEmpModel>> iGetDineInTablesAllByTableGroup(int groupID)
        {
            return Service.GetDineInTablesAllByTableGroup(groupID);
        }
        public Task iPutDineInTablesPosition(List<DineInTableModel> items)
        {
            return Service.PutDineInTablesPosition(items);
        }

        public Task<List<DineInTableModel>> iGetOnlyDineInTablesByTableGroup(int groupID)
        {
            return Service.GetOnlyDineInTablesByTableGroup(groupID);
        }
        public Task<List<MenuGroupsModel>> iGetMenuGroups()
        {
            return Service.GetMenuGroups();
        }

        public Task<List<MenuItemsModel>> iGetMenuItemsByGroupID(int id)
        {
            return Service.GetMenuItemsByGroupID(id);
        }
        public Task iPutOrderHeadersDineInTableId(OrderHeadersModel item, int id)
        {
            return Service.PutOrderHeadersDineInTableId(item, id);
        }
        public Task iPostOrderTransactionNotActive(List<OrderTransactionsModel> user, int tableId)
        {
            return Service.PostOrderTransactionNotActive(user, tableId);
        }

        public Task iPostOrderTransactionActive(List<OrderTransactionsModel> user, int tableId)
        {
            return Service.PostOrderTransactionActive(user, tableId);
        }
        public Task<List<OrderTransactionsModel>> iGetOrderTransactionsByOrderID(int id)
        {
            return Service.GetOrderTransactionsByOrderID(id);
        }
        public Task iPutOrderHeaders(OrderHeadersModel item, int id)
        {
            return Service.PutOrderHeaders(item, id);
        }

        public Task iPostOrderHeader(OrderHeadersModel user)
        {
            return Service.PostOrderHeader(user);
        }

        public Task<List<OrderHeadersModel>> iGetOrderHeadersByDineInTableID(int id)
        {
            return Service.GetOrderHeadersByDineInTableID(id);
        }

        public Task<OrderHeadersModel> iGetOrderHeaderByID(int id)
        {
            return Service.GetOrderHeaderByID(id);
        }

        public Task<List<OrderHeadersModel>> iGetOrderHeaders()
        {
            return Service.GetOrderHeaders();
        }
        public Task<List<MenuItemsModel>> iGetMenuItems()
        {
            return Service.GetMenuItems();
        }

        public Task<List<OrderTransactionsModel>> iGetOrderTransactionsByListOfOrderIDs(int[] ids)
        {
            return Service.GetOrderTransactionsByListOfOrderIDs(ids);
        }
        public Task<List<OrderHeadersModel>> iGetOrderHeadersSync()
        {
            return Service.GetOrderHeadersSync();
        }
        public Task iPostOrderTransactionSync(List<OrderTransactionsModel> user)
        {
            return Service.PostOrderTransactionSync(user);
        }
        public Task iPostStationSettings(StationSettingsModel model)
        {
            return Service.PostStationSettings(model);
        }
        public Task<StationSettingsModel> iGetStationSettings(string name)
        {
            return Service.GetStationSettings(name);
        }
    }
}
