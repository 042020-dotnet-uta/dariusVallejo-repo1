using System;

namespace StoreApp.Data {
    public class Inventory {
        public int InventoryId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Location Location { get; set; }
    }
}
