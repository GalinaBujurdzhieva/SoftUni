--14/1.Create Table Logs
USE Bank
CREATE TABLE Logs 
(
LogId INT PRIMARY KEY IDENTITY, 
AccountId INT FOREIGN KEY REFERENCES Accounts(Id) NOT NULL, 
OldSum MONEY, 
NewSum MONEY
)
GO

CREATE TRIGGER tr_LogWhenAccountChanges
ON Accounts FOR UPDATE
AS
    INSERT INTO Logs (AccountId, OldSum, NewSum)
	SELECT i.AccountHolderId, d.Balance, i.Balance
	FROM inserted AS i
	JOIN deleted AS d ON i.Id = d.Id
GO

--15/2.Create Table Emails
CREATE TABLE NotificationEmails
(
Id INT PRIMARY KEY NOT NULL, 
Recipient INT FOREIGN KEY REFERENCES Accounts(Id) NOT NULL, 
[Subject] VARCHAR(100) NOT NULL, 
Body VARCHAR(MAX) NOT NULL
)
GO

CREATE TRIGGER tr_NewEmailLog
ON Logs AFTER INSERT
AS
   INSERT INTO NotificationEmails (Recipient, [Subject], Body)
   SELECT i.AccountId,
          'Balance change for account: ' + CONVERT(VARCHAR, i.AccountId),
		  CONCAT('On ', CONVERT(VARCHAR, FORMAT (GETDATE(), 'MMM dd yyyy hh:mmtt')), ' your balance was changed from ', i.OldSum, ' to ', i.NewSum, '.')
   FROM inserted AS i
GO

--16/3.Deposit Money
CREATE PROC usp_DepositMoney (@AccountId INT, @MoneyAmount MONEY)
AS
BEGIN TRANSACTION
    DECLARE @NeededAccountId INT = (SELECT Id FROM Accounts WHERE Id = @AccountId)
	
	IF (@NeededAccountId IS NULL)
	BEGIN
	     ROLLBACK;
		 THROW 50001, 'The account does not exist!', 1
		 RETURN
	END
	
	IF (@MoneyAmount < 0)
	BEGIN
	     ROLLBACK;
		 THROW 50002, 'Negative money amount!', 1
		 RETURN
	END

UPDATE Accounts
SET Balance += ROUND(@MoneyAmount, 4)
WHERE Id = @AccountId
COMMIT
GO

--17/4.Withdraw Money Procedure
CREATE PROC usp_WithdrawMoney (@AccountId INT, @MoneyAmount MONEY) 
AS
BEGIN TRANSACTION
DECLARE @NeededAccountId INT = (SELECT Id FROM Accounts WHERE Id = @AccountId)
    IF (@NeededAccountId IS NULL)
	BEGIN
	ROLLBACK;
	THROW 50001, 'The account does not exist!', 2
	RETURN
	END

	IF (@MoneyAmount < 0)
	BEGIN
	ROLLBACK;
	THROW 50002, 'Negative money amount!', 2
	RETURN
	END

DECLARE @NeededAcountBalance MONEY = (SELECT Balance FROM Accounts WHERE Id = @AccountId)
DECLARE @BalanceAfterWithdraw MONEY = @NeededAcountBalance - @MoneyAmount
    IF (@BalanceAfterWithdraw < 0)
	BEGIN
	ROLLBACK;
	THROW 50003, 'Insufficient money amount', 1
	RETURN
	END

UPDATE Accounts
SET Balance -= ROUND(@MoneyAmount, 4)
WHERE Id = @AccountId
COMMIT
GO

--18/5.Money Transfer
CREATE PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount MONEY)
AS
BEGIN TRANSACTION
    EXEC usp_WithdrawMoney @SenderId, @Amount
    EXEC usp_DepositMoney @ReceiverId, @Amount
COMMIT
GO

--20/7.Massive Shopping
USE Diablo
GO
--Stamat UserId = 9;
--Safflower GameId = 87;
--StamatSafflowerId = 110;

DECLARE @UserId INT = (SELECT Id FROM Users WHERE Username = 'Stamat')
DECLARE @GameId INT = (SELECT Id FROM Games WHERE [Name] = 'Safflower')
DECLARE @UserGameId INT = (SELECT Id FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId)
DECLARE @UserCash MONEY = (SELECT Cash FROM UsersGames
                           WHERE UserId = @UserId AND GameId = @GameId)

