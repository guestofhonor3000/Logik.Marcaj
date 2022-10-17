using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.LocalDbModels
{
    public class LOrderTransactionsModel
    {
        public int OrderTransactionID { get; set; }
        public int OrderID { get; set; }
        public int MenuItemID { get; set; }
        public float MenuItemUnitPrice { get; set; }
        public float Quantity { get; set; }
        public float ExtendedPrice { get; set; }
        public bool DiscountTaxable { get; set; }
        public string TransactionStatus { get; set; }
        public string NotificationStatus { get; set; }
        public string RowGUID { get; set; }
        public string MenuItemTextOT { get; set; }

    }
}
