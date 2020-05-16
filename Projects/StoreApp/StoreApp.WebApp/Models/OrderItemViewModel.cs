using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApp {
    public class OrderItemViewModel {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }

        public int LocationId { get; set; }
        public int Quantity { get; set; }
    }
}