using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApp {
    public class ProductViewModel {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        
        public int LocationId { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }
    }
}