using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Marcaj.Models.LocalDbModels;
using SQLite;
using Marcaj.Models.CustomModels;

namespace Marcaj.Services.LocalDbServices
{
    public class LServiceManager
    {
        readonly SQLiteAsyncConnection iDatabase;

        public LServiceManager(string dbpath)
        {
            //Connection

            iDatabase = new SQLiteAsyncConnection(dbpath);

            //Create Tables
            iDatabase.CreateTableAsync<LDineInTablesModel>().Wait();
            iDatabase.CreateTableAsync<LDineInTableGroupsModel>().Wait();
            iDatabase.CreateTableAsync<LEmployeeFileModel>().Wait();
            iDatabase.CreateTableAsync<LOrderHeaderModel>().Wait();
            iDatabase.CreateTableAsync<LMenuItemsModel>().Wait();
            iDatabase.CreateTableAsync<LMenuGroupsModel>().Wait();
            iDatabase.CreateTableAsync<LOrderTransactionsModel>().Wait();
            iDatabase.CreateTableAsync<LStationSettingsModel>().Wait();

        }
        #region EmployeeFiles
        //Get EmployeeFiles by Security Code
        public Task<LEmployeeFileModel> lGetEmployeeFiles(string securityCode)
        {
            return iDatabase.Table<LEmployeeFileModel>().Where(x => x.AccessCode == securityCode).FirstOrDefaultAsync();
        }

        //Get All EmployeeFiles
        public Task<List<LEmployeeFileModel>> lGetAllEmployeeFiles()
        {
            return iDatabase.Table<LEmployeeFileModel>().ToListAsync();
        }

        //Post EmployeeFile
        public Task<int> lPostEmployeeFiles(LEmployeeFileModel model)
        {
            return iDatabase.InsertAsync(model);
        }

        //Get Last Id of EmployeeFile
        public Task<LEmployeeFileModel> lGetLastIdEmployeeFiles()
        {
            return iDatabase.Table<LEmployeeFileModel>().OrderByDescending(x => x.EmployeeID).FirstOrDefaultAsync();
        }

        //Delete All EmployeeFiles
        public Task<int> lDeleteEmployeeFiles()
        {
            return iDatabase.DeleteAllAsync<LEmployeeFileModel>();
        }
        #endregion

        #region DineInTables

        //Get DineInTables By GroupID
        public Task<List<LDineInTablesModel>> lGetDineInTablesByGroupID(int GroupID)
        {
            return iDatabase.Table<LDineInTablesModel>().Where(x => x.TableGroupID == GroupID).ToListAsync();
        }

        //Get All DineInTables
        public Task<List<LDineInTablesModel>> lGetDineInTables()
        {
            return iDatabase.Table<LDineInTablesModel>().ToListAsync();
        }

        //Get Last DineInTableID
        public Task<LDineInTablesModel> lGetLastIdDineInTables()
        {
            return iDatabase.Table<LDineInTablesModel>().OrderByDescending(x => x.DineInTableID).FirstOrDefaultAsync();
        }

        //Put DineInTable
        public Task<int> lPutDineInTable(LDineInTablesModel model)
        {
            return iDatabase.UpdateAsync(model);
        }

        //Post DineInTables
        public Task<int> lPostDineInTables(LDineInTablesModel model)
        {
            return iDatabase.InsertAsync(model);
        }

        //Delete All DineInTables
        public Task<int> lDeleteDineInTables()
        {
            return iDatabase.DeleteAllAsync<LDineInTablesModel>();
        }

        //Get Custom Model DineInTablesAndEmpName
        public List<LDineInTableAndEmpModel> lGetDineInTablesEmpNameByGroupID(int GroupID)
        {
            var returnList = new List<LDineInTableAndEmpModel>();

            var lst = iDatabase.Table<LDineInTablesModel>().Where(x => x.TableGroupID == GroupID).ToListAsync().Result;

            foreach(var dine in lst)
            {
                var model = new LDineInTableAndEmpModel();
                model.DineIn = dine;
                returnList.Add(model);
            }

            foreach(var item in returnList)
            {
                int id = -1;

                var a = iDatabase.Table<LOrderHeaderModel>().Where(x => x.DineInTableID == item.DineIn.DineInTableID).OrderByDescending(x=>x.OrderID).FirstOrDefaultAsync().Result;
                
                if(a!=null)
                {
                    id = a.EmployeeID;
                }
                try
                {
                    var b = iDatabase.Table<LEmployeeFileModel>().Where(x => x.EmployeeID == id).ToListAsync().Result;
                    if(b.Count != 0)
                    {
                        item.EmpName = b[0].FirstName;
                    }
                }
                catch
                {

                }
            }
            return returnList;
        }
        #endregion

        #region DineInTableGroups

        //Get DineInTableGroups
        public Task<List<LDineInTableGroupsModel>> lGetDineInTableGroups()
        {
            return iDatabase.Table<LDineInTableGroupsModel>().ToListAsync();
        }

        //Post DineInTableGroups
        public Task<int> lPostDineInTableGroups(LDineInTableGroupsModel model)
        {
            return iDatabase.InsertAsync(model);
        }

        //Delete All DineInTableGroups
        public Task<int> lDeleteDineInTableGroups()
        {
            return iDatabase.DeleteAllAsync<LDineInTableGroupsModel>();
        }
        #endregion

        #region OrderHeaders

        //Get All OrderHeaders
        public Task<List<LOrderHeaderModel>> lGetOrderHeaders()
        {
            return iDatabase.Table<LOrderHeaderModel>().ToListAsync();
        }

        //Get Last OrderID
        public Task<LOrderHeaderModel> lGetLastIdOrderHeaders()
        {
            return iDatabase.Table<LOrderHeaderModel>().OrderByDescending(x => x.DineInTableID).FirstOrDefaultAsync();
        }

