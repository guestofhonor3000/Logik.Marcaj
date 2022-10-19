using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.LocalDbModels
{
    public class LOrderHeaderModel
    {
        public int OrderID { get; set; }
        public System.DateTime OrderDateTime { get; set; }
        public int EmployeeID { get; set; }
        public int StationID { get; set; }
        public string OrderType { get; set; }
        public Nullable<int> DineInTableID { get; set; }
        public string OrderStatus { get; set; }
        public float AmountDue { get; set; }
        public bool Kitchen1AlreadyPrinted { get; set; }
        public bool Kitchen2AlreadyPrinted { get; set; }
        public bool Kitchen3AlreadyPrinted { get; set; }
        public bool BarAlreadyPrinted { get; set; }
        public bool PackagerAlreadyPrinted { get; set; }
        public float SalesTaxAmountUsed { get; set; }
        public bool GuestCheckPrinted { get; set; }
        public float SalesTaxRate { get; set; }
        public float SubTotal { get; set; }
        public Nullable<bool> BarOrder { get; set; }
        public string RowGUID { get; set; }

    }
}
