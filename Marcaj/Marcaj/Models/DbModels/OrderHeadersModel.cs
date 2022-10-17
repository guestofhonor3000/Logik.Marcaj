using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public partial class OrderHeadersModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderHeadersModel()
        {
            this.OrderTransactions = new HashSet<OrderTransactionsModel>();
            /*this.BadCheckFiles = new HashSet<BadCheckFile>();
            this.ComplimentaryAmounts = new HashSet<ComplimentaryAmount>();
            this.OnAccountCharges = new HashSet<OnAccountCharge>();
            this.FreqDinerTrackings = new HashSet<FreqDinerTracking>();
            this.GiftCertificateUsages = new HashSet<GiftCertificateUsage>();
            this.OrderCheckDriverLicenseLogs = new HashSet<OrderCheckDriverLicenseLog>();
            this.OrderPayments = new HashSet<OrderPayment>();
            this.OrderRefunds = new HashSet<OrderRefund>();

            this.OrderVoidLogs = new HashSet<OrderVoidLog>();
            this.PrankCallFiles = new HashSet<PrankCallFile>();*/
        }

        public int OrderID { get; set; }
        public System.DateTime OrderDateTime { get; set; }
        public int EmployeeID { get; set; }
        public int StationID { get; set; }
        public string OrderType { get; set; }
        public Nullable<int> DineInTableID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<float> DeliveryCharge { get; set; }
        public Nullable<float> DeliveryComp { get; set; }
        public Nullable<int> DriverEmployeeID { get; set; }
        public Nullable<System.DateTime> DriverDepartureTime { get; set; }
        public Nullable<System.DateTime> DriverArrivalTime { get; set; }
        public Nullable<System.DateTime> OnHoldUntilTime { get; set; }
        public float SalesTaxRate { get; set; }
        public Nullable<int> DiscountID { get; set; }
        public Nullable<float> DiscountAmount { get; set; }
        public string DiscountBasis { get; set; }
        public bool DiscountTaxable { get; set; }
        public string OrderStatus { get; set; }
        public float AmountDue { get; set; }
        public bool Kitchen1AlreadyPrinted { get; set; }
        public bool Kitchen2AlreadyPrinted { get; set; }
        public bool Kitchen3AlreadyPrinted { get; set; }
        public bool BarAlreadyPrinted { get; set; }
        public bool PackagerAlreadyPrinted { get; set; }
        public Nullable<int> SurchargeID { get; set; }
        public Nullable<float> SurchargeAmount { get; set; }
        public string SurchargeBasis { get; set; }
        public Nullable<float> CashDiscountAmount { get; set; }
        public Nullable<int> CashDiscountApprovalEmpID { get; set; }
        public float SubTotal { get; set; }
        public Nullable<int> GratuityPercent { get; set; }
        public Nullable<float> CashGratuity { get; set; }
        public Nullable<int> CreditID { get; set; }
        public Nullable<float> CreditAmountUsed { get; set; }
        public Nullable<float> DiscountAmountUsed { get; set; }
        public Nullable<float> SurchargeAmountUsed { get; set; }
        public float SalesTaxAmountUsed { get; set; }
        public Nullable<System.DateTime> DriveThruComplete { get; set; }
        public Nullable<float> GSTRate { get; set; }
        public Nullable<float> GSTAmountUsed { get; set; }
        public string BarTabName { get; set; }
        public Nullable<int> ServerBankID { get; set; }
        public bool TableReady { get; set; }
        public Nullable<int> GuestNumber { get; set; }
        public string SpecificCustomerName { get; set; }
        public bool GuestCheckPrinted { get; set; }
        public string ServerBankType { get; set; }
        public Nullable<float> ServerBankAmount { get; set; }
        public Nullable<bool> Kitchen4AlreadyPrinted { get; set; }
        public Nullable<bool> Kitchen5AlreadyPrinted { get; set; }
        public Nullable<bool> Kitchen6AlreadyPrinted { get; set; }
        public Nullable<float> LiquorTaxRate { get; set; }
        public Nullable<float> LiquorTaxAmount { get; set; }
        public Nullable<System.DateTime> EditTimestamp { get; set; }
        public Nullable<short> RemoteSiteNumber { get; set; }
        public Nullable<int> RemoteOrigRowID { get; set; }
        public Nullable<int> FacturaNumber { get; set; }
        public string GlobalID { get; set; }
        public string RowVer { get; set; }
        public Nullable<System.DateTime> SynchVer { get; set; }
        public Nullable<int> StoreNumber { get; set; }
        public Nullable<bool> BarTabPreAuth { get; set; }
        public Nullable<int> ParentOrderID { get; set; }
        public string HQRowID { get; set; }
        public string LastRowHash { get; set; }
        public Nullable<short> RowOwner { get; set; }
        public string RowGUID { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }
        public string EmployeeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
       
        public virtual DineInTableModel DineInTable { get; set; }
        
        public virtual EmployeeFileModel EmployeeFile { get; set; }
        public virtual EmployeeFileModel EmployeeFile1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderTransactionsModel> OrderTransactions { get; set; }
        /* public virtual ICollection<BadCheckFile> BadCheckFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplimentaryAmount> ComplimentaryAmounts { get; set; }
        public virtual CustomerCredit CustomerCredit { get; set; }
        public virtual CustomerFile CustomerFile { get; set; }
         public virtual Discount Discount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnAccountCharge> OnAccountCharges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FreqDinerTracking> FreqDinerTrackings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftCertificateUsage> GiftCertificateUsages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCheckDriverLicenseLog> OrderCheckDriverLicenseLogs { get; set; }
        public virtual StationSetting StationSetting { get; set; }
        public virtual Surcharge Surcharge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPayment> OrderPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderRefund> OrderRefunds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderVoidLog> OrderVoidLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrankCallFile> PrankCallFiles { get; set; }*/
    }
}
