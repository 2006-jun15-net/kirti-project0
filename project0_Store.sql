CREATE DATABASE Project0;

GO
CREATE SCHEMA Store
GO

-- DROP TABLE Store.Location
CREATE TABLE Store.Location (
    LocationId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LocationName NVARCHAR(255) NOT NULL
);

-- DROP TABLE Store.Customer
CREATE TABLE Store.Customer (
    CustomerId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(26) NOT NULL,
    LastName NVARCHAR(26) NOT NULL,
    UserName NVARCHAR(50) NOT NULL,
    LocationId INT
    CONSTRAINT FK_Customer FOREIGN KEY (LocationId) 
        REFERENCES Store.Location (LocationId)
            ON DELETE SET NULL
);

CREATE TABLE Store.Orders (
    OrderId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LocationId INT NOT NULL,
    CustomerId INT NOT NULL,
    Price DECIMAL (9,2) NOT NULL CHECK (Price > 0),
    OrderDate DATETIME NOT NULL,
    CONSTRAINT FK_LocationId_Location FOREIGN KEY (LocationId) 
        REFERENCES Store.Location (LocationId)
            ON DELETE CASCADE,
    CONSTRAINT FK_CustomerId_Customer FOREIGN KEY (CustomerId) 
        REFERENCES Store.Customer (CustomerId)
            ON DELETE CASCADE
);

CREATE TABLE Store.OrderLine (
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    CONSTRAINT FK_OrderId_Orders FOREIGN KEY (OrderId) 
        REFERENCES Store.Orders (OrderId)
            ON DELETE CASCADE,
    CONSTRAINT FK_ProductId_Product FOREIGN KEY (ProductId) 
        REFERENCES Store.Product (ProductId)
            ON DELETE CASCADE
);

CREATE TABLE Store.Product (
    ProductId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    Price DECIMAL (9,2) NOT NULL CHECK(Price > 0),
);

CREATE TABLE Store.Stock (
    StockId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductId INT NOT NULL,
    LocationID INT NOT NULL,
    Inventory INT NOT NULL CHECK(Inventory > 0),
    CONSTRAINT FK_ProductId2_Product FOREIGN KEY (ProductId) 
        REFERENCES Store.Product (ProductId)
            ON DELETE CASCADE,
    CONSTRAINT FK_LocationId2_Location FOREIGN KEY (LocationID) 
        REFERENCES Store.Location (LocationID)
            ON DELETE CASCADE
);

INSERT INTO Store.Customer(FirstName, LastName) 
    VALUES ('Kirti', 'Patel'), ('Yash', 'Patel'), ('Catherinr', 'McBroom');

DELETE FROM Store.Customer WHERE FirstName = 'Kirti';

INSERT INTO Store.Customer (FirstName, LastName, LocationID) VALUES ('Kirti', 'Patel', 1);

INSERT INTO Store.Location (LocationName) VALUES ('Bothell'), ('Bellevue'), ('Seattle');

INSERT INTO Store.Product (ProductName, Price) 
    VALUES ('Range Rover', '90000'), ('Tesla', '89480'), ('Audi', '39000'), ('Cadillac', '76195'), ('Acura', '92950'), ('Lexus', '93864');

