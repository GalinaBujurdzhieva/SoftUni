--01.Find Names of All Employees by First Name
USE SoftUni

SELECT FirstName, LastName
FROM Employees
WHERE FirstName LIKE 'Sa%'

--02.Find Names of All Employees by Last Name
SELECT FirstName, LastName
FROM Employees
WHERE LastName LIKE '%ei%'

--03.Find First Names of All Employess
SELECT FirstName
FROM Employees
WHERE DepartmentID IN (3, 10) AND DATEPART(YEAR, HireDate) BETWEEN 1995 AND 2005

--04.Find All Employees Except Engineers
SELECT FirstName, LastName
FROM Employees
WHERE JobTitle NOT LIKE '%engineer%'

--05.Find Towns with Name Length
SELECT [Name]
FROM Towns
WHERE LEN([Name]) = 5 OR LEN([Name]) = 6
ORDER BY [Name] 

--06.Find Towns Starting With
SELECT *
FROM Towns
WHERE [Name] LIKE '[MKBE]%'
ORDER BY [Name]

--07.Find Towns Not Starting With
SELECT *
FROM Towns
WHERE [Name] LIKE '[^RBD]%'
ORDER BY [Name]

GO
--08.Create View Employees Hired After
CREATE VIEW [V_EmployeesHiredAfter2000] AS
SELECT FirstName, LastName
FROM Employees
WHERE DATEPART(YEAR, HireDate) > 2000

GO
--09.Length of Last Name
SELECT FirstName, LastName
FROM Employees
WHERE LEN(LastName) = 5

--10.Rank Employees by Salary
SELECT EmployeeID, FirstName, LastName, Salary, DENSE_RANK() OVER (PARTITION BY Salary ORDER BY EmployeeID) AS Rank
FROM Employees
WHERE Salary BETWEEN 10000 AND 50000
ORDER BY Salary DESC

--11.Find All Employees with Rank 2
SELECT * 
FROM (SELECT EmployeeID, FirstName, LastName, Salary, DENSE_RANK() OVER (PARTITION BY Salary ORDER BY EmployeeID) AS [Rank]
FROM Employees
WHERE Salary BETWEEN 10000 AND 50000) AS Result
WHERE Rank = 2
ORDER BY Salary DESC

USE Geography
--12.Countries Holding 'A'
SELECT CountryName AS [Country Name], IsoCode AS [ISO Code]
FROM Countries
WHERE LEN(CountryName) - LEN(REPLACE(CountryName, 'A', '')) >= 3
ORDER BY [ISO Code]

--13.Mix of Peak and River Names
SELECT PeakName, RiverName, LOWER(LEFT(PeakName, LEN(PeakName) - 1) + '' + RiverName) AS Mix
FROM Peaks, Rivers
WHERE RIGHT(PeakName, 1) = LEFT(RiverName, 1)
ORDER BY Mix

--14.Games from 2011 and 2012 year
USE Diablo

SELECT TOP(50) [Name], FORMAT ([Start], 'yyyy-MM-dd') AS [Start]
FROM Games
WHERE DATEPART(YEAR, [Start]) IN (2011, 2012)
ORDER BY [Start], [Name]

--15.User Email Providers
SELECT Username, RIGHT(Email, LEN(Email) - CHARINDEX('@', Email, 1)) AS [Email Provider]
FROM Users
ORDER BY [Email Provider], Username

--16.Get Users with IPAdress Like Pattern
SELECT Username, IpAddress AS [IP Address]
FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username

--17.Show All Games with Duration
SELECT [Name] AS Game,
CASE 
    WHEN DATEPART(HOUR, Start) BETWEEN 0 AND 11 THEN 'Morning'
	WHEN DATEPART(HOUR, Start) BETWEEN 12 AND 17 THEN 'Afternoon'
	ELSE 'Evening'
    END AS [Part of the Day],
CASE 
    WHEN Duration <= 3 THEN 'Extra Short'
	WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
	WHEN Duration > 6 THEN 'Long'
	ELSE 'Extra Long'
	END AS Duration
FROM Games
ORDER BY Game, Duration, [Part of the Day]

--18.Orders Table
USE Orders

SELECT ProductName, OrderDate,
DATEADD(DAY, 3, OrderDate) AS [Pay Due],
DATEADD(MONTH, 1, OrderDate) AS [Deliver Due]
FROM Orders

--19.People Table
USE master

CREATE TABLE People
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
Birthdate DATETIME NOT NULL
)

INSERT INTO People VALUES
('Victor', '2000-12-07 00:00:00.000'),
('Steven', '1992-09-10 00:00:00.000'),
('Stephen', '1910-09-19 00:00:00.000'),
('John', '2010-01-06 00:00:00.000')

SELECT *,
DATEDIFF(MONTH, Birthdate, GETDATE()) / 12 AS [Age in Years],
DATEDIFF(MONTH, Birthdate, GETDATE()) AS [Age in Months],
DATEDIFF(DAY, Birthdate, GETDATE()) AS [Age in Days],
DATEDIFF(MINUTE, Birthdate, GETDATE()) AS [Age in Minutes]
FROM People
