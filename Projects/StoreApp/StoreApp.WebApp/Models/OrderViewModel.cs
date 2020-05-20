using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp {
    /// <summary>
    /// View model representing lists of database orders and order items
    /// </summary>
    public class OrderViewModel {
        public IEnumerable<BusinessOrder> Orders { get; set; }
        public IEnumerable<BusinessOrderItem> OrderItems { get; set; }
    }
}