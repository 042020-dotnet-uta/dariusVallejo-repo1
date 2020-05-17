using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using StoreApp.BusinessLogic;

namespace StoreApp.Data {
    public class Repository : IRepository {
        private readonly BusinessContext _context;

        public Repository(BusinessContext context) {
            _context = context;
        }

        public async Task<BusinessCustomer> createCustomerAsync(BusinessCustomer customer) {
            // TODO Meaningful exceptions that don't break
            if (await _context.Customers.AnyAsync(c => c.Username == customer.Username)) {
                  throw new InvalidOperationException("Already exists.");
            }
            Customer newCustomer = new Customer {
                Username = customer.Username,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Password = customer.Password
            };
            _context.Add(newCustomer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<BusinessCustomer> loginCustomerAsync(BusinessCustomer customer) {
            Customer databaseCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Username == customer.Username && c.Password == customer.Password);
            if (databaseCustomer != null) {
                customer.CustomerId = databaseCustomer.CustomerId;
                return customer;
            } return null;
        }

        public async Task<BusinessCustomer> getCustomerByUsernameAsync(string username) {
            Customer databaseCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Username == username);
            if (databaseCustomer != null) {
                return new BusinessCustomer {
                    CustomerId = databaseCustomer.CustomerId,
                    Username = databaseCustomer.Username,
                    FirstName = databaseCustomer.FirstName,
                    LastName = databaseCustomer.LastName,
                    Email = databaseCustomer.Email
                };
            } return null;
        }

        public async Task<BusinessCustomer> getCustomerByIdAsync(int customerId) {
            Customer databaseCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (databaseCustomer != null) {
                return new BusinessCustomer {
                    CustomerId = databaseCustomer.CustomerId,
                    Username = databaseCustomer.Username,
                    FirstName = databaseCustomer.FirstName,
                    LastName = databaseCustomer.LastName,
                    Email = databaseCustomer.Email
                };
            } return null;
        }

        public async Task<IEnumerable<BusinessLocation>> listLocationsAsync() {
            List<Location> locations = await _context.Locations.ToListAsync();
            return locations.Select(databaseLocation => new BusinessLocation {
                LocationId = databaseLocation.LocationId,
                LocationName = databaseLocation.LocationName
            });
        }

        public async Task<IEnumerable<BusinessProduct>> listProductsAsync(int locationId) {
            var businessProducts = await (from p in _context.Products
                                                    join i in _context.Inventories
                                                    on p.ProductId equals i.Product.ProductId
                                                    select new {
                                                        Product = p,
                                                        Quantity = i.Quantity,
                                                        LocationId = i.Location.LocationId
                                                    }).Where(a => a.LocationId == locationId).ToListAsync();
            return businessProducts.Select(bp => new BusinessProduct {
                ProductId = bp.Product.ProductId,
                ProductName = bp.Product.ProductName,
                ProductPrice = bp.Product.ProductPrice,
                LocationId = locationId,
                Quantity = bp.Quantity,
            });
        }

        public async Task<BusinessProduct> getProductAsync(int productId, int locationId) {
            var result = await listProductsAsync(locationId);
            return result.Select(bp => new BusinessProduct {
                ProductId = bp.ProductId,
                ProductName = bp.ProductName,
                ProductPrice = bp.ProductPrice,
                LocationId = locationId,
                Quantity = bp.Quantity,
            }).Where(p => p.ProductId == productId && p.LocationId == p.LocationId).FirstOrDefault();
        }

