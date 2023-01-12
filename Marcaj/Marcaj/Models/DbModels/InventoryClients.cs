using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public partial class InventoryClients
    {
        public int InventoryClientID { get; set; }
        public string InventoryClientText { get; set; }
        public Nullable<bool> InventoryClientInActive { get; set; }
        public string RowGUID { get; set; }
        public string InventoryClientText2 { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }
    }
}
