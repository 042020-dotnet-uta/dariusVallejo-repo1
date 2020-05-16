using System;
using System.Collections.Generic;

namespace StoreApp.BusinessLogic {
    public class BusinessProduct {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        
        public int LocationId { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<BusinessInventory> Inventories { get; set; }
    }
}
