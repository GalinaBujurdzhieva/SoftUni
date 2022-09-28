--01.DDL
CREATE DATABASE TripService
GO

USE TripService

CREATE TABLE Cities
(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(20) NOT NULL,
CountryCode CHAR(2) NOT NULL
)

CREATE TABLE Hotels
(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30) NOT NULL,
CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
EmployeeCount INT NOT NULL,
BaseRate DECIMAL(38,2)
)

CREATE TABLE Rooms
(
Id INT PRIMARY KEY IDENTITY,
Price DECIMAL(38,2) NOT NULL,
[Type] NVARCHAR(20) NOT NULL,
Beds INT NOT NULL,
HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL
)

CREATE TABLE Trips
(
Id INT PRIMARY KEY IDENTITY,
RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL,
BookDate DATETIME NOT NULL,
CHECK(BookDate < ArrivalDate),
ArrivalDate DATETIME NOT NULL,
CHECK(ArrivalDate < ReturnDate),
ReturnDate DATETIME NOT NULL, 
CancelDate DATETIME
)

CREATE TABLE Accounts
(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(50) NOT NULL,
MiddleName NVARCHAR(20),
LastName NVARCHAR(50) NOT NULL,
CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
BirthDate DATETIME NOT NULL,
Email VARCHAR(100) UNIQUE NOT NULL
)

CREATE TABLE AccountsTrips
(
AccountId INT FOREIGN KEY REFERENCES Accounts(Id) NOT NULL,
TripId INT FOREIGN KEY REFERENCES Trips(Id) NOT NULL,
PRIMARY KEY (AccountId, TripId),
Luggage INT NOT NULL,
CHECK(Luggage >= 0)
)

--DML
--02.Insert
INSERT INTO Accounts VALUES
('John', 'Smith', 'Smith', 34, '1975-07-21', 'j_smith@gmail.com'),
('Gosho', NULL,	'Petrov', 11, '1978-05-16', 'g_petrov@gmail.com'),
('Ivan', 'Petrovich', 'Pavlov', 59, '1849-09-26', 'i_pavlov@softuni.bg'),
('Friedrich', 'Wilhelm', 'Nietzsche', 2, '1844-10-15', 'f_nietzsche@softuni.bg')

INSERT INTO Trips VALUES
(101, '2015-04-12', '2015-04-14', '2015-04-20', '2015-02-02'),
(102, '2015-07-07', '2015-07-15', '2015-07-22', '2015-04-29'),
(103, '2013-07-17', '2013-07-23', '2013-07-24', NULL),
(104, '2012-03-17', '2012-03-31', '2012-04-01', '2012-01-10'),
(109, '2017-08-07',	'2017-08-28', '2017-08-29', NULL)

--03.Update
SELECT *
FROM Rooms
WHERE HotelId IN (5, 7, 9)

UPDATE Rooms
SET Price *= 1.14
WHERE HotelId IN (5, 7, 9)

--04.Delete
SELECT *
FROM AccountsTrips
WHERE AccountId = 47

DELETE FROM AccountsTrips
WHERE AccountId = 47

--Querying 
--05.EEE-Mails
SELECT a.FirstName, a.LastName, FORMAT(a.BirthDate, 'MM-dd-yyyy') AS BirthDate, c.[Name] AS Hometown, a.Email
FROM Accounts AS a
JOIN Cities AS c ON a.CityId = c.Id
WHERE a.Email LIKE 'e%'
ORDER BY c.[Name]

--06.City Statistics
SELECT c.[Name] AS City, COUNT(*) AS Hotels
FROM Cities AS c
JOIN Hotels AS h ON c.Id = h.CityId
GROUP BY c.[Name]
ORDER BY COUNT(*) DESC, c.[Name]

--07.Longest and Shortest Trips
SELECT a.Id AS AccountId, a.FirstName + ' ' + a.LastName AS FullName, MAX(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)) AS LongestTrip, MIN(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)) AS ShortestTrip
FROM Accounts AS a
JOIN AccountsTrips AS ac ON a.Id = ac.AccountId
JOIN Trips AS t ON ac.TripId = t.Id
WHERE a.MiddleName IS NULL AND t.CancelDate IS NULL
GROUP BY a.Id, a.FirstName + ' ' + a.LastName
ORDER BY MAX(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)) DESC, MIN(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate))

--08.Metropolis
SELECT TOP 10 c.Id, c.[Name] AS City, c.CountryCode AS Country, COUNT(*) AS Accounts
FROM Cities AS c
JOIN Accounts AS a ON c.Id = a.CityId
GROUP BY c.Id, c.[Name], c.CountryCode
ORDER BY COUNT(*) DESC

--09.Romantic Getaways
SELECT SubQuery.Id, SubQuery.Email, SubQuery.City, COUNT(*) AS Trips
FROM  
     (SELECT a.Id, a.Email, c.[Name] AS City
     FROM Accounts AS a
     JOIN Cities AS c ON a.CityId = c.Id
     JOIN AccountsTrips AS atr ON a.Id = atr.AccountId
     JOIN Trips AS tr ON atr.TripId = tr.Id
     JOIN Rooms AS r ON tr.RoomId = r.Id
     JOIN Hotels AS h ON r.HotelId = h.Id
     WHERE a.CityId = h.CityId) AS SubQuery
GROUP BY SubQuery.Id, SubQuery.Email, SubQuery.City
ORDER BY COUNT(*) DESC, SubQuery.Id

--10.GDPR Violation
SELECT tr.Id,
       CONCAT(a.FirstName,' ', ISNULL(a.MiddleName + ' ', ''), a.LastName) AS [Full Name],
	   c.[Name] AS [From],
	   hotcit.[Name] AS [To],
	   CASE
	   WHEN tr.CancelDate IS NULL THEN CAST(DATEDIFF(DAY, tr.ArrivalDate, tr.ReturnDate) AS VARCHAR) + ' days' 
	   ELSE 'Canceled'
	   END AS Duration
FROM Accounts AS a 
JOIN AccountsTrips AS atr ON atr.AccountId = a.Id
JOIN Trips AS tr ON tr.Id = atr.TripId
JOIN Cities AS c ON a.CityId = c.Id
JOIN Rooms AS r ON tr.RoomId = r.Id
JOIN Hotels AS h ON r.HotelId = h.Id
JOIN Cities AS hotcit ON h.CityId = hotcit.Id
ORDER BY CONCAT(a.FirstName,' ', ISNULL(a.MiddleName + ' ', ''), a.LastName), tr.Id
GO

--Programmability
--11.Available Room
CREATE FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(200)
AS 
BEGIN
DECLARE @desiredRoomId INT = (SELECT TOP 1
                              r.Id
                              FROM Rooms AS r
                              LEFT JOIN Trips AS t ON t.RoomId = r.Id
                              WHERE 
                              HotelId = @HotelId 
                              AND 
                              r.Beds > @People
                              AND 
                              ((@Date NOT BETWEEN t.ArrivalDate AND t.ReturnDate)
                              OR 
                              (@Date BETWEEN t.ArrivalDate AND t.ReturnDate
                              AND t.CancelDate IS NOT NULL))
                              ORDER BY r.Price DESC)

IF(@desiredRoomId IS NULL)
   BEGIN
     RETURN 'No rooms available'
   END

DECLARE @myHotelBaseRate DECIMAL(38,2) = (SELECT BaseRate
	                                      FROM Hotels
										  WHERE Id = @HotelId) 

DECLARE @desiredRoomPrice DECIMAL(38,2) = (SELECT Price
	                                       FROM Rooms
										   WHERE Id = @desiredRoomId)
DECLARE @totalPrice DECIMAL(38,2) = (@myHotelBaseRate + @desiredRoomPrice) * @People

DECLARE @myRoomType NVARCHAR(20) = (SELECT [Type]
                                    FROM Rooms
									WHERE Id = @desiredRoomId)
DECLARE @myRoomBedsCount INT = (SELECT Beds
                                FROM Rooms
							    WHERE Id = @desiredRoomId)
RETURN CONCAT('Room ', CAST(@desiredRoomId AS VARCHAR), ': ', @myRoomType, ' (', CAST(@myRoomBedsCount AS VARCHAR), ' beds) - $', CAST(@totalPrice AS VARCHAR))
END
GO

SELECT dbo.udf_GetAvailableRoom(112, '2011-12-17', 2)
SELECT dbo.udf_GetAvailableRoom(94, '2015-07-26', 3)
GO

--12.Switch Room
CREATE PROC usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
BEGIN
   DECLARE @tripHotelId INT = (SELECT r.HotelId
                               FROM Trips AS t
                               JOIN Rooms AS r ON t.RoomId = r.Id
                               WHERE t.Id = @TripId)
   
   DECLARE @newRoomHotelId INT = (SELECT HotelId
                                  FROM Rooms
                                  WHERE Id = @TargetRoomId)
   
   IF(@tripHotelId != @newRoomHotelId)
      BEGIN;
         THROW 50001, 'Target room is in another hotel!', 1
      END
   
   DECLARE @newRoomBedsCount INT = (SELECT Beds
                                    FROM Rooms
                                    WHERE Id = @TargetRoomId)
   
   DECLARE @accountsCountForTheTrip INT = (SELECT COUNT(*)
                                           FROM AccountsTrips   
   									       WHERE TripId = @TripId)
   
   IF(@newRoomBedsCount < @accountsCountForTheTrip)
      BEGIN;
         THROW 50002, 'Not enough beds in target room!', 1
      END
   
   UPDATE Trips
   SET RoomId = @TargetRoomId 
   WHERE Id = @TripId
END

EXEC usp_SwitchRoom 10, 11
SELECT RoomId FROM Trips WHERE Id = 10
EXEC usp_SwitchRoom 10, 7
EXEC usp_SwitchRoom 10, 8

