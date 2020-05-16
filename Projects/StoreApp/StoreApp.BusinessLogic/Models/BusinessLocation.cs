using System;
using System.Collections.Generic;

namespace StoreApp.BusinessLogic {
    public class BusinessLocation {
        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public IEnumerable<BusinessInventory> Inventories { get; set; }
    }
}
