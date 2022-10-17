using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public partial class AccessDeniedLogModel
    {
        public System.DateTime AccessDeniedDateTime { get; set; }
        public int StationID { get; set; }
        public string AccessDeniedType { get; set; }
        public Nullable<int> AttemptedAccessCode { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string AttemptedFeatureName { get; set; }
        public int AutoID { get; set; }
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

        public virtual EmployeeFileModel EmployeeFile { get; set; }
        //public virtual StationSetting StationSetting { get; set; }
    }
}
