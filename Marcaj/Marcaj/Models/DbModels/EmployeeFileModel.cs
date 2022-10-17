using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public class EmployeeFileModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeFileModel()
        {
            ComplimentaryAmounts = new HashSet<ComplimentaryAmountModel>();
            CustomerCredits = new HashSet<CustomerCreditModel>();
            Emails = new HashSet<EmailModel>();
            Emails1 = new HashSet<EmailModel>();
            AccessDeniedLogs = new HashSet<AccessDeniedLogModel>();
            /*EmployeePayrollHistories = new HashSet<EmployeePayrollHistory>();
            EmployeeSchedules = new HashSet<EmployeeSchedule>();
            EmployeeTimeCards = new HashSet<EmployeeTimeCard>();
            FreqDinerTrackings = new HashSet<FreqDinerTracking>();
            GiftCertificates = new HashSet<GiftCertificate>();
            GiftCertificateUsages = new HashSet<GiftCertificateUsage>();
            NoSaleLogs = new HashSet<NoSaleLog>();
            OnAccountCharges = new HashSet<OnAccountCharge>();
            OnAccountPayments = new HashSet<OnAccountPayment>();
            OrderCheckDriverLicenseLogs = new HashSet<OrderCheckDriverLicenseLog>();
            OrderHeaders = new HashSet<OrderHeader>();
            OrderHeaders1 = new HashSet<OrderHeader>();
            OrderPayments = new HashSet<OrderPayment>();
            OrderRefunds = new HashSet<OrderRefund>();
            OrderVoidLogs = new HashSet<OrderVoidLog>();
            OtherPayments = new HashSet<OtherPayment>();
            PayOuts = new HashSet<PayOut>();
            PayOuts1 = new HashSet<PayOut>();
            PrankCallFiles = new HashSet<PrankCallFile>();
            RegisterCashiers = new HashSet<RegisterCashier>();
            RegisterCashiers1 = new HashSet<RegisterCashier>();*/
        }

        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string MailingAddress { get; set; }
        public string MailingZipCode { get; set; }
        public DateTime? DateHired { get; set; }
        public DateTime? DateReleased { get; set; }
        public bool EmployeeInActive { get; set; }
        public int JobTitleID { get; set; }
        public string SecurityLevel { get; set; }
        public string AccessCode { get; set; }
        public bool TipsReceived { get; set; }
        public string PayBasis { get; set; }
        public float PayRate { get; set; }
        public string ScanCode { get; set; }
        public string DriverLicenseNumber { get; set; }
        public DateTime? DriverLicenseExpires { get; set; }
        public string CarInsurancePolicyCarrier { get; set; }
        public string CarInsurancePolicyNumber { get; set; }
        public DateTime? CarInsurancePolicyExpires { get; set; }
        public string CarInsurancePolicyNotes { get; set; }
        public string PrefUserInterfaceLocale { get; set; }
        public string EmployeeNotes { get; set; }
        public bool OrderEntryUseSecLang { get; set; }
        public bool EmployeeIsDriver { get; set; }
        public int? DefaultOEMenuGroupID { get; set; }
        public bool? UseStaffBank { get; set; }
        public bool? ScheduleNotEnforced { get; set; }
        public bool? UseHostess { get; set; }
        public bool? IsAServer { get; set; }
        public bool? NoCashierOut { get; set; }
        public DateTime? EditTimestamp { get; set; }
        public string PhoneNumber { get; set; }
        public short? RemoteSiteNumber { get; set; }
        public int? RemoteOrigRowID { get; set; }
        public string GlobalID { get; set; }
        public string RowVer { get; set; }
        public DateTime? SynchVer { get; set; }
        public int? StoreNumber { get; set; }
        public bool? DineInNotAvail { get; set; }
        public bool? BarNotAvail { get; set; }
        public bool? TakeOutNotAvail { get; set; }
        public bool? DriveThruNotAvail { get; set; }
        public bool? DeliveryNotAvail { get; set; }
        public bool? UseEmail { get; set; }
        public string EmailAddress { get; set; }
        public string PictureName { get; set; }
        public string HQRowID { get; set; }
        public string LastRowHash { get; set; }
        public short? RowOwner { get; set; }
        public short? HolidayPayScale { get; set; }
        public string RowGUID { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplimentaryAmountModel> ComplimentaryAmounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerCreditModel> CustomerCredits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailModel> Emails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailModel> Emails1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessDeniedLogModel> AccessDeniedLogs { get; set; }
        /*public virtual ZipCode ZipCode { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public virtual ZipCode ZipCode1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeePayrollHistory> EmployeePayrollHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeTimeCard> EmployeeTimeCards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FreqDinerTracking> FreqDinerTrackings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftCertificate> GiftCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftCertificateUsage> GiftCertificateUsages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NoSaleLog> NoSaleLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnAccountCharge> OnAccountCharges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnAccountPayment> OnAccountPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCheckDriverLicenseLog> OrderCheckDriverLicenseLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderHeader> OrderHeaders1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPayment> OrderPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderRefund> OrderRefunds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderVoidLog> OrderVoidLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OtherPayment> OtherPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PayOut> PayOuts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PayOut> PayOuts1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrankCallFile> PrankCallFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisterCashier> RegisterCashiers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisterCashier> RegisterCashiers1 { get; set; }*/
    }
}
