using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.BusinessLogic {
    /// <summary>
    /// View model representation of a database location
    /// </summary>
    public class BusinessLocation {
        public int LocationId { get; set; }

        [Display(Name="store")]
        public string LocationName { get; set; }

        public IEnumerable<BusinessInventory> Inventories { get; set; }
    }
}
