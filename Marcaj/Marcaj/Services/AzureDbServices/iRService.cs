using Marcaj.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Marcaj.Models.CustomModels;
using Marcaj.Models.DbModels.LGK;

namespace Marcaj.Services
{
    public interface iRService
    {
        Task<EmployeeFileModel> GetEmployeeFiles(string securityCode);
        Task<List<DineInTableModel>> GetDineInTables();
        Task PutDineInTable(DineInTableModel item, int id);
        Task PutOrderHeadersDineInTableId(OrderHeadersModel item, int id);
        Task<List<DineInTableGroupModel>> GetDineInTableGroups();
        Task<List<DineInTableAndEmpModel>> GetDineInTablesByTableGroup(int groupID);
        Task<List<MenuGroupsModel>> GetMenuGroups();
        Task<List<MenuItemsModel>> GetMenuItemsByGroupID(int id);
        Task<List<OrderTransactionsModel>> GetOrderTransactionsByOrderID(int id);
        Task PostOrderTransactionNotActive(List<OrderTransactionsModel> user, int tableId);
        Task PutOrderHeaders(OrderHeadersModel item, int id);
        Task PostOrderHeader(OrderHeadersModel user);
        Task PostTableGroup(DineInTableGroupModel model);
        Task PutSynchVerOrderHeaders(OrderHeadersModel item, int id);
        Task PutDineInTablesPosition(List<DineInTableModel> items);
        Task PutPopUpSetting(StationSettingsModel model, bool popUp);
        Task PutStationName(StationSettingsModel item);
        Task<List<LGKMClientsModel>> GetAllClients();
        Task<List<InventoryClients>> GetAllInventoryClients();
        Task<List<StationSettingsModel>> GetAllStationSettings();
        Task<List<DineInTableModel>> GetOnlyDineInTablesByTableGroup(int groupID);
        Task<List<OrderHeadersModel>> GetOrderHeadersByDineInTableID(int id);
        Task<OrderHeadersModel> GetOrderHeaderByID(int id);
        Task DeleteTableGroup(int id);
        Task PutTableGroups(DineInTableGroupModel item, int id);
        Task PostOrderTransactionActive(List<OrderTransactionsModel> user, int tableId);
        Task<List<EmployeeFileModel>> GetAllEmployeeFiles();
        Task<int> GetLastIdEmployeeFiles();
        Task<List<OrderHeadersModel>> GetActiveOrderHeadersBar();
        Task<List<OrderHeadersModel>> GetActiveOrderHeadersRestaurant();
        Task PostEmployeeFiles(List<EmployeeFileModel> user);
        Task PutEmployeeFile(EmployeeFileModel item, int id);
        Task PostDineInTable(DineInTableModel user);
        Task<List<OrderHeadersModel>> GetOrderHeaders();
        Task<List<MenuItemsModel>> GetMenuItems();
        Task<List<OrderTransactionsModel>> GetOrderTransactionsByListOfOrderIDs(int[] ids);
        Task<List<OrderHeadersModel>> GetOrderHeadersSync();
        Task PostOrderTransactionSync(List<OrderTransactionsModel> user);
        Task<StationSettingsModel> GetStationSettings(string name);
        Task PostStationSettings(StationSettingsModel model);
        Task<List<DineInTableAndEmpModel>> GetDineInTablesAllByTableGroup(int groupID);
        Task<List<OrderHeadersModel>> GetActiveOrderHeaders();
        Task<List<OrderHeadersModel>> GetActiveOrderHeadersByEmpId(int empId);
    }
}
