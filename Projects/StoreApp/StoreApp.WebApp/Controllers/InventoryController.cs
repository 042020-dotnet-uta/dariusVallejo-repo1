using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.BusinessLogic;
using Microsoft.Extensions.Logging;

namespace StoreApp.WebApp.Controllers {
    /// <summary>
    /// Manages drawing inventory and interacting with inventory repository
    /// </summary>
    public class InventoryController : Controller {

        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public InventoryController(ILogger<InventoryController> logger, IRepository repository) {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Lists all inventories matching searched location and product strings
        /// </summary>
        /// <param name="locationName">The name of the location to find products for</param>
        /// <param name="searchString">The name of the products to find</param>
        /// <returns>A visual list of all the inventories in the system</returns>
        public async Task<IActionResult> Index(string locationName, string searchString) {
            try {
                var inventories = await _repository.listInventoriesAsync();
                if (!String.IsNullOrWhiteSpace(locationName)) {
                    inventories = inventories.Where(i => i.Location.LocationName == locationName);
                }
                if (!String.IsNullOrWhiteSpace(searchString)) {
                        searchString = searchString.Replace(" ", "").ToUpper();
                        inventories = inventories.Where(i => i.Product.ProductName.ToUpper().Contains(searchString));
                }
                var inventoryView = new InventoryViewModel {
                    Inventories = inventories
                };
                ViewData["Locations"] = await _repository.listLocationsAsync();
                return View(inventoryView);
            } catch (Exception exception) {
                _logger.LogCritical(exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lists the individual details of a line of inventory
        /// </summary>
        /// <param name="id">The id of the line of inventory to show details for</param>
        /// <returns>A view of all the attributes of the inventory item</returns>
        public async Task<IActionResult> Details(int id) {
            try {
                var inventories = await _repository.listInventoriesAsync();
                var inventory = inventories.Where(i => i.InventoryId == id).FirstOrDefault();
                ViewData["Stock"] = inventory.Quantity;
                return View(new OrderItemViewModel {
                    Product = inventory.Product,
                    Location = inventory.Location,
                    Quantity = 1,
                });
            } catch (Exception exception) {
                _logger.LogCritical(exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
