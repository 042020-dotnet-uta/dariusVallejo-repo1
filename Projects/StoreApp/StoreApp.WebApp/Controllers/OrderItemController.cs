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
                var orderItems = await _repository.listOrderItemsAsync();
                orderItems = orderItems.Where(oi => oi.Order.Customer.CustomerId == customerId && oi.Order.OrderDate == null);
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
        public async Task<IActionResult> Details(int id)
        {
            var orderItems = await _repository.listOrderItemsAsync();
            orderItems = orderItems.Where(oi => oi.Order.OrderId == id);
            var orderItemView = new OrdeurItemViewModel {
                OrderItems = orderItems
            };
            return View(orderItemView);
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
                var orderItems = await _repository.listOrderItemsAsync();
                orderItems = orderItems.Where(oi => oi.Order.OrderId == orderId);
                
                foreach (var orderItem in orderItems) {
                    int productId = orderItem.Product.ProductId;
                    var product = await _repository.getProductAsync(productId, locationId);
                    // await _repository.updateInventoryAsync(locationId, orderItem);
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
            var sessionValue = HttpContext.Session.GetInt32("CustomerId");
            if (ModelState.IsValid && sessionValue != null)
            {
                int customerId = Convert.ToInt32(sessionValue);

                // get the order with no order date for this location
                await _repository.createOrderAsync(customerId, locationId);
                var orders = await _repository.listOrdersByCustomerAsync(customerId);
                var order = orders.Where(o => o.Location.LocationId == locationId &&  o.OrderDate == null)
                .FirstOrDefault();

                // Make a new order item for this product
                BusinessOrderItem orderItem = new BusinessOrderItem {
                    OrderItemId = -1,
                    Quantity = orderItemView.Quantity,
                    Order = order,
                    Product = new BusinessProduct {
                        ProductId = orderItemView.ProductId,
                        ProductName = orderItemView.ProductName,
                        ProductPrice = orderItemView.ProductPrice
                    }
                };

                // if the order already has the incoming product id...
                var existingOrderItems = await _repository.listOrderItemsAsync();
                var existingOrderItem = existingOrderItems.Where(oi => oi.Order.OrderId == order.OrderId && oi.Order.Location.LocationId == locationId && oi.Product.ProductId == orderItemView.ProductId).FirstOrDefault();

                if (existingOrderItem != null) {
                    // orderItem.Quantity += existingOrderItem.Quantity;
                    orderItem.OrderItemId = existingOrderItem.OrderItemId;
                }

                // update the order in the system
                order.Total += orderItemView.Quantity * orderItemView.ProductPrice;
                var inventory = await _repository.updateOrderAsync(order, orderItem);
                if (inventory == null) {
                    ModelState.AddModelError("Quantity", "Please input valid quantity.");
                } else {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
            // return View(orderItem);
        }

        public async Task<IActionResult> List(string searchString, string locationName) {
            var sessionValue = HttpContext.Session.GetInt32("CustomerId");
            var username = HttpContext.Session.GetString("Username");
            if (sessionValue != null) {
                int customerId = Convert.ToInt32(sessionValue);
                var locations = await _repository.listLocationsAsync();
                var orders = await _repository.listOrdersAsync();
                orders = orders.Where(o => o.OrderDate != null);
                if (username != "admin") {
                    orders = orders.Where(o => o.Customer.CustomerId == customerId);
                }

                var orderView = new OrderViewModel {
                    Orders = orders
                };
                ViewData["Locations"] = await _repository.listLocationsAsync();
                return View(orderView);
            }
            return RedirectToAction("Index", "Home");
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
