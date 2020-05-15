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
    public class ProductController : Controller
    {
        private readonly BusinessContext _context;
        private readonly IRepository _repository;

        public ProductController(BusinessContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // POST: Product
        public async Task<IActionResult> Index(int id) {
            var databaseProducts = await _repository.listProductsAsync(id);
            var productViews = databaseProducts.Select(
                databaseProduct => new ProductViewModel {
                    ProductId = databaseProduct.ProductId,
                    ProductName = databaseProduct.ProductName,
                    ProductPrice = databaseProduct.ProductPrice,
                    LocationId = id,
                    Quantity = databaseProduct.Quantity
                }
            );
            return View(productViews);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int productId, int locationId)
        {
            // if (productId == null)
            // {
            //     return NotFound();
            // }

            // var product = await _context.Products
            //     .FirstOrDefaultAsync(m => m.ProductId == id);
            var databaseProduct = await _repository.getProductAsync(productId, locationId);
            if (databaseProduct == null)
            {
                return NotFound();
            }
            ProductViewModel productView = new ProductViewModel {
                ProductId = databaseProduct.ProductId,
                ProductName = databaseProduct.ProductName,
                ProductPrice = databaseProduct.ProductPrice,
                Quantity = databaseProduct.Quantity
            };

            return View(productView);
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
