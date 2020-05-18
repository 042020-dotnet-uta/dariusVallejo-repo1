using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp {
    public class OrderViewModel {
        public IEnumerable<BusinessOrder> Orders { get; set; }
    }
}