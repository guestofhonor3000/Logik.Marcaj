using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.LocalDbModels
{
    public class LMenuItemsModel
    {
        public int MenuItemID { get;set; }
        public string MenuItemText { get;set; }
        public int MenuCategoryID { get;set; }    
        public int MenuGroupID { get;set; }
        public int DisplayIndex { get;set; }
        public Single DefaultUnitPrice { get;set; }
        public string MenuItemNotification { get; set; }
        public bool MenuItemInActive { get; set; }
        public bool MenuItemInStock { get; set; }
        public bool MenuItemTaxable { get; set; }
        public bool MenuItemDiscountable { get; set; }
        public string MenuItemType { get; set; }
        public bool HasModifierPopUps { get; set; }
        public bool ShowCaption { get; set; }
        public bool GSTApplied { get; set; }
        public bool Pizza { get; set; }
        public bool Bar { get; set; }
        public bool OrderByWeight { get; set; }
        public bool PrintPizzaLabel { get; set; }

    }
}