        //Delete All DineInTables
        public Task<int> lDeleteOrderHeaders()
        {
            return iDatabase.DeleteAllAsync<LOrderHeaderModel>();
        }
        //Get OH by Table ID
        public Task<List<LOrderHeaderModel>> lGetOrderHeadersByTableID(int tableID)
        {
            return iDatabase.Table<LOrderHeaderModel>().Where(x => x.DineInTableID == tableID).ToListAsync();
        }
        //Get OH by ID
        public Task<List<LOrderHeaderModel>> lGetOrderHeadersByID(int orderID)
        {
            return iDatabase.Table<LOrderHeaderModel>().Where(x => x.OrderID == orderID).ToListAsync();
        }

        //Put OH
        public Task<int> lPutOrderHeader(LOrderHeaderModel model)
        {
            return iDatabase.UpdateAsync(model);
        }

        //Post OH
        public Task<int> lPostOrderHeader(LOrderHeaderModel model)
        {
            return iDatabase.InsertAsync(model);
        }
        #endregion

        #region MenuItems
        //Get Last MenuItemID
        public Task<LMenuItemsModel> lGetLastIdMenuItems()
        {
            return iDatabase.Table<LMenuItemsModel>().OrderByDescending(x => x.MenuItemID).FirstOrDefaultAsync();
        }
        //Get All MenuItems
        public Task<List<LMenuItemsModel>> lGetMenuItems()
        {
            return iDatabase.Table<LMenuItemsModel>().ToListAsync();
        }
        //Delete All MenuItems
        public Task<int> lDeleteMenuItems()
        {
            return iDatabase.DeleteAllAsync<LMenuItemsModel>();
        }
        //Post MenuItem
        public Task<int> lPostMenuItems(LMenuItemsModel model)
        {
            return iDatabase.InsertAsync(model);
        }
        //Get MenuItems by GroupID
        public Task<List<LMenuItemsModel>> lGetMenuItemsByGroupID(int MenuGroupID)
        {
            return iDatabase.Table<LMenuItemsModel>().Where(x => x.MenuGroupID == MenuGroupID).ToListAsync();
        }
        #endregion

        #region MenuGroups
        //Get Last MenuGroupID
        public Task<LMenuGroupsModel> lGetLastIdMenuGroups()
        {
            return iDatabase.Table<LMenuGroupsModel>().OrderByDescending(x => x.MenuGroupID).FirstOrDefaultAsync();
        }
        //Get All MenuGroups
        public Task<List<LMenuGroupsModel>> lGetMenuGroups()
        {
            return iDatabase.Table<LMenuGroupsModel>().ToListAsync();
        }
        //Delete All MenuGroups
        public Task<int> lDeleteMenuGroups()
        {
            return iDatabase.DeleteAllAsync<LMenuGroupsModel>();
        }
        //Post MenuGroup
        public Task<int> lPostMenuGroups(LMenuGroupsModel model)
        {
            return iDatabase.InsertAsync(model);
        }
        #endregion

        #region OrderTransactions
        //Get All OrderTransactions
        public Task<List<LOrderTransactionsModel>> lGetOrderTransactions()
        {
            return iDatabase.Table<LOrderTransactionsModel>().ToListAsync();
        }

        //Post OrderTransactions for Not Active Table
        public Task<int> lPostOrderTransactionNotActiveTable(List<LOrderTransactionsModel> model, int DineInId)
        {
            var a = iDatabase.Table<LOrderHeaderModel>().Where(x => x.DineInTableID == DineInId).OrderByDescending(x => x.OrderID).FirstOrDefaultAsync();
            try
            {

                foreach (var m in model)
                {
                    if (a != null)
                    {
                        m.OrderID = a.Result.OrderID + 1;
                    }
                    else
                    {
                        m.OrderID = iDatabase.Table<LOrderHeaderModel>().OrderByDescending(x => x.OrderID).FirstOrDefaultAsync().Result.OrderID + 1;
                    }
                    iDatabase.InsertAsync(m);
                }


                return Task.FromResult(1);
            }
            catch
            {
                return Task.FromResult(0);
            }
        }
        //Get Last OrderID
        public Task<LOrderTransactionsModel> lGetLastIdOrderTransactions()
        {
            return iDatabase.Table<LOrderTransactionsModel>().OrderByDescending(x => x.OrderTransactionID).FirstOrDefaultAsync();
        }
        //Get OT by ID
        public Task<List<LOrderTransactionsModel>> lGetOrderTransactionsByOrderID(int orderID)
        {
            return iDatabase.Table<LOrderTransactionsModel>().Where(x => x.OrderID == orderID).ToListAsync();
        }
        //Delete All DineInTables
        public Task<int> lDeleteOrderTransactions()
        {
            return iDatabase.DeleteAllAsync<LOrderTransactionsModel>();
        }
        //Put OH
        public Task<int> lPutOrderTransactions(LOrderTransactionsModel model)
        {
            return iDatabase.UpdateAsync(model);
        }

        //Post OH
        public Task<int> lPostOrderTransactions(LOrderTransactionsModel model)
        {
            return iDatabase.InsertAsync(model);
        }
        #endregion

        #region StationSettings
        //Get Station
        public Task<LStationSettingsModel> lGetStationSettings(string name)
        {
            return iDatabase.Table<LStationSettingsModel>().Where(x => x.ComputerName == name).FirstOrDefaultAsync();
        }
        //Post Station
        public Task<int> lPostStationSettings(LStationSettingsModel model)
        {
            return iDatabase.InsertAsync(model);
        }
        #endregion

    }
}
