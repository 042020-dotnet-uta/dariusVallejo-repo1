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
            // TODO List all locations carts for customer
            var sessionValue = HttpContext.Session.GetInt32("CustomerId");
            if (sessionValue != null) {
                int customerId = Convert.ToInt32(sessionValue);
                var orderItems = await _repository.listOrderItemsAsync(customerId, -1);
                if (orderItems != null) {
                    var orderItemViews = orderItems.Select(oi => new OrderItemViewModel {
                        OrderId = oi.Order.OrderId,
                        ProductId = oi.Product.ProductId,
                        ProductName = oi.Product.ProductName,
                        ProductPrice = oi.Product.ProductPrice,
                        Quantity = oi.Quantity
                    });
                    return View(orderItemViews);   
                } else {
                    ModelState.AddModelError(string.Empty, "Your cart is empty.");
                }
            }
            return View(new List<OrderItemViewModel>());
            // return View(orderItemView);
            // return View(await _context.OrderItems.ToListAsync());
        }

        // GET: OrderItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var sessionValue = HttpContext.Session.GetInt32("CustomerId");
            if (sessionValue != null) {
                int customerId = Convert.ToInt32(sessionValue);

                if (id == null) {
                    // (IEnumerable) orderItemViews = HttpContext.Current.Session["orderItemViews"];
                    return View();
                } else {
                    // TODO Specific order details
                }

            }
            return View();
        }

        // TODO Add empty order check
        public async Task<IActionResult> Submit()
        {
            var sessionValue = HttpContext.Session.GetInt32("CustomerId");
            int customerId = Convert.ToInt32(sessionValue);

            // get all the orders for the customer that has no order date
            var orders = await _repository.listOrdersByCustomerAsync(customerId);
            orders = orders.Where(o => o.OrderDate == null);

            List<OrderItemViewModel> orderItemViews = new List<OrderItemViewModel>();
            
            // for each order item in each order, update the inventory for that item at that location
            foreach (var order in orders) {
                int orderId = order.OrderId;
                int locationId = order.Location.LocationId;
                var orderItems = await _repository.listOrderItemsAsync(customerId, orderId);
                
                foreach (var orderItem in orderItems) {
                    int productId = orderItem.Product.ProductId;
                    var product = await _repository.getProductAsync(productId, locationId);
                    // await _repository.updateInventory(inventoryId, orderItem)?
                    await _repository.updateInventoryAsync(locationId, orderItem);
                    await _repository.submitOrderAsync(order);
                    orderItemViews.Add(new OrderItemViewModel {
                        OrderItemId = orderItem.OrderItemId,
                        OrderId = orderId,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        Quantity = orderItem.Quantity
                    });
                }
            }
            
            // print the order items to the screen
            // TempData["orderItemViews"] = orderItemViews;
            // HttpContext.Current.Session.Add("orderItemViews", orderItemViews);
            return View(orderItemViews);
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
        public async Task<IActionResult> Create(OrderItemViewModel orderItemView, int locationId)
        {
            Console.WriteLine(orderItemView.LocationId);
            Console.WriteLine(orderItemView.ProductId);
            var sessionValue = HttpContext.Session.GetInt32("CustomerId");
            if (ModelState.IsValid && sessionValue != null)
            {
                int customerId = Convert.ToInt32(sessionValue);
                // find all orders for a customer id
                await _repository.createOrderAsync(customerId, locationId);
                var orders = await _repository.listOrdersByCustomerAsync(customerId);
                var order = orders.Where(o => o.OrderDate == null).FirstOrDefault();

                // add the order item to the order
                BusinessOrderItem orderItem = new BusinessOrderItem {
                    Quantity = orderItemView.Quantity,
                    Order = order,
                    Product = new BusinessProduct {
                        ProductId = orderItemView.ProductId,
                        ProductName = orderItemView.ProductName,
                        ProductPrice = orderItemView.ProductPrice
                    }
                };
                // update the order in the system
                await _repository.updateOrderAsync(order, orderItem);
                return RedirectToAction(nameof(Index));
            }
            return View();
            // return View(orderItem);
        }

        // GET: OrderItem/Edit/5
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(int id, [Bind("OrderItemId,Quantity")] OrderItem orderItem)
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
        public async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.OrderItemId == id);
        }
    }
}
