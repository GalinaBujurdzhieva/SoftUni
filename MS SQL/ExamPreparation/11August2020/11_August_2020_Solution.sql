--01.DDL
CREATE DATABASE Bakery
GO

USE Bakery

CREATE TABLE Countries
(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Customers
(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(25) NOT NULL,
LastName NVARCHAR(25) NOT NULL,
Gender CHAR(1) NOT NULL,
CHECK(Gender = 'M' OR Gender = 'F'),
Age INT NOT NULL,
PhoneNumber VARCHAR(10) NOT NULL,
CHECK(LEN(PhoneNumber) = 10),
CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
)

CREATE TABLE Products
(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(25) UNIQUE NOT NULL,
[Description] NVARCHAR(250),
Recipe NVARCHAR(MAX) NOT NULL,
Price DECIMAL(38,2) NOT NULL,
CHECK(Price > 0)
)

CREATE TABLE Feedbacks
(
Id INT PRIMARY KEY IDENTITY,
[Description] NVARCHAR(255),
Rate DECIMAL (4,2) NOT NULL,
CHECK(Rate BETWEEN 0 AND 10),
ProductId INT FOREIGN KEY REFERENCES Products(Id) NOT NULL,
CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL
)

CREATE TABLE Distributors
(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(25) UNIQUE NOT NULL,
AddressText NVARCHAR(30) NOT NULL,
Summary NVARCHAR(200) NOT NULL,
CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
)

CREATE TABLE Ingredients
(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30) NOT NULL,
[Description] NVARCHAR(200),
OriginCountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL,
DistributorId INT FOREIGN KEY REFERENCES Distributors(Id) NOT NULL
)

CREATE TABLE ProductsIngredients
(
ProductId INT FOREIGN KEY REFERENCES Products(Id) NOT NULL,
IngredientId INT FOREIGN KEY REFERENCES Ingredients(Id) NOT NULL,
PRIMARY KEY (ProductId, IngredientId)
)

--DML
--02.Insert
INSERT INTO Distributors([Name], CountryId, AddressText, Summary) VALUES
('Deloitte & Touche', 2, '6 Arch St #9757', 'Customizable neutral traveling'),
('Congress Title', 13, '58 Hancock St', 'Customer loyalty'),
('Kitchen People', 1, '3 E 31st St #77', 'Triple-buffered stable delivery'),
('General Color Co Inc', 21, '6185 Bohn St #72', 'Focus group'),
('Beck Corporation', 23, '21 E 64th Ave', 'Quality-focused 4th generation hardware')

INSERT INTO Customers(FirstName, LastName, Age, Gender, PhoneNumber, CountryId) VALUES
('Francoise', 'Rautenstrauch', 15, 'M', '0195698399', 5),
('Kendra', 'Loud', 22, 'F', '0063631526', 11),
('Lourdes', 'Bauswell', 50, 'M', '0139037043', 8),
('Hannah', 'Edmison', 18, 'F', '0043343686', 1),
('Tom', 'Loeza', 31, 'M', '0144876096', 23),
('Queenie', 'Kramarczyk', 30, 'F', '0064215793', 29),
('Hiu', 'Portaro', 25, 'M', '0068277755', 16),
('Josefa', 'Opitz', 43, 'F', '0197887645', 17)

--03.Update
SELECT *
FROM Ingredients
WHERE OriginCountryId = 8

UPDATE Ingredients
SET DistributorId = 35
WHERE [Name] IN ('Bay Leaf', 'Paprika', 'Poppy')

UPDATE Ingredients
SET OriginCountryId = 14
WHERE OriginCountryId = 8

--04.Delete
DELETE FROM Feedbacks
WHERE CustomerId = 14 OR ProductId = 5

--Querying 
--05.Products by Price
SELECT [Name], Price, [Description]
FROM Products
ORDER BY Price DESC, [Name]

--06.Negative Feedback
SELECT f.ProductId, f.Rate, f.Description, c.Id AS CustomerId, c.Age, c.Gender 
FROM Feedbacks AS f
LEFT JOIN Customers AS c ON f.CustomerId = c.Id
WHERE f.Rate < 5.0
ORDER BY f.ProductId DESC, f.Rate

--07.Customers without Feedback
SELECT CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName, c.PhoneNumber, c.Gender
FROM Customers AS c
LEFT JOIN Feedbacks AS f ON f.CustomerId = c.Id
WHERE f.CustomerId IS NULL
ORDER BY c.Id

--08.Customers by Criteria
SELECT cust.FirstName, cust.Age, cust.PhoneNumber
FROM Customers AS cust
LEFT JOIN Countries AS coun ON cust.CountryId = coun.Id
WHERE (cust.Age >= 21 AND cust.FirstName LIKE '%an%') OR (cust.PhoneNumber LIKE '%38' AND coun.[Name] != 'Greece')
ORDER BY cust.FirstName, cust.Age DESC

--09.Middle Range Distributors
SELECT d.[Name] AS DistributorName, i.[Name] AS IngredientName, p.[Name] AS ProductName, AVG(f.Rate) AS AverageRate
FROM Ingredients AS i
JOIN ProductsIngredients AS prin ON i.Id = prin.IngredientId
JOIN Products AS p ON prin.ProductId = p.Id
JOIN Distributors AS d ON i.DistributorId = d.Id
JOIN Feedbacks AS f ON f.ProductId = p.Id
GROUP BY d.[Name], p.[Name], i.[Name]
HAVING AVG(f.Rate) BETWEEN 5 AND 8
ORDER BY d.[Name], i.[Name], p.[Name]

--10.Country Representative
SELECT SubQuery.CountryName, SubQuery.DisributorName
FROM (SELECT 
     c.[Name] AS CountryName, 
     d.[Name] AS DisributorName,
     DENSE_RANK() OVER (PARTITION BY c.[Name] ORDER BY COUNT(i.Id) DESC) AS Ranking
     FROM Countries AS c
     LEFT JOIN Distributors AS d ON c.Id = d.CountryId
     LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId
     GROUP BY c.[Name], d.[Name]) AS SubQuery
WHERE SubQuery.Ranking = 1
ORDER BY SubQuery.CountryName, SubQuery.DisributorName
GO

--Programmability
--11.Customers with Countries
CREATE VIEW v_UserWithCountries 
AS
SELECT CONCAT(cust.FirstName, ' ', cust.LastName) AS CustomerName, cust.Age, cust.Gender, coun.[Name] AS CountryName
FROM Customers AS cust
JOIN Countries AS coun ON cust.CountryId = coun.Id
GO

SELECT TOP 5 *
  FROM v_UserWithCountries
 ORDER BY Age
 GO

 --12.Delete Products
 DELETE FROM Products WHERE Id = 7
 GO

 CREATE TRIGGER tr_InsteadOfDelete
 ON Products INSTEAD OF DELETE
 AS
       DELETE FROM ProductsIngredients
	   WHERE ProductId IN
	                      (SELECT p.Id
						  FROM Products AS p
						  JOIN deleted AS del ON p.Id = del.Id)

       DELETE FROM Feedbacks
	   WHERE ProductId IN
	                      (SELECT p.Id
						  FROM Products AS p
						  JOIN deleted AS del ON p.Id = del.Id)

	   DELETE FROM Products
	   WHERE Id IN
	                      (SELECT p.Id
						  FROM Products AS p
						  JOIN deleted AS del ON p.Id = del.Id)
GO

