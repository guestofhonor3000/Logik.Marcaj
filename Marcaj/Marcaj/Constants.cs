using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj
{
    public class Constants
    {
        //EmployeeFiles
        public static string GetUriEmployeeFilesByCode = "https://elogik.azurewebsites.net/api/EmployeeFiles?securityCode=";
        public static string GetUriEmployeeFiles = "https://elogik.azurewebsites.net/api/EmployeeFiles";
        public static string GetUriEmployeeFilesLastId = "https://elogik.azurewebsites.net/api/EmployeeFiles?syncEmployee=0";
        public static string PostUriEmployeeFiles = "https://elogik.azurewebsites.net/api/EmployeeFiles";
        public static string PutUriEmployeeFiles = "https://elogik.azurewebsites.net/api/EmployeeFiles?id=";

        //DineInTables
        public static string GetUriDineInTables = "https://elogik.azurewebsites.net/api/DineInTables";
        public static string PutUriDineInTables = "https://elogik.azurewebsites.net/api/DineInTables?id=";
        public static string GetUriDineInTablesByTableGroupID = "https://elogik.azurewebsites.net/api/DineInTables?dineInTableGroupID=";
        public static string PostUriDineInTable = "https://elogik.azurewebsites.net/api/DineInTables";

        //DineInTablesGroups
        public static string GetUriDineInTablesGroups = "https://elogik.azurewebsites.net/api/DineInTableGroup";
        public static string DeleteUriDineInTableGroup = "https://elogik.azurewebsites.net/api/DineInTableGroup?id=";
        public static string PutUriDineInTableGroups = "https://elogik.azurewebsites.net/api/DineInTableGroup?id=";
        public static string PostUriDineInTableGroups = "https://elogik.azurewebsites.net/api/DineInTableGroup";


        //MenuItems
        public static string GetUriMenuItemsByMenuGroupID = "https://elogik.azurewebsites.net/api/MenuItems?menuGroupID=";
        public static string GetUriMenuItems = "https://elogik.azurewebsites.net/api/MenuItems";

        //MenuItemsGroups
        public static string GetUriMenuGroups = "https://elogik.azurewebsites.net/api/MenuGroups";

        //OrderTransactions
        public static string GetUriOrderTransactionsByOrderID = "https://elogik.azurewebsites.net/api/OrderTransactions?getOrderID=";
        public static string PostUriOrderTransactionsNotActive = "https://elogik.azurewebsites.net/api/OrderTransactions?type=notactivetable&getDineInTableID=";
        public static string PostUriOrderTransactionsActive = "https://elogik.azurewebsites.net/api/OrderTransactions?type=activetable&getDineInTableID=";
        public static string PostUriOrderTransactionsSync = "https://elogik.azurewebsites.net/api/OrderTransactions?type=sync&getDineInTableID=0";
        public static string GetUriOrderTransactionsByListOfOrderIDs = "https://elogik.azurewebsites.net/api/OrderTransactions?";

        //OrderHeaders
        public static string GetUriOrderHeadersSync = "https://elogik.azurewebsites.net/api/OrderHeaders?sync=Now";
        public static string GetUriActiveOrderHeaders = "https://elogik.azurewebsites.net/api/OrderHeaders?sync=No";
        public static string GetUriActiveOrderHeadersBar = "https://elogik.azurewebsites.net/api/OrderHeaders?sync=Bar";
        public static string GetUriActiveOrderHeadersByEmp = "https://elogik.azurewebsites.net/api/OrderHeaders?sync=EmpLogged&empId=";
        public static string GetUriActiveOrderHeadersRestaurant = "https://elogik.azurewebsites.net/api/OrderHeaders?sync=Restaurant";

        public static string GetUriOrderHeaders = "https://elogik.azurewebsites.net/api/OrderHeaders";
        public static string GetUriOrderHeadersByTableID = "https://elogik.azurewebsites.net/api/OrderHeaders?type=DineInTable&getID=";
        public static string GetUriOrderHeadersByID = "https://elogik.azurewebsites.net/api/OrderHeaders?type=OrderHeader&getID=";
        public static string PutUriOrderHeaders = "https://elogik.azurewebsites.net/api/OrderHeaders?id=";
        public static string PostUriOrderHeaders = "https://elogik.azurewebsites.net/api/OrderHeaders";

        //StationSettings
        public static string GetUriStationSettings = "https://elogik.azurewebsites.net/api/StationSettings?stationName=";
        public static string PostUriStationSettings = "https://elogik.azurewebsites.net/api/StationSettings";


    }
}
