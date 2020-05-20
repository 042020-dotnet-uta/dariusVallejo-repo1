using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.BusinessLogic {
    /// <summary>
    /// View model representation of a database product
    /// </summary>
    public class BusinessProduct {
        public int ProductId { get; set; }

        [Display(Name="product")]
        public string ProductName { get; set; }

        [Display(Name="price")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float ProductPrice { get; set; }
        
        public int LocationId { get; set; }

        [Display(Name="quantity")]
        public int Quantity { get; set; }
        public ISet<BusinessInventory> Inventories { get; set; }
    }
}
