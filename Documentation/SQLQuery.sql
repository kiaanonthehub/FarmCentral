CREATE DATABASE FarmCentral;
USE FarmCentral;

CREATE TABLE Users
(
userID INT PRIMARY KEY IDENTITY NOT NULL,
name VARCHAR(100) NOT NULL,
surname VARCHAR(100) NOT NULL,
email VARCHAR(100) NOT NULL,
password VARCHAR(100) NOT NULL,
role VARCHAR(25) NOT NULL
);

CREATE TABLE Product 
(
productID INT PRIMARY KEY IDENTITY NOT NULL,
product_name VARCHAR(50)
);

CREATE TABLE Product_Type
(
productTypeID INT PRIMARY KEY IDENTITY NOT NULL,
productType VARCHAR(50) NOT NULL,
);

CREATE TABLE Users_Product
(
usersProductID INT PRIMARY KEY IDENTITY NOT NULL,
userID  INT  NOT NULL,
productID INT NOT NULL,
productTypeID  INT NOT NULL,
quantity INT,
product_type VARCHAR(50) NOT NULL,
product_date DATE NOT NULL,
FOREIGN KEY (userID) REFERENCES Users (userID),
FOREIGN KEY (productID) REFERENCES Product (productID),
FOREIGN KEY (productTypeID) REFERENCES Product_Type (productTypeID)
);

insert into Users(name,surname,email,password,role) values('Kiaan','Maharaj','kiaanmaharaj7@gmail.com','$2a$11$IXVv/cxwsI2zbc76PosnMevGv.7y3coqLqf36/WlDziHOXGiL9KNu','Employee')

INSERT INTO Product_Type (productType) VALUES
('Animals or livestock'),
('Plants or fungi'),
('Vegetables'),
('Fruits'),
('Grains'),
('Food for livestock'),
('Fuel'),
('Fiber'),
('Raw Materials');