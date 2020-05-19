using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp.Controllers {
    public class InventoryController : Controller {
        private readonly IRepository _repository;

        public InventoryController(IRepository repository) {
            _repository = repository;
        }

        // POST: Product
        public async Task<IActionResult> Index(string locationName, string searchString) {
            var inventories = await _repository.listInventoriesAsync();
            if (!String.IsNullOrWhiteSpace(locationName)) {
                inventories = inventories.Where(i => i.Location.LocationName == locationName);
            }
            if (!String.IsNullOrWhiteSpace(searchString)) {
                    searchString = searchString.Trim().ToUpper();
                    inventories = inventories.Where(i => i.Product.ProductName.ToUpper().Contains(searchString));
            }
            var inventoryView = new InventoryViewModel {
                Inventories = inventories
            };
            ViewData["Locations"] = await _repository.listLocationsAsync();
            return View(inventoryView);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int id) {
            var inventories = await _repository.listInventoriesAsync();
            var inventory = inventories.Where(i => i.InventoryId == id).FirstOrDefault();
            ViewData["Stock"] = inventory.Quantity;
            return View(new OrderItemViewModel {
                Product = inventory.Product,
                Location = inventory.Location,
                Quantity = 1,
            });
        }
    }
}