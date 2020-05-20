using System;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.BusinessLogic {
    /// <summary>
    /// View model representation of database inventory
    /// </summary>
    public class BusinessInventory {
        public int InventoryId { get; set; }

        [Display(Name="quantity")]
        public int Quantity { get; set; }

        [Display(Name="quantity")]
        public int Stock { get; set; }

        public BusinessProduct Product { get; set; }
        public BusinessLocation Location { get; set; }
    }
}
