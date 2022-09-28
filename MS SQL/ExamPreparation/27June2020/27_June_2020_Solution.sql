--01.DDL

CREATE DATABASE WMS
GO

USE WMS

CREATE TABLE Clients
(
ClientId INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
Phone VARCHAR(12) NOT NULL,
CHECK(LEN(Phone) = 12)
)

CREATE TABLE Mechanics
(
MechanicId INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
[Address] VARCHAR(255) NOT NULL
)

CREATE TABLE Models
(
ModelId INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Jobs
(
JobId INT PRIMARY KEY IDENTITY,
ModelId INT FOREIGN KEY REFERENCES Models(ModelId) NOT NULL,
[Status] VARCHAR(11) NOT NULL DEFAULT 'Pending',
CHECK([Status] IN ('Pending', 'In Progress', 'Finished')),
ClientId INT FOREIGN KEY REFERENCES Clients(ClientId) NOT NULL,
MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId),
IssueDate DATE NOT NULL,
FinishDate DATE
)

CREATE TABLE Orders
(
OrderId INT PRIMARY KEY IDENTITY,
JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
IssueDate DATE,
Delivered BIT NOT NULL
)

CREATE TABLE Vendors
(
VendorId INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Parts
(
PartId INT PRIMARY KEY IDENTITY,
SerialNumber VARCHAR(50) UNIQUE NOT NULL,
[Description] VARCHAR(255),
Price MONEY NOT NULL,
CHECK(Price > 0 AND Price <= 9999.99),
VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId) NOT NULL,
StockQty INT NOT NULL DEFAULT 0,
CHECK(StockQty >= 0)
)

CREATE TABLE PartsNeeded
(
JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL
PRIMARY KEY (JobId, PartId),
Quantity INT NOT NULL DEFAULT 1,
CHECK(Quantity > 0)
)

CREATE TABLE OrderParts
(
OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) NOT NULL,
PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
PRIMARY KEY (OrderId, PartId),
Quantity INT NOT NULL DEFAULT 1,
CHECK(Quantity > 0)
)

--DML
--02.Insert
INSERT INTO Clients VALUES
('Teri', 'Ennaco', '570-889-5187'),
('Merlyn', 'Lawler', '201-588-7810'),
('Georgene', 'Montezuma', '925-615-5185'),
('Jettie', 'Mconnell', '908-802-3564'),
('Lemuel', 'Latzke', '631-748-6479'),
('Melodie', 'Knipp', '805-690-1682'),
('Candida', 'Corbley', '908-275-8357')

INSERT INTO Parts(SerialNumber,	[Description], Price, VendorId) VALUES
('WP8182119', 'Door Boot Seal', 117.86, 2),
('W10780048', 'Suspension Rod', 42.81, 1),
('W10841140', 'Silicone Adhesive', 6.77, 4),
('WPY055980', 'High Temperature Adhesive', 13.94, 3)

--03.Update
UPDATE Jobs
SET MechanicId = (SELECT MechanicId
                  FROM Mechanics
                  WHERE FirstName = 'Ryan' AND LastName = 'Harnos')
WHERE [Status] = 'Pending'

UPDATE Jobs
SET [Status] = 'In Progress'
WHERE MechanicId = (SELECT MechanicId
                    FROM Mechanics
                    WHERE FirstName = 'Ryan' AND LastName = 'Harnos')
AND [Status] = 'Pending'

--04.Delete
DELETE FROM OrderParts
WHERE OrderId = 19

DELETE FROM Orders
WHERE OrderId = 19

--Querying
--05.Mechanic Assignments
SELECT m.FirstName + ' ' + m.LastName AS Mechanic, j.[Status], j.IssueDate
FROM Mechanics AS m
JOIN Jobs AS j ON m.MechanicId = j.MechanicId
ORDER BY m.MechanicId, j.IssueDate, j.JobId

--06.Current Clients
SELECT c.FirstName + ' ' + c.LastName AS Client, DATEDIFF(DAY, j.IssueDate, '2017-04-24') AS [Days going], j.[Status]
FROM Clients AS c
JOIN Jobs AS j ON c.ClientId = j.ClientId
WHERE j.[Status] != 'Finished'
ORDER BY [Days going] DESC, c.ClientId

--07.Mechanic Performance
SELECT m.FirstName + ' ' + m.LastName AS Mechanic, AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS [Average Days]
FROM Mechanics AS m
JOIN Jobs AS j ON m.MechanicId = j.MechanicId
GROUP BY m.FirstName + ' ' + m.LastName, m.MechanicId
ORDER BY m.MechanicId

--08.Available Mechanics
SELECT FirstName + ' ' + LastName AS 'Available'
FROM Mechanics
WHERE MechanicId NOT IN (SELECT m.MechanicId
                         FROM Mechanics AS m
                         LEFT JOIN Jobs AS j ON m.MechanicId = j.MechanicId
                         GROUP BY m.FirstName + ' ' + m.LastName, m.MechanicId, j.[Status]
                         HAVING j.[Status] != 'Finished')
