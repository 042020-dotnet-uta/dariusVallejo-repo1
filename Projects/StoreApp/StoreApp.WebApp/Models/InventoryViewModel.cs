using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp {
    /// <summary>
    /// View model representing a list of database inventories
    /// </summary>
    public class InventoryViewModel {
        public IEnumerable<BusinessInventory> Inventories { get; set; }
    }
}