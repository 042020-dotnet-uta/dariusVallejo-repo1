using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.BusinessLogic;

namespace StoreApp.WebApp.Controllers
{
    public class InventoryController : Controller
    {
        private readonly BusinessContext _context;
        private readonly IRepository _repository;

        public InventoryController(BusinessContext context, IRepository repository)
        {
            _context = context;
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
        public async Task<IActionResult> Details(int id)
        {
            var inventories = await _repository.listInventoriesAsync();
            var inventory = inventories.Where(i => i.InventoryId == id).FirstOrDefault();
            ViewData["Stock"] = inventory.Quantity;
            return View(new OrderItemViewModel {
                Product = inventory.Product,
                // Location = inventory.Location,
                ProductId = inventory.Product.ProductId,
                ProductName = inventory.Product.ProductName,
                ProductPrice = inventory.Product.ProductPrice,

                LocationId = inventory.Location.LocationId,
                LocationName = inventory.Location.LocationName,
                
                Quantity = 1,
            });
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
