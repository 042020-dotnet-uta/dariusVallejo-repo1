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

        public async Task<IEnumerable<BusinessCustomer>> listCustomersAsync() {
            var customers = await _context.Customers.ToListAsync();
            return customers.Select(c => new BusinessCustomer {
                    CustomerId = c.CustomerId,
                    Username = c.Username,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
            });
        }

        public async Task<IEnumerable<BusinessLocation>> listLocationsAsync() {
            List<Location> locations = await _context.Locations.ToListAsync();
            return locations.Select(databaseLocation => new BusinessLocation {
                LocationId = databaseLocation.LocationId,
                LocationName = databaseLocation.LocationName
            });
        }

        public async Task<IEnumerable<BusinessProduct>> listInventoriesAsync() {
            var inventories = await _context.Inventories.Include(i => i.Product)
                                                        .Include(i => i.Location)
                                                        .ToListAsync();
            return inventories.Select(item => new BusinessProduct {
                ProductId = item.Product.ProductId,
                ProductName = item.Product.ProductName,
                ProductPrice = item.Product.ProductPrice,
                LocationId = item.Location.LocationId,
                Quantity = item.Quantity,
            });
        }

        public async Task<BusinessProduct> getInventoryAsync(int productId, int locationId) {
            var result = await listInventoriesAsync();
            return result.Select(bp => new BusinessProduct {
                ProductId = bp.ProductId,
                ProductName = bp.ProductName,
                ProductPrice = bp.ProductPrice,
                LocationId = locationId,
                Quantity = bp.Quantity,
            }).Where(p => p.ProductId == productId && p.LocationId == p.LocationId).FirstOrDefault();
        }

        public async Task<IEnumerable<BusinessOrder>> listOrdersAsync() {
            var orders = await _context.Orders
                                    .Include(o => o.Customer)
                                    .Include(o => o.Location)
                                    .Include(o => o.OrderItems)
                                    .ToListAsync();
            return orders.Select(o => new BusinessOrder {
                OrderId = o.OrderId,
                Total = o.Total,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(oi => new BusinessOrderItem {
                    OrderItemId = oi.OrderItemId,
                    Quantity = oi.Quantity
                }).ToHashSet(),
                
                Customer = new BusinessCustomer {
                    CustomerId = o.Customer.CustomerId,
                    Username = o.Customer.Username,
                    FirstName = o.Customer.FirstName,
                    LastName = o.Customer.LastName,
                    Email = o.Customer.Email
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
            if (customer != null && location != null && order == null) {
                order = new Order {
                    OrderDate = null,
                    Customer = customer,
                    Location = location,
                    OrderItems = new List<OrderItem>()
                };
                _context.Add(order);
                await _context.SaveChangesAsync();
                return new BusinessOrder();
            } return null;
        }

        public async Task<BusinessOrder> updateOrderAsync(BusinessOrder businessOrder, BusinessOrderItem businessOrderItem) {
            Order order = await _context.Orders.Include(o => o.OrderItems)
                                                .Where(o => o.OrderId == businessOrder.OrderId)
                                                .FirstOrDefaultAsync();
            Inventory inventory = await _context.Inventories.Include(i => i.Product)
                                                            .Where(i => i.Product.ProductId == businessOrderItem.Product.ProductId)
                                                            .FirstOrDefaultAsync();
            OrderItem orderItem;
            if (businessOrderItem.OrderItemId == -1) {
                orderItem = new OrderItem {
                    Quantity = businessOrderItem.Quantity,
                    Order = order,
                    Product = await _context.Products.Where(p => p.ProductId == businessOrderItem.Product.ProductId).FirstOrDefaultAsync(),
                };
                order.OrderItems.Add(orderItem);
                businessOrder.OrderItems.Add(businessOrderItem);
                _context.Add(orderItem);
            } else {
                orderItem = await _context.OrderItems.Where(oi => oi.OrderItemId == businessOrderItem.OrderItemId).FirstOrDefaultAsync();
                orderItem.Quantity += businessOrderItem.Quantity;
                _context.Update(orderItem);
            }
            inventory.Quantity -= businessOrderItem.Quantity;
            order.Total = businessOrder.Total;
            _context.Update(order);
            _context.Update(inventory);
            await _context.SaveChangesAsync();
            return businessOrder;
        }

        // TODO try catch, logging
        public async Task<BusinessOrder> submitOrderAsync(BusinessOrder businessOrder) {
            Order order = await _context.Orders.Where(o => o.OrderId == businessOrder.OrderId).FirstOrDefaultAsync();
            if (order != null && order.OrderDate == null) {
                order.OrderDate = DateTime.Now.ToString();
                _context.Update(order);
                await _context.SaveChangesAsync();
                return new BusinessOrder();
            } return null;
        }

        public async Task<IEnumerable<BusinessOrderItem>> listOrderItemsAsync() {
            var orderItems = await _context.OrderItems
                                .Include(oi => oi.Order).ThenInclude(o => o.Customer)
                                .Include(oi => oi.Order).ThenInclude(o => o.Location)
                                .Include(oi => oi.Product)
                                .ToListAsync();

            return orderItems.Select(oi => new BusinessOrderItem {
                OrderItemId = oi.OrderItemId,
                Quantity = oi.Quantity,
                
                Order = new BusinessOrder {
                    OrderId = oi.Order.OrderId,
                    OrderDate = oi.Order.OrderDate,

                    Customer = new BusinessCustomer {
                        CustomerId = oi.Order.Customer.CustomerId
                    },

                    Location = new BusinessLocation {
                        LocationId = oi.Order.Location.LocationId,
                        LocationName = oi.Order.Location.LocationName
                    }
                },
                Product = new BusinessProduct {
                    ProductId = oi.Product.ProductId,
                    ProductName = oi.Product.ProductName,
                    ProductPrice = oi.Product.ProductPrice
                }
            });
        }

        public async Task<BusinessOrderItem> updateInventoryAsync(int locationId, BusinessOrderItem businessOrderItem) {
            Inventory inventory = await _context.Inventories.Where(i => i.Location.LocationId == locationId && i.Product.ProductId == businessOrderItem.Product.ProductId).FirstOrDefaultAsync();
            int update = inventory.Quantity - businessOrderItem.Quantity;
            if (update >= 0) {
                inventory.Quantity = update;
                _context.Update(inventory);
                await _context.SaveChangesAsync();
                return businessOrderItem;
            } return null;
        }
    }
}