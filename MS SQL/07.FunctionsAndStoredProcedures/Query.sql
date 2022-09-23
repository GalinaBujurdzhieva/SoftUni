--Queries for SoftUni Database

--01.Employees with Salary Above 35000
USE SoftUni
GO

CREATE PROC usp_GetEmployeesSalaryAbove35000
AS
SELECT FirstName, LastName
FROM Employees
WHERE Salary > 35000
GO

--02.Employees with Salary Above Number
CREATE PROC usp_GetEmployeesSalaryAboveNumber(@EmployeeSalary DECIMAL(18,4))
AS
SELECT FirstName, LastName
FROM Employees
WHERE Salary >= @EmployeeSalary
GO

EXEC usp_GetEmployeesSalaryAboveNumber 35000

--03.Town Names Starting With
GO

CREATE PROC usp_GetTownsStartingWith(@StartingString VARCHAR(MAX))
AS
SELECT [Name] AS Town
FROM Towns
WHERE SUBSTRING([Name], 1, LEN(@StartingString)) = @StartingString

EXEC usp_GetTownsStartingWith 'bE'
GO
--04.Employees from Town

CREATE PROC usp_GetEmployeesFromTown(@TownName VARCHAR(100))
AS
SELECT E.FirstName, E.LastName
FROM Employees AS E
LEFT JOIN Addresses AS A ON E.AddressID = A.AddressID
LEFT JOIN Towns AS T ON A.TownID = T.TownID
WHERE T.Name = @TownName

EXEC usp_GetEmployeesFromTown 'Sofia'
GO

--05.Salary Level Function
CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4)) 
RETURNS VARCHAR(10)
AS
BEGIN
DECLARE @SalaryLevel VARCHAR(10)
IF @salary < 30000 
SET @SalaryLevel = 'Low'
ELSE IF @salary BETWEEN 30000 AND 50000
SET @SalaryLevel = 'Average'
ELSE IF @salary > 50000
SET @SalaryLevel = 'High'
RETURN @SalaryLevel
END
GO 

SELECT Salary, dbo.ufn_GetSalaryLevel(Salary) AS [Salary Level]
  FROM Employees
GO

--06.Employees by Salary Level
CREATE PROC usp_EmployeesBySalaryLevel(@SalaryLevel VARCHAR(10))
AS
SELECT FirstName, LastName
FROM Employees
WHERE dbo.ufn_GetSalaryLevel(Salary) = @SalaryLevel

EXEC usp_EmployeesBySalaryLevel 'High'
GO

--07.Define Function
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(MAX), @word VARCHAR(MAX)) 
RETURNS BIT
BEGIN
   DECLARE @Count INT = 1
   WHILE(@Count <= LEN(@word))
   BEGIN
       --DECLARE @CurrentLetter VARCHAR(1)
	   SET @CurrentLetter = SUBSTRING(@word, @Count, 1)
          IF CHARINDEX(@CurrentLetter, @setOfLetters, 1) = 0
	      RETURN 0
	   SET @Count += 1
   END
   RETURN 1
END
GO

--08.Delete Employees and Departments
DECLARE @departmentId INT

SELECT EmployeeID
FROM Employees
WHERE DepartmentID = @departmentId
GO

CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
BEGIN
  DELETE FROM EmployeesProjects
  WHERE EmployeeID IN
                     (
	                  SELECT EmployeeID
                      FROM Employees
                      WHERE DepartmentID = @departmentId
	                  )

  UPDATE Employees
  SET ManagerID = NULL
  WHERE ManagerID IN
                      (
	                  SELECT EmployeeID
                      FROM Employees
                      WHERE DepartmentID = @departmentId
	                  )

  ALTER TABLE Departments
  ALTER COLUMN ManagerID INT NULL

  UPDATE Departments
  SET ManagerID = NULL
  WHERE ManagerID IN
                      (
	                  SELECT EmployeeID
                      FROM Employees
                      WHERE DepartmentID = @departmentId
	                  )
  DELETE 
  FROM Employees
  WHERE DepartmentID = @departmentId;

  DELETE
  FROM Departments
  WHERE DepartmentID = @departmentId

  SELECT COUNT(*)
  FROM Employees
  WHERE DepartmentID = @departmentId
END
GO

--09.Find Full Name
USE Bank
GO

CREATE PROC usp_GetHoldersFullName
AS
SELECT FirstName + ' ' + LastName AS [Full Name]
FROM AccountHolders
GO

--10.People with Balance Higher Than
CREATE PROC usp_GetHoldersWithBalanceHigherThan(@AccountSum DECIMAL(38,2))
AS
BEGIN
SELECT FirstName, LastName
FROM AccountHolders AS ac
JOIN Accounts AS a ON ac.Id = a.AccountHolderId
GROUP BY ac.FirstName, ac.LastName
HAVING SUM(Balance) > @AccountSum
ORDER BY ac.FirstName, ac.LastName
END

EXEC usp_GetHoldersWithBalanceHigherThan 3000
GO

--11.Future Value Function
--FV=I×((1+R)^T)
CREATE FUNCTION ufn_CalculateFutureValue(@InitialSum DECIMAL(38,2), @YearlyInterestRate FLOAT, @Years INT)
RETURNS DECIMAL(36, 4)
BEGIN
DECLARE @FutureValue DECIMAL(36, 4)
SET @FutureValue = @InitialSum * POWER(1 + @YearlyInterestRate, @Years)
RETURN @FutureValue
END
GO

SELECT dbo.ufn_CalculateFutureValue(1000, 0.1, 5) AS [Output]
GO

--12.Calculating Interest
CREATE PROC usp_CalculateFutureValueForAccount(@UserAccountId INT, @YearlyInterestRate FLOAT)
AS
BEGIN 
   SELECT a.Id AS [Account Id], ac.FirstName AS [First Name], ac.LastName AS [Last Name], a.Balance AS [Current Balance], dbo.ufn_CalculateFutureValue(a.Balance, @YearlyInterestRate, 5) AS [Balance in 5 years]
   FROM AccountHolders AS ac
   JOIN Accounts AS a ON ac.Id = a.AccountHolderId
   WHERE a.Id = @UserAccountId
END

EXEC usp_CalculateFutureValueForAccount 1, 0.1
GO

--13.Cash in User Games Odd Rows
USE Diablo
GO

CREATE FUNCTION ufn_CashInUsersGames(@GameName NVARCHAR(50))
RETURNS TABLE
AS
RETURN SELECT 
(
SELECT SUM(Sub.Cash) AS SumCash
FROM
     (
      SELECT ug.Cash, ROW_NUMBER() OVER (ORDER BY ug.Cash DESC) AS [Row Number]
      FROM UsersGames AS ug 
      JOIN Games AS g ON g.Id = ug.GameId
      WHERE g.[Name] = @GameName
      ) Sub
      WHERE Sub.[Row Number] % 2 = 1
) AS SumCash

GO
SELECT * FROM ufn_CashInUsersGames('Love in a mist')
