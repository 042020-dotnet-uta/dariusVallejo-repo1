using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp {
    public class OrderItemViewModel {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }

        public BusinessProduct Product { get; set; }
        public BusinessLocation Location { get; set; }
    }
}