        public async Task<IEnumerable<BusinessOrder>> listOrdersByCustomerAsync(int customerId) {
            var orders = await _context.Orders.Include(o => o.Location).Include(o => o.OrderItems).Where(o => o.Customer.CustomerId == customerId).ToListAsync();
            return orders.Select(o => new BusinessOrder {
                OrderId = o.OrderId,
                // Total = 0,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(oi => new BusinessOrderItem {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity
                }).ToHashSet(),
                
                // Customer = null;
                Location = new BusinessLocation {
                    LocationId = o.Location.LocationId
                }
            });
        }

        public async Task<IEnumerable<BusinessOrder>> listOrdersAsync() {
            var orders = await _context.Orders.Include(o => o.Customer).Include(o => o.Location).Include(o => o.OrderItems).ToListAsync();
            return orders.Select(o => new BusinessOrder {
                OrderId = o.OrderId,
                // Total = 0,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(oi => new BusinessOrderItem {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity
                }).ToHashSet(),
                
                Customer = new BusinessCustomer {
                    CustomerId = o.Customer.CustomerId,
                    Username = o.Customer.Username
                },
                Location = new BusinessLocation {
                    LocationId = o.Location.LocationId,
                    LocationName = o.Location.LocationName
                }
            });
        }

        public async Task<BusinessOrder> createOrderAsync(int customerId, int locationId) {
            Customer customer = await _context.Customers.Where(c => c.CustomerId == customerId).FirstOrDefaultAsync();
            Location location = await _context.Locations.Where(l => l.LocationId == locationId).FirstOrDefaultAsync();
            Order order = await _context.Orders.Where(o => o.Customer.CustomerId == customerId && o.Location.LocationId == locationId && o.OrderDate == null).FirstOrDefaultAsync();
            if (order == null) {
                order = new Order {
                OrderDate = null,
                Customer = customer,
                Location = location,
                OrderItems = new List<OrderItem>()
            };
                _context.Add(order);
                await _context.SaveChangesAsync();
            } return new BusinessOrder();
            
        }

        public async Task<BusinessOrder> updateOrderAsync(BusinessOrder businessOrder, BusinessOrderItem businessOrderItem) {
            Order order = await _context.Orders.Include(o => o.OrderItems).Where(o => o.OrderId == businessOrder.OrderId).FirstOrDefaultAsync();
            OrderItem orderItem = new OrderItem {
                Quantity = businessOrderItem.Quantity,
                Order = order,
                Product = await _context.Products.Where(p => p.ProductId == businessOrderItem.Product.ProductId).FirstOrDefaultAsync(),
            };
            order.OrderItems.Add(orderItem);
            businessOrder.OrderItems.Add(businessOrderItem);
            _context.Add(orderItem);
            await _context.SaveChangesAsync();
            return businessOrder;
        }

        public async Task<BusinessOrder> submitOrderAsync(BusinessOrder businessOrder) {
            // Order order = await _context.Orders.Include(o => o.OrderItems).Where(o => o.OrderId == businessOrder.OrderId).FirstOrDefaultAsync();
            Order order = await _context.Orders.Where(o => o.OrderId == businessOrder.OrderId).FirstOrDefaultAsync();
            order.OrderDate = DateTime.Now.ToString();
            _context.Update(order);
            await _context.SaveChangesAsync();
            return businessOrder;
        }

        public async Task<IEnumerable<BusinessOrderItem>> listOrderItemsAsync(int customerId, int orderId) {
            Order order;
            if (orderId == -1) {
                order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(o => o.Product).Where(o => o.Customer.CustomerId == customerId && o.OrderDate == null).FirstOrDefaultAsync();
            } else {
                order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(o=> o.Product).Where(o => o.OrderId == orderId).FirstOrDefaultAsync();
            }
            try {
                var orderItems = order.OrderItems;
                return orderItems.Select(oi => new BusinessOrderItem {
                OrderItemId = oi.OrderItemId,
                Quantity = oi.Quantity,
                
                Order = new BusinessOrder {
                    OrderId = order.OrderId
                },
                Product = new BusinessProduct {
                    ProductId = oi.Product.ProductId,
                    ProductName = oi.Product.ProductName,
                    ProductPrice = oi.Product.ProductPrice
                }
            });
            } catch (NullReferenceException exception) {
                // TODO LOG EXCEPTION
                return null;
            }
        }

        public async Task<BusinessOrderItem> updateInventoryAsync(int locationId, BusinessOrderItem businessOrderItem) {
            Inventory inventory = await _context.Inventories.Where(i => i.Location.LocationId == locationId && i.Product.ProductId == businessOrderItem.Product.ProductId).FirstOrDefaultAsync();
            inventory.Quantity = inventory.Quantity - businessOrderItem.Quantity;
            _context.Update(inventory);
            await _context.SaveChangesAsync();
            return businessOrderItem;
        }
    }
}