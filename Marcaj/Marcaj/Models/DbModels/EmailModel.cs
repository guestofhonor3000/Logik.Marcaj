using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public class EmailModel
    {
        public int ToEmployeeID { get; set; }
        public System.DateTime EmailDateTime { get; set; }
        public string Message { get; set; }
        public int FromEmployeeID { get; set; }
        public int EmailID { get; set; }
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
        public virtual EmployeeFileModel EmployeeFile1 { get; set; }
    }
}
