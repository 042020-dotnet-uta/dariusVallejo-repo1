using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApp {
    public class ProductViewModel {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
    }
}