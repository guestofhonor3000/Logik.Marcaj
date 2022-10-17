using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels
{
    public partial class OrderTransactionsModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderTransactionsModel()
        {
            /*this.MenuExplosions = new HashSet<MenuExplosion>();
            this.OrderVoidLogs = new HashSet<OrderVoidLog>();*/
        }

        public int OrderTransactionID { get; set; }
        public int OrderID { get; set; }
        public int MenuItemID { get; set; }
        public string MenuItemTextOT { get; set; }
        public string MenuItemAutoPriceText { get; set; }
        public float MenuItemUnitPrice { get; set; }
        public float Quantity { get; set; }
        public float ExtendedPrice { get; set; }
        public Nullable<int> DiscountID { get; set; }
        public Nullable<float> DiscountAmount { get; set; }
        public string DiscountBasis { get; set; }
        public bool DiscountTaxable { get; set; }
        public string TransactionStatus { get; set; }
        public string NotificationStatus { get; set; }
        public Nullable<int> Mod1ID { get; set; }
        public Nullable<float> Mod1Cost { get; set; }
        public Nullable<int> Mod2ID { get; set; }
        public Nullable<float> Mod2Cost { get; set; }
        public Nullable<int> Mod3ID { get; set; }
        public Nullable<float> Mod3Cost { get; set; }
        public Nullable<int> Mod4ID { get; set; }
        public Nullable<float> Mod4Cost { get; set; }
        public Nullable<int> Mod5ID { get; set; }
        public Nullable<float> Mod5Cost { get; set; }
        public Nullable<int> Mod6ID { get; set; }
        public Nullable<float> Mod6Cost { get; set; }
        public Nullable<int> Mod7ID { get; set; }
        public Nullable<float> Mod7Cost { get; set; }
        public Nullable<int> Mod8ID { get; set; }
        public Nullable<float> Mod8Cost { get; set; }
        public Nullable<int> Mod9ID { get; set; }
        public Nullable<float> Mod9Cost { get; set; }
        public Nullable<int> Mod10ID { get; set; }
        public Nullable<float> Mod10Cost { get; set; }
        public Nullable<int> Mod11ID { get; set; }
        public Nullable<float> Mod11Cost { get; set; }
        public Nullable<int> Mod12ID { get; set; }
        public Nullable<float> Mod12Cost { get; set; }
        public Nullable<int> Mod13ID { get; set; }
        public Nullable<float> Mod13Cost { get; set; }
        public Nullable<int> Mod14ID { get; set; }
        public Nullable<float> Mod14Cost { get; set; }
        public Nullable<int> Mod15ID { get; set; }
        public Nullable<float> Mod15Cost { get; set; }
        public Nullable<int> Mod16ID { get; set; }
        public Nullable<float> Mod16Cost { get; set; }
        public Nullable<int> Mod17ID { get; set; }
        public Nullable<float> Mod17Cost { get; set; }
        public Nullable<int> Mod18ID { get; set; }
        public Nullable<float> Mod18Cost { get; set; }
        public Nullable<int> Mod19ID { get; set; }
        public Nullable<float> Mod19Cost { get; set; }
        public Nullable<int> Mod20ID { get; set; }
        public Nullable<float> Mod20Cost { get; set; }
        public Nullable<float> DiscountAmountUsed { get; set; }
        public Nullable<int> SeatNumber { get; set; }
        public Nullable<System.DateTime> OnHoldUntilTime { get; set; }
        public Nullable<bool> GSTTaxable { get; set; }
        public string ShortNote { get; set; }
        public Nullable<bool> FoodStampsPayable { get; set; }
        public Nullable<bool> LiquorTaxApplied { get; set; }
        public Nullable<bool> PizzaLabelPrinted { get; set; }
        public Nullable<System.DateTime> EditTimestamp { get; set; }
        public Nullable<short> RemoteSiteNumber { get; set; }
        public Nullable<int> RemoteOrigRowID { get; set; }
        public string GlobalID { get; set; }
        public string RowVer { get; set; }
        public Nullable<System.DateTime> SynchVer { get; set; }
        public Nullable<int> StoreNumber { get; set; }
        public Nullable<bool> Kitchen1Printed { get; set; }
        public Nullable<bool> Kitchen2Printed { get; set; }
        public Nullable<bool> Kitchen3Printed { get; set; }
        public Nullable<bool> Kitchen4Printed { get; set; }
        public Nullable<bool> Kitchen5Printed { get; set; }
        public Nullable<bool> Kitchen6Printed { get; set; }
        public Nullable<bool> BarPrinted { get; set; }
        public Nullable<int> CourseNumber { get; set; }
        public Nullable<bool> ItemFired { get; set; }
        public string HQRowID { get; set; }
        public string LastRowHash { get; set; }
        public Nullable<short> RowOwner { get; set; }
        public string RowGUID { get; set; }
        //public byte[] SSMA_TimeStamp { get; set; }
        public virtual MenuItemsModel MenuItem { get; set; }
        //public virtual Discount Discount { get; set; }
        
        public virtual OrderHeadersModel OrderHeader { get; set; }
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuExplosion> MenuExplosions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderVoidLog> OrderVoidLogs { get; set; }*/
    }
}
