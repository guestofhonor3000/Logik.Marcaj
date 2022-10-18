using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels;
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

        public Task<List<DineInTableAndEmpModel>> iGetDineInTablesAllByTableGroup(int groupID)
        {
            return Service.GetDineInTablesAllByTableGroup(groupID);
        }
        public Task<List<MenuGroupsModel>> iGetMenuGroups()
        {
            return Service.GetMenuGroups();
        }

        public Task<List<MenuItemsModel>> iGetMenuItemsByGroupID(int id)
        {
            return Service.GetMenuItemsByGroupID(id);
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
