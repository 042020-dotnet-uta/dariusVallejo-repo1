using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreApp.WebApp {
    public class OrderItemViewModel {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public int Quantity { get; set; }
    }
}