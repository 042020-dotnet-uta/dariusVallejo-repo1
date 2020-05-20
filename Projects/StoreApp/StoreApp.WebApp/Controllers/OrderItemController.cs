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
using Microsoft.Extensions.Logging;

namespace StoreApp.WebApp.Controllers
{
    /// <summary>
    /// Handles all order interaction between client and order repository
    /// </summary>
    public class OrderItemController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public OrderItemController(ILogger<OrderItemController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Displays the current cart contents for the current session's customer
        /// </summary>
        /// <returns>Customer's list of all the orders in the system that have NOT been submitted yet (cart)</returns>
        public async Task<IActionResult> Index()
        {
            try {
                var sessionValue = HttpContext.Session.GetInt32("CustomerId");
                if (sessionValue != null) {
                    int customerId = Convert.ToInt32(sessionValue);
                    var orderItems = await _repository.listOrderItemsAsync();
                    orderItems = orderItems.Where(oi => oi.Order.Customer.CustomerId == customerId && oi.Order.OrderDate == null);
                    float total = orderItems.Sum(oi => oi.Product.ProductPrice * oi.Quantity);
                    if (orderItems != null) {
                        var orderItemViews = orderItems.Select(oi => new OrderItemViewModel {
                            Product = oi.Product,
                            Quantity = oi.Quantity
                        });
                        ViewData["Total"] = total;
                        return View(orderItemViews);   
                    } else {
                        ModelState.AddModelError(string.Empty, "Your cart is empty.");
                    }
                }
                return View(new List<OrderItemViewModel>());
            } catch (Exception exception) {
                _logger.LogCritical(exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Shows the details of an order
        /// </summary>
        /// <param name="id">The order id</param>
        /// <returns>A list of the order items for an order</returns>
        public async Task<IActionResult> Details(int id)
        {
            try {
                var orderItems = await _repository.listOrderItemsAsync();
                orderItems = orderItems.Where(oi => oi.Order.OrderId == id);
                var orderItemView = new OrderViewModel {
                    OrderItems = orderItems
                };
                return View(orderItemView);
            } catch (Exception exception) {
                _logger.LogCritical(exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Submits an order to the system
        /// </summary>
        /// <returns>A list of the order items belonging to the submitted order(s)</returns>
        public async Task<IActionResult> Submit()
        {
            try {       
                var sessionValue = HttpContext.Session.GetInt32("CustomerId");
                if (sessionValue == null) {
                    ModelState.AddModelError(string.Empty, "You need to be logged in to do that");
                    return RedirectToAction("Index");
                }
                int customerId = Convert.ToInt32(sessionValue);

                // get all the orders for the customer that has no order date
                var orderItems = await _repository.listOrderItemsAsync();
                orderItems = orderItems.Where(o => o.Order.Customer.CustomerId == customerId && o.Order.OrderDate == null);
                float total = orderItems.Sum(oi => oi.Product.ProductPrice * oi.Quantity);
                if (orderItems.Count() == 0) {
                    ModelState.AddModelError(string.Empty, "Your cart is empty");
                    return RedirectToAction("Index");
                }

                List<OrderItemViewModel> orderItemViews = new List<OrderItemViewModel>();
                
                // for each order item in each order, update the inventory for that item at that location   
                foreach (var orderItem in orderItems) {
                    var order = orderItem.Order;
                    var update = await _repository.submitOrderAsync(order);
                    _logger.LogInformation("Order {0} placed for customer {1} at {2} for a total of ${3}", update.OrderId, update.Customer.Username, update.OrderDate, update.Total);
                    orderItemViews.Add(new OrderItemViewModel {
                        Product = orderItem.Product,
                        Quantity = orderItem.Quantity
                    });
                }
                
                // print the order items to the screen
                ViewData["Total"] = total;
                return View(orderItemViews);
            } catch (Exception exception) {
                _logger.LogCritical(exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        // /// <summary>
        // /// 
        // /// </summary>
        // /// <returns></returns>
        // public IActionResult Create()
        // {
        //     return View();
        // }

        /// <summary>
        /// Adds an order item to a non-submitted order (Adds an item to cart)
        /// </summary>
        /// <param name="orderItemView">The order item to add to the order</param>
        /// <returns>A redirect to a view of the updated cart</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItemViewModel orderItemView)
        {
            try {
                int locationId = orderItemView.Location.LocationId;
                var sessionValue = HttpContext.Session.GetInt32("CustomerId");
                if (ModelState.IsValid && sessionValue != null)
                {
                    int customerId = Convert.ToInt32(sessionValue);

                    // get the order with no order date for this location
                    await _repository.createOrderAsync(customerId, locationId);
                    var orders = await _repository.listOrdersAsync();
                    var order = orders.Where(o => o.Customer.CustomerId == customerId && o.Location.LocationId == locationId &&  o.OrderDate == null)
                    .FirstOrDefault();

                    // Make a new order item for this product
                    BusinessOrderItem orderItem = new BusinessOrderItem {
                        OrderItemId = -1,
                        Quantity = orderItemView.Quantity,
                        Order = order,
                        Product = orderItemView.Product
                    };         

                    // if the order already has the incoming product id...
                    var existingOrderItems = await _repository.listOrderItemsAsync();
                    var existingOrderItem = existingOrderItems.Where(oi => oi.Order.OrderId == order.OrderId && oi.Order.Location.LocationId == locationId && oi.Product.ProductId == orderItemView.Product.ProductId).FirstOrDefault();

                    if (existingOrderItem != null) {
                        // orderItem.Quantity += existingOrderItem.Quantity;
                        orderItem.OrderItemId = existingOrderItem.OrderItemId;
                    }

                    // update the order in the system
                    order.Total += orderItemView.Quantity * orderItemView.Product.ProductPrice;
                    var inventory = await _repository.updateOrderAsync(order, orderItem);
                    if (inventory == null) {
                        ModelState.AddModelError("Quantity", "Please input valid quantity.");
                    } else {
                        _logger.LogInformation("{0} added {1} x {2} to Order #{3}", orderItem.Order.Customer.Username, orderItem.Quantity, orderItem.Product.ProductName, orderItem.Order.OrderId);
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View();
                // return View(orderItem);
            } catch (Exception exception) {
                _logger.LogCritical(exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Shows all of the orders for a certain customer (optional) and location (optional)
        /// </summary>
        /// <param name="id">Optional customer id used in order search</param>
        /// <param name="locationName">Optional location name used in order search</param>
        /// <returns></returns>
        public async Task<IActionResult> List(int? id, string locationName) {
            try {
                var sessionValue = HttpContext.Session.GetInt32("CustomerId");
                var username = HttpContext.Session.GetString("Username");
                if (sessionValue != null) {
                    int customerId = Convert.ToInt32(sessionValue);
                    if (id != null) {
                        customerId = id.Value;
                    }
                    var orders = await _repository.listOrdersAsync();
                    orders = orders.Where(o => o.OrderDate != null);
                    if (username != "admin" || id != null) {
                        orders = orders.Where(o => o.Customer.CustomerId == customerId);
                    }
                    if (!String.IsNullOrWhiteSpace(locationName)) {
                        orders = orders.Where(o => o.Location.LocationName == locationName);
                    }

                    var orderView = new OrderViewModel {
                        Orders = orders
                    };
                    ViewData["Locations"] = await _repository.listLocationsAsync();
                    return View(orderView);
                }
                return RedirectToAction("Index", "Home");
            } catch (Exception exception) {
                _logger.LogCritical(exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
