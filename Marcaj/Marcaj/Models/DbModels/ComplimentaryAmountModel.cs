using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public class ComplimentaryAmountModel
    {
        public int ComplimentaryAmountID { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public int CashierID { get; set; }
        public int? NonCashierEmployeeID { get; set; }
        public int OrderID { get; set; }
        public float PaymentAmount { get; set; }
        public DateTime? EditTimestamp { get; set; }
        public short? RemoteSiteNumber { get; set; }
        public int? RemoteOrigRowID { get; set; }
        public string GlobalID { get; set; }
        public string RowVer { get; set; }
        public DateTime? SynchVer { get; set; }
        public int? StoreNumber { get; set; }
        public string HQRowID { get; set; }
        public string LastRowHash { get; set; }
        public short? RowOwner { get; set; }
        public string RowGUID { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }

        //public virtual OrderHeader OrderHeader { get; set; }
        //public virtual RegisterCashier RegisterCashier { get; set; }
        //public virtual EmployeeFile EmployeeFile { get; set; }
    }
}
