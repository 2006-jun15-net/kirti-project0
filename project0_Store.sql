CREATE DATABASE Project0;

GO
CREATE SCHEMA Store
GO

DROP TABLE Store.Location;
DROP TABLE Store.Customer;
DROP TABLE Store.Orders;
DROP TABLE Store.OrderLine;
DROP TABLE Store.Product;
DROP TABLE Store.Stock;


CREATE TABLE Store.Location (
    LocationId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LocationName NVARCHAR(255) NOT NULL
);

CREATE TABLE Store.Customer (
    CustomerId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(26) NOT NULL,
    LastName NVARCHAR(26) NOT NULL,
);

CREATE TABLE Store.Product (
    ProductId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL UNIQUE,
    Price DECIMAL (9,2) NOT NULL CHECK(Price > 0),
);

CREATE TABLE Store.Orders (
    OrderId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LocationId INT NOT NULL,
    CustomerId INT NOT NULL,
    Price DECIMAL (9,2) NOT NULL CHECK (Price > 0),
    OrderDate DATETIME NOT NULL,
    CONSTRAINT FK_Orders_LocationId_Location FOREIGN KEY (LocationId) 
        REFERENCES Store.Location (LocationId)
            ON DELETE CASCADE,
    CONSTRAINT FK_Orders_CustomerId_Customer FOREIGN KEY (CustomerId) 
        REFERENCES Store.Customer (CustomerId)
            ON DELETE CASCADE
);

CREATE TABLE Store.OrderLine (
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK(Quantity > 0),
    CONSTRAINT PK_OrderId_ProductId PRIMARY KEY(OrderId, ProductId),
    CONSTRAINT FK_OrderLine_OrderId_Orders FOREIGN KEY (OrderId) 
        REFERENCES Store.Orders (OrderId)
            ON DELETE CASCADE,
    CONSTRAINT FK_OrderLine_ProductId_Product FOREIGN KEY (ProductId) 
        REFERENCES Store.Product (ProductId)
            ON DELETE CASCADE
);

CREATE TABLE Store.Stock (
    StockId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductId INT NOT NULL,
    LocationID INT NOT NULL,
    Inventory INT NOT NULL CHECK(Inventory >= 0),
    CONSTRAINT FK_ProductId_Product FOREIGN KEY (ProductId) 
        REFERENCES Store.Product (ProductId)
            ON DELETE CASCADE,
    CONSTRAINT FK_LocationId_Location FOREIGN KEY (LocationID) 
        REFERENCES Store.Location (LocationID)
            ON DELETE CASCADE
);

SELECT * FROM Store.Orders;