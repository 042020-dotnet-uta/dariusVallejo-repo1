using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp {
    public class OrdeurItemViewModel {
        public IEnumerable<BusinessOrderItem> OrderItems { get; set; }
        public string OrderItemString { get; set; }
    }
}