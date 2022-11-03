using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public partial class DineInTableModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DineInTableModel()
        {
            //this.OrderHeaders = new HashSet<OrderHeader>();
        }

        public int DineInTableID { get; set; }
        public string DineInTableText { get; set; }
        public int TableGroupID { get; set; }
        public int DisplayIndex { get; set; }
        public bool DineInTableInActive { get; set; }
        public Nullable<int> MaxGuests { get; set; }
        public Nullable<bool> Hibachi { get; set; }
        public string HBStyle { get; set; }
        public string DisplayPosition { get; set; }
        public string HibachiBridgeTo { get; set; }
        public Nullable<short> HibachiBridgeSeats { get; set; }
        public Nullable<short> HBLeftSeats { get; set; }
        public Nullable<short> HBTopSeats { get; set; }
        public Nullable<short> HBBottomSeats { get; set; }
        public Nullable<short> HBRightSeats { get; set; }
        public Nullable<bool> Smoking { get; set; }
        public Nullable<bool> Window { get; set; }
        public Nullable<bool> Booth { get; set; }
        public Nullable<bool> Privacy { get; set; }
        public string PictureName { get; set; }
        public Nullable<System.DateTime> EditTimestamp { get; set; }
        public Nullable<short> RemoteSiteNumber { get; set; }
        public Nullable<int> RemoteOrigRowID { get; set; }
        public string GlobalID { get; set; }
        public string RowVer { get; set; }
        public Nullable<System.DateTime> SynchVer { get; set; }
        public Nullable<int> StoreNumber { get; set; }
        public string HQRowID { get; set; }
        public string LastRowHash { get; set; }
        public Nullable<short> RowOwner { get; set; }
        public string RowGUID { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }

        //public virtual DineInTableGroup DineInTableGroup { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderHeadersModel> OrderHeaders { get; set; }
    }
}