BEGIN TRANSACTION
DECLARE @ItemsLevel11To12TotalPrice MONEY = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 11 AND 12)
   
   /*IF(@UserCash < @ItemsLevel11To12TotalPrice)
      BEGIN
        ROLLBACK;
	    THROW 50001, 'Insuffient money for buying items', 1
	    RETURN
      END
	  */
   IF(@UserCash >= @ItemsLevel11To12TotalPrice)
	  BEGIN
		INSERT INTO UserGameItems
	    SELECT i.Id, @UserGameID
        FROM Items AS i
        WHERE i.Id IN (SELECT Id FROM Items WHERE MinLevel BETWEEN 11 AND 12)
	  
	    UPDATE UsersGames
        SET Cash -= @ItemsLevel11To12TotalPrice
        WHERE GameId = @GameID AND UserId = @UserID
	  COMMIT
	  END

SET @UserCash = (SELECT Cash FROM UsersGames WHERE UserId = @UserID AND GameId = @GameID)
BEGIN TRANSACTION
DECLARE @ItemsLevel19To21TotalPrice MONEY = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 19 AND 21)
   
   /*IF(@UserCash < @ItemsLevel19To21TotalPrice)
      BEGIN
        ROLLBACK;
	    THROW 50001, 'Insuffient money for buying items', 2
	    RETURN
      END
	  */
   IF(@UserCash >= @ItemsLevel19To21TotalPrice)
	  BEGIN
		INSERT INTO UserGameItems
	    SELECT i.Id, @UserGameID
        FROM Items AS i
        WHERE i.Id IN (SELECT Id FROM Items WHERE MinLevel BETWEEN 19 AND 21)
	  
	    UPDATE UsersGames
        SET Cash -= @ItemsLevel19To21TotalPrice
        WHERE GameId = @GameID AND UserId = @UserID
	  COMMIT
	  END

SELECT [Name] AS [Item Name]
FROM Items AS i
JOIN UserGameItems AS ugi ON i.Id = ugi.ItemId
WHERE ugi.UserGameId = 110
ORDER BY i.[Name]

--21/8.Employees with Three Projects
USE SoftUni
GO

CREATE PROC usp_AssignProject(@emloyeeId INT, @projectID INT)
AS
BEGIN TRANSACTION
DECLARE @NeededEmloyeeId INT = (SELECT EmployeeID FROM Employees WHERE EmployeeID = @emloyeeId)
      IF (@NeededEmloyeeId IS NULL)
	  BEGIN
	  ROLLBACK;
	  THROW 50001, 'Invalid Employee ID', 1
	  RETURN
	  END

DECLARE @NeededProjectId INT = (SELECT ProjectID FROM Projects WHERE ProjectID = @projectID)
      IF (@NeededProjectId IS NULL)
	  BEGIN
	  ROLLBACK;
	  THROW 50002, 'Invalid Project ID', 1
	  RETURN
	  END

DECLARE @EmployeeProjectCount INT = (
                                     SELECT COUNT(*) FROM Employees AS e
                                     JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
                                     WHERE e.EmployeeID = @emloyeeId
									)
     IF (@EmployeeProjectCount >= 3)
	 BEGIN
	 ROLLBACK;
	 THROW 50003, 'The employee has too many projects!', 1
	 RETURN
	 END

INSERT INTO EmployeesProjects(EmployeeID, ProjectID) VALUES
(@emloyeeId, @projectID)
COMMIT
GO

SELECT COUNT(*) FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
WHERE e.EmployeeID = 2

--22/9.Delete Employees
CREATE TABLE Deleted_Employees
(
EmployeeId INT PRIMARY KEY NOT NULL, 
FirstName VARCHAR(50) NOT NULL, 
LastName VARCHAR(50) NOT NULL, 
MiddleName VARCHAR(50), 
JobTitle VARCHAR(50) NOT NULL, 
DepartmentId INT FOREIGN KEY REFERENCES Departments(DepartmentID) NOT NULL,
Salary MONEY NOT NULL
) 
GO

CREATE TRIGGER tr_DeletedEmployees
ON Employees FOR DELETE
AS
INSERT INTO Deleted_Employees (FirstName, LastName, MiddleName, JobTitle, DepartmentId, Salary)
SELECT d.FirstName, d.LastName, d.MiddleName, d.JobTitle, d.DepartmentID, d.Salary
FROM deleted AS d
