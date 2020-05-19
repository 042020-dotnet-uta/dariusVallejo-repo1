delete from Inventories;
delete from Locations;
delete from Products;
delete from Customers;
delete from OrderItems;
delete from Orders;

DBCC CHECKIDENT (Locations, RESEED, 0);
DBCC CHECKIDENT (Products, RESEED, 0);
DBCC CHECKIDENT (Inventories, RESEED, 0);
DBCC CHECKIDENT (Customers, RESEED, 0);

Insert into Locations
VALUES 
('Sears'),
('Walmart'),
('Target');

Insert into Products (
ProductName,
ProductPrice)
VALUES
('Pliers', 12.99),
('Glue', 2.75),
('Dishwasher', 299.99),
('Maple Syrup', 1.28),
('Carburetor', 342.99),
('Broom', 8.56),
('Eggs', 1.99),
('Flashlight', 3.00),
('Zip Ties', 0.38);

Insert into Inventories (
Quantity,
LocationId,
ProductId)
VALUES
(RAND()*(9)+1, RAND()*(3)+1, 1),
(RAND()*(9)+1, RAND()*(3)+1, 2),
(RAND()*(9)+1, RAND()*(3)+1, 3),
(RAND()*(9)+1, RAND()*(3)+1, 4),
(RAND()*(9)+1, RAND()*(3)+1, 5),
(RAND()*(9)+1, RAND()*(3)+1, 6),
(RAND()*(9)+1, RAND()*(3)+1, 7),
(RAND()*(9)+1, RAND()*(3)+1, 8),
(RAND()*(9)+1, RAND()*(3)+1, 9);