ORDER BY MechanicId

SELECT CONCAT(FirstName, ' ' , LastName) AS Available
FROM Mechanics AS m
LEFT JOIN Jobs AS j ON j.MechanicId = m.MechanicId
WHERE j.JobId IS NULL OR 'Finished' = ALL(SELECT j.[Status]
								          FROM Jobs AS j
								          WHERE j.MechanicId = m.MechanicId)	  
GROUP BY CONCAT(FirstName,' ',LastName), m.MechanicId
ORDER BY m.MechanicId

--09.Past Expenses
SELECT j.JobId, ISNULL(SUM(p.Price * op.Quantity), 0) AS [Total]
FROM Jobs AS j
LEFT JOIN Orders AS o ON j.JobId = o.JobId
LEFT JOIN OrderParts AS op ON o.OrderId = op.OrderId
LEFT JOIN Parts AS p ON op.PartId = p.PartId
WHERE j.[Status] = 'Finished'
GROUP BY j.JobId
ORDER BY [Total] DESC, j.JobId

--10.Missing Parts
SELECT
       p.PartId, 
	   p.[Description], 
	   pn.Quantity AS [Required], 
	   p.StockQty AS [In Stock],
	   ISNULL(op.Quantity, 0) AS [Ordered]
FROM Jobs AS j
LEFT JOIN PartsNeeded AS pn ON j.JobId = pn.JobId
LEFT JOIN Parts AS p ON pn.PartId = p.PartId
LEFT JOIN Orders AS o ON o.JobId = j.JobId
LEFT JOIN OrderParts AS op ON o.OrderId = op.OrderId
WHERE (j.[Status] != 'Finished') AND (o.Delivered != 1 OR o.Delivered IS NULL) AND (ISNULL(op.Quantity, 0) + p.StockQty < pn.Quantity)
ORDER BY p.PartId
GO

--Programmability
--11.Place Order
CREATE PROC usp_PlaceOrder(@jobId INT, @serialNumber VARCHAR(50), @quantity INT)
AS
BEGIN
   IF(@quantity <= 0)
      BEGIN;
	     THROW 50012, 'Part quantity must be more than zero!', 1
      END
   
   DECLARE @myJobStatus VARCHAR(11) = (SELECT [Status]
                                       FROM Jobs
                                       WHERE JobId = @jobId)
   IF(@myJobStatus = 'Finished')
      BEGIN;
	      THROW 50011, 'This job is not active!', 1
      END

   DECLARE @myJobId INT = (SELECT JobId
                           FROM Jobs
                           WHERE JobId = @jobId)
   IF(@myJobId IS NULL)
      BEGIN;
	     THROW 50013, 'Job not found!', 1
      END

   DECLARE @myPartId INT = (SELECT PartId
                            FROM Parts
                            WHERE SerialNumber = @serialNumber)
   
   IF(@myPartId IS NULL)
      BEGIN;
	     THROW 50014, 'Part not found!', 1
      END

   DECLARE @myOrderId INT = (SELECT OrderId
                             FROM Orders
						     WHERE JobId = @jobId AND IssueDate IS NULL)

   IF(@myOrderId IS NULL)
      BEGIN
	     INSERT INTO Orders VALUES
	     (@jobId, NULL, 0)
	  END;
   SET @myOrderId = (SELECT OrderId
                     FROM Orders
				     WHERE JobId = @jobId AND IssueDate IS NULL)

	DECLARE @myOrderPartQuantity INT = (SELECT Quantity
	                                    FROM OrderParts
										WHERE OrderId = @myOrderId AND PartId = @myPartId)

    IF(@myOrderPartQuantity IS NULL)
	  BEGIN
	      INSERT INTO OrderParts VALUES
		  (@myOrderId, @myPartId, @quantity)
	  END
    ELSE
	  BEGIN
	      UPDATE OrderParts
		  SET Quantity += @quantity
		  WHERE OrderId = @myOrderId AND PartId = @myPartId
	  END
END
GO

--12.Cost of Order
CREATE FUNCTION udf_GetCost(@jobId INT)
RETURNS DECIMAL(18,2)
   BEGIN
   DECLARE @result DECIMAL(18,2) = (SELECT 
                                    SUM(p.Price * op.Quantity) AS Result
                                    FROM Orders AS o
                                    JOIN OrderParts AS op ON o.OrderId = op.OrderId
                                    JOIN Parts AS p ON p.PartId = op.PartId
                                    WHERE o.JobId = @jobId
									GROUP BY o.JobId)
   RETURN ISNULL(@result, 0)
   END
GO

   SELECT dbo.udf_GetCost(3)
