using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.LocalDbModels
{
    public partial class LStationSettingsModel
    {
        public int StationID { get; set; }
        public string ComputerName { get; set; }
        public string ReceiptPrinterPort { get; set; }
        public int ReceiptPrinterType { get; set; }
        public bool CashDrawerAttached { get; set; }
        public string KitchenPrinter1Title { get; set; }
        public string KitchenPrinter1Port { get; set; }
        public int KitchenPrinter1Type { get; set; }
        public string KitchenPrinter1Name { get; set; }
        public bool DedicatedToCashier { get; set; }
        public string UserInterfaceLocale { get; set; }
        public string ReceiptPrinterName { get; set; }
        public Nullable<bool> PopUpBool { get; set; }
        public bool FastBar { get; set; }
        public bool BarTab { get; set; }
        public bool OrderRecallToBrowse { get; set; }
        public bool DefaultToMenuGroupsInOrderEntry { get; set; }
        public bool HasEDC { get; set; }
        public bool DriveThruStation { get; set; }
        public bool KeepInOrderEntryAfterSent { get; set; }
        public string RowGUID { get; set; }
    }
}
