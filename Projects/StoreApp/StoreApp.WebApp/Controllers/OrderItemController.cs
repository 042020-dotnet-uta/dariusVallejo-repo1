using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.BusinessLogic;
using Microsoft.AspNetCore.Http;

namespace StoreApp.WebApp.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly BusinessContext _context;
        private readonly IRepository _repository;

        public OrderItemController(BusinessContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: OrderItem
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderItems.ToListAsync());
        }

        // GET: OrderItem/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(m => m.OrderItemId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItemViewModel orderItemView)
        {
            string username = HttpContext.Session.GetString("Username");
            if (ModelState.IsValid && username != null)
            {
                // get customer for username
                BusinessCustomer businessCustomer = await _repository.getCustomerByUsernameAsync(username);

                // find all orders for a customer id
                var businessOrders = await _repository.listOrdersByCustomerAsync(businessCustomer.CustomerId);

                // if there isn't one, make it
                if (businessOrders.Count() == 0) {
                    
                } else {
                    // otherwise, get the one WITHOUT an order date

                    // add the order item to the order
                }

                // _context.Add(orderItem);
                // await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
            // return View(orderItem);
        }

        // GET: OrderItem/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderItemId,Quantity")] OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.OrderItemId))
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
            return View(orderItem);
        }

        // GET: OrderItem/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(m => m.OrderItemId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(string id)
        {
            return _context.OrderItems.Any(e => e.OrderItemId == id);
        }
    }
}
