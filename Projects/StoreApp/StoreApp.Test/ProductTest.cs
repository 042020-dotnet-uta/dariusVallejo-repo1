using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using StoreApp.BusinessLogic;
using StoreApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Test
{
    public class ProductTest
    {        
        [Fact]
        public async Task TestListInventories() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_inventory")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                var productA = new Product { ProductId = 1 };
                var productB = new Product { ProductId = 2 };
                var locationA = new Location { LocationId = 1 };
                var inventoryA = new Inventory {
                    Product = productA,
                    Location = locationA
                };
                var inventoryB = new Inventory {
                    Product = productB,
                    Location = locationA
                };
                bc.Add(productA);
                bc.Add(productB);
                bc.Add(locationA);
                bc.Add(inventoryA);
                bc.Add(inventoryB);

                await bc.SaveChangesAsync();

                var productsA = await repository.listInventoriesAsync();
                var productsB = await repository.listInventoriesAsync();
                productsA = productsA.Where(p => p.Location.LocationId == 1);
                productsB = productsB.Where(p => p.Location.LocationId == 2);

                Assert.Equal(2, productsA.Count());
                Assert.Empty(productsB);
            }
        }

        [Fact]
        public async Task TestListInventoriesEmpty() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_inventory_empty")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                var productA = new Product { ProductId = 1 };
                var productB = new Product { ProductId = 2 };
                var locationA = new Location { LocationId = 1 };
                var inventoryA = new Inventory {
                    Product = productA,
                    Location = locationA
                };
                var inventoryB = new Inventory {
                    Product = productB,
                    Location = locationA
                };
                bc.Add(productA);
                bc.Add(productB);
                bc.Add(locationA);
                bc.Add(inventoryA);
                bc.Add(inventoryB);

                await bc.SaveChangesAsync();

                var productsB = await repository.listInventoriesAsync();
                productsB = productsB.Where(p => p.Location.LocationId == 2);

                Assert.Empty(productsB);
            }
        }

        [Fact]
        public async Task TestUpdateInventory() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_inventory_update")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                int productId = 1;
                int locationId = 1;
                var inventory = new Inventory {
                    Product = new Product { ProductId = productId },
                    Location = new Location{ LocationId = locationId },
                    Quantity = 3
                };
                bc.Add(inventory);
                await bc.SaveChangesAsync();
            
                var orderItem = new BusinessOrderItem {
                    Product = new BusinessProduct { ProductId = productId },
                    Quantity = 1
                };
                var result = await repository.updateInventoryAsync(locationId, orderItem);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task TestUpdateInventoryFail() {
            var options = new DbContextOptionsBuilder<BusinessContext>()
                            .UseInMemoryDatabase(databaseName: "test_inventory_update_fail")
                            .Options;

            using (var bc = new BusinessContext(options)) {
                var repository = new Repository(bc);

                int productId = 1;
                int locationId = 1;
                var inventory = new Inventory {
                    Product = new Product { ProductId = productId },
                    Location = new Location{ LocationId = locationId },
                    Quantity = 3
                };
                bc.Add(inventory);
                await bc.SaveChangesAsync();
            
                var orderItem = new BusinessOrderItem {
                    Product = new BusinessProduct { ProductId = productId },
                    Quantity = 4
                };
                var result = await repository.updateInventoryAsync(locationId, orderItem);
                Assert.Null(result);
            }
        }
    }
}