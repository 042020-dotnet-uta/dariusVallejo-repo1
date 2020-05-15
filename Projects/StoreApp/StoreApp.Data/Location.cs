using System;
using System.Collections.Generic;

namespace StoreApp.Data {
    public class Location {
        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public ICollection<Inventory> Inventories { get; set; }
    }
}
