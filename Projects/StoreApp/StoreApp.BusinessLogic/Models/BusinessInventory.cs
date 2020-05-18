using System;

namespace StoreApp.BusinessLogic {
    public class BusinessInventory {
        public int InventoryId { get; set; }
        public int Quantity { get; set; }

        public BusinessProduct Product { get; set; }
        public BusinessLocation Location { get; set; }
    }
}
