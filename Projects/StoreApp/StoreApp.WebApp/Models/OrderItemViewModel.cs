using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp {
    /// <summary>
    /// View model representing a database order item
    /// </summary>
    public class OrderItemViewModel {
        public int OrderItemId { get; set; }

        [Display(Name="quantity")]
        public int Quantity { get; set; }

        public BusinessProduct Product { get; set; }
        public BusinessLocation Location { get; set; }
    }
}