using System;
using System.Collections.Generic;

namespace StoreApp.Data {
    public class Product {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }

        public ICollection<Inventory> Inventories { get; set; }
    }
}
