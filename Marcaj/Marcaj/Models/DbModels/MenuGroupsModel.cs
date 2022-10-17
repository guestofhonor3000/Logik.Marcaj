using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public partial class MenuGroupsModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuGroupsModel()
        {
            this.MenuItems = new HashSet<MenuItemsModel>();
        }

        public int MenuGroupID { get; set; }
        public string MenuGroupText { get; set; }
        public int DisplayIndex { get; set; }
        public bool MenuGroupInActive { get; set; }
        public string SecLangMenuGroupText { get; set; }
        public Nullable<System.DateTime> AvailStartTime { get; set; }
        public Nullable<System.DateTime> AvailStopTime { get; set; }
        public string PictureName { get; set; }
        public bool ShowCaption { get; set; }
        public Nullable<int> ButtonColor { get; set; }
        public Nullable<System.DateTime> EditTimestamp { get; set; }
        public Nullable<short> RemoteSiteNumber { get; set; }
        public Nullable<int> RemoteOrigRowID { get; set; }
        public string GlobalID { get; set; }
        public string RowVer { get; set; }
        public Nullable<System.DateTime> SynchVer { get; set; }
        public Nullable<int> StoreNumber { get; set; }
        public Nullable<bool> DineInNotAvail { get; set; }
        public Nullable<bool> BarNotAvail { get; set; }
        public Nullable<bool> TakeOutNotAvail { get; set; }
        public Nullable<bool> DriveThruNotAvail { get; set; }
        public Nullable<bool> DeliveryNotAvail { get; set; }
        public Nullable<bool> DefaultGroupDineIn { get; set; }
        public Nullable<bool> DefaultGroupBar { get; set; }
        public Nullable<bool> DefaultGroupToGo { get; set; }
        public string HQRowID { get; set; }
        public string LastRowHash { get; set; }
        public Nullable<short> RowOwner { get; set; }
        public string RowGUID { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuItemsModel> MenuItems { get; set; }
    }
}
