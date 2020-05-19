using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using StoreApp.BusinessLogic;
using StoreApp.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StoreApp.Test
{
    // Task<BusinessOrder> createOrderAsync(int customerId, int locationId);
    // Task<BusinessOrder> updateOrderAsync(BusinessOrder order, BusinessOrderItem orderItem);
    // Task<BusinessOrder> submitOrderAsync(BusinessOrder order);
    // Task<IEnumerable<BusinessOrder>> listOrdersAsync();
    public class OrderTest
    {   
        [Fact]
        public async Task TestCreateOrder() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_order")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                int customerId = 1;
                int locationId = 1;
                var customer = new Customer { CustomerId = customerId };
                var location = new Location { LocationId = locationId };

                bc.Add(customer);
                bc.Add(location);
                await bc.SaveChangesAsync();

                var order = await repository.createOrderAsync(customerId, locationId);
                Assert.NotNull(order);
            }
        }

        [Fact]
        public async Task TestCreateOrderFail() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_order_fail")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);
                var order = await repository.createOrderAsync(234, 234);
                Assert.Null(order);
            }
        }

        [Fact]
        public async Task TestSubmitOrder() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_order_submit")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                int customerId = 1;
                int locationId = 1;
                int orderId = 1;
                var customer = new Customer { CustomerId = customerId };
                var location = new Location { LocationId = locationId };
                var order = new Order {
                    OrderId = orderId,
                    Customer = customer,
                    Location = location
                };
                var orderItem = new OrderItem { Order = order };
                

                bc.Add(customer);
                bc.Add(location);
                bc.Add(order);
                bc.Add(orderItem);
                await bc.SaveChangesAsync();

                var submit = await repository.submitOrderAsync(new BusinessOrder { OrderId = orderId });
                Assert.NotNull(submit);
            }
        }

        [Fact]
        public async Task TestSubmitOrderFail() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_order_submit_fail")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                int orderId = 1;
                var order = new Order {
                    OrderId = orderId,
                    OrderDate = "not-null"
                };                

                bc.Add(order);
                await bc.SaveChangesAsync();
                
                var submitA = await repository.submitOrderAsync(new BusinessOrder { OrderId = orderId });
                Assert.Null(submitA);
            }
        }

        [Fact]
        public async Task TestSubmitOrderNull() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_order_submit_failb")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                var submitB = await repository.submitOrderAsync(new BusinessOrder { OrderId = 2 });
                Assert.Null(submitB);
            }
        }

        [Fact]
        public async Task TestListOrderItems() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_orderitem_list")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                var orderItemA = new OrderItem {
                    Order = new Order {
                        Customer = new Customer(),
                        Location = new Location()
                    },
                    Product = new Product(),
                };
                var orderItemB = new OrderItem {
                    Order = new Order {
                        Customer = new Customer(),
                        Location = new Location()
                    },
                    Product = new Product()
                };
                bc.Add(orderItemA);
                bc.Add(orderItemB);
                await bc.SaveChangesAsync();
                
                var orderItems = await repository.listOrderItemsAsync();
                Assert.Equal(2, orderItems.Count());
            }
        }

        [Fact]
        public async Task TestListOrderItemsEmpty() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_orderitem_empty")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);
                
                var orderItems = await repository.listOrderItemsAsync();
                Assert.Empty(orderItems);
            }
        }
    }
}