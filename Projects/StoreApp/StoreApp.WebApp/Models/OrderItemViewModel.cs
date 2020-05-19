using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp {
    public class OrderItemViewModel {
        public int OrderItemId { get; set; }

        // public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        // public string SearchString { get; set; }

        public int Quantity { get; set; }

        public BusinessProduct Product { get; set; }
        // public BusinessLocation Location { get; set; }
    }
}