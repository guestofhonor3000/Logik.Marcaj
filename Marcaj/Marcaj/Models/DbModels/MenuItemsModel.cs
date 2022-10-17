using System;
using System.Collections.Generic;
using System.Text;
using Marcaj.Models.DbModels;

namespace Marcaj.Models.DbModels
{
    public partial class MenuItemsModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuItemsModel()
        {
            //this.MenuItemPrices = new HashSet<MenuItemPrice>();
            //this.MenuItemIngredients = new HashSet<MenuItemIngredient>();
            //this.MenuModifierPopUps = new HashSet<MenuModifierPopUp>();
            //this.OrderTransactions = new HashSet<OrderTransaction>();
        }

        public int MenuItemID { get; set; }
        public string MenuItemText { get; set; }
        public int MenuCategoryID { get; set; }
        public int MenuGroupID { get; set; }
        public int DisplayIndex { get; set; }
        public float DefaultUnitPrice { get; set; }
        public string MenuItemDescription { get; set; }
        public string MenuItemNotification { get; set; }
        public bool MenuItemInActive { get; set; }
        public bool MenuItemInStock { get; set; }
        public bool MenuItemTaxable { get; set; }
        public bool MenuItemDiscountable { get; set; }
        public string MenuItemType { get; set; }
        public Nullable<int> MenuItemPopUpHeaderID { get; set; }
        public string MenuItemPopUpChoiceText { get; set; }
        public bool HasModifierPopUps { get; set; }
        public string SecLangMenuItemText { get; set; }
        public string SecLangPopUpChoiceText { get; set; }
        public string PictureName { get; set; }
        public bool ShowCaption { get; set; }
        public bool GSTApplied { get; set; }
        public bool Pizza { get; set; }
        public bool Bar { get; set; }
        public string DefaultModifierType { get; set; }
        public Nullable<int> ButtonColor { get; set; }
        public string Barcode { get; set; }
        public string GasPump { get; set; }
        public Nullable<bool> FoodStampsPayable { get; set; }
        public Nullable<bool> LiquorTaxApplied { get; set; }
        public Nullable<float> ItemDelCharge { get; set; }
        public Nullable<float> ItemDelComp { get; set; }
        public Nullable<int> JumpMenuGroupID { get; set; }
        public string LargePictureName { get; set; }
        public Nullable<float> DineInPrice { get; set; }
        public Nullable<float> BarTabPrice { get; set; }
        public Nullable<float> TakeOutPrice { get; set; }
        public Nullable<float> DriveThruPrice { get; set; }
        public Nullable<float> DeliveryPrice { get; set; }
        public bool OrderByWeight { get; set; }
        public bool PrintPizzaLabel { get; set; }
        public Nullable<int> KitchenSortNumber { get; set; }
        public Nullable<int> ModBuilderTemplateID { get; set; }
        public Nullable<System.DateTime> EditTimestamp { get; set; }
        public Nullable<short> RemoteSiteNumber { get; set; }
        public Nullable<int> RemoteOrigRowID { get; set; }
        public Nullable<float> QtyCountDown { get; set; }
        public Nullable<float> MenuItemCost { get; set; }
        public Nullable<short> PizzaSize { get; set; }
        public Nullable<short> PizzaToppingThreshold { get; set; }
        public Nullable<bool> PizzaToppingFreeInThreshold { get; set; }
        public string ScreenName { get; set; }
        public string ScreenAltName { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptAltName { get; set; }
        public string ChitName { get; set; }
        public string ChitAltName { get; set; }
        public Nullable<bool> PrintToKitchen1 { get; set; }
        public Nullable<bool> PrintToKitchen2 { get; set; }
        public Nullable<bool> PrintToKitchen3 { get; set; }
        public Nullable<bool> PrintToKitchen4 { get; set; }
        public Nullable<bool> PrintToKitchen5 { get; set; }
        public Nullable<bool> PrintToKitchen6 { get; set; }
        public Nullable<bool> PrintToBar { get; set; }
        public Nullable<bool> PrintItemDetailOnReceipt { get; set; }
        public Nullable<bool> PrintItemDetailToKitchen { get; set; }
        public Nullable<bool> PrintItemDetailToBar { get; set; }
        public string PrintableItemDetails { get; set; }
        public string ScriptDetails { get; set; }
        public string MixMatchCode { get; set; }
        public Nullable<bool> DoNotPrintOnReceipt { get; set; }
        public Nullable<bool> OpenPriceItem { get; set; }
        public Nullable<bool> OpenPriceAskDescription { get; set; }
        public Nullable<short> OpenPriceSecurityLevel { get; set; }
        public Nullable<int> LCPLU { get; set; }
        public Nullable<bool> AllowPostEntryAdjustQty { get; set; }
        public Nullable<float> MinProfitPercent { get; set; }
        public Nullable<bool> DisallowDiscountIfAutoPriced { get; set; }
        public Nullable<float> MinimumPrice { get; set; }
        public Nullable<float> MaximumPrice { get; set; }
        public Nullable<bool> HideFromPrepForecast { get; set; }
        public string HQRowID { get; set; }
        public string LastRowHash { get; set; }
        public Nullable<short> RowOwner { get; set; }
        public Nullable<int> LotMaxQty { get; set; }
        public Nullable<bool> LotChild { get; set; }
        public Nullable<bool> ShowQtyInTBS { get; set; }
        public Nullable<int> MaxQtyInputPerItem { get; set; }
        public string RowGUID { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }

        //public virtual MenuCategory MenuCategory { get; set; }
        public virtual MenuGroupsModel MenuGroup { get; set; }
        /* [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         public virtual ICollection<MenuItemPrice> MenuItemPrices { get; set; }
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         public virtual ICollection<MenuItemIngredient> MenuItemIngredients { get; set; }
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         public virtual ICollection<MenuModifierPopUp> MenuModifierPopUps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         public virtual ICollection<OrderTransactionsModel> OrderTransactions { get; set; }*/
    }

}
