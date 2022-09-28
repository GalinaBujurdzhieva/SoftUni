--01.DDL
CREATE DATABASE ColonialJourney
GO

USE ColonialJourney

CREATE TABLE Planets
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Spaceports
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
PlanetId INT FOREIGN KEY REFERENCES Planets(Id) NOT NULL
)

CREATE TABLE Spaceships
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
Manufacturer VARCHAR(30) NOT NULL,
LightSpeedRate INT DEFAULT 0
)

CREATE TABLE Colonists
(
Id INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(20) NOT NULL,
LastName VARCHAR(20) NOT NULL,
Ucn VARCHAR(10) UNIQUE NOT NULL,
BirthDate DATE NOT NULL
)

CREATE TABLE Journeys
(
Id INT PRIMARY KEY IDENTITY,
JourneyStart DATETIME NOT NULL,
JourneyEnd DATETIME NOT NULL,
Purpose VARCHAR(11),
CHECK(Purpose IN('Medical', 'Technical', 'Educational', 'Military')),
DestinationSpaceportId INT FOREIGN KEY REFERENCES Spaceports(Id) NOT NULL,
SpaceshipId INT FOREIGN KEY REFERENCES Spaceships(Id) NOT NULL
)

CREATE TABLE TravelCards
(
Id INT PRIMARY KEY IDENTITY,
CardNumber CHAR(10) UNIQUE NOT NULL,
JobDuringJourney VARCHAR(8),
CHECK(JobDuringJourney IN('Pilot', 'Engineer', 'Trooper', 'Cleaner', 'Cook')),
ColonistId INT FOREIGN KEY REFERENCES Colonists(Id) NOT NULL,
JourneyId INT FOREIGN KEY REFERENCES Journeys(Id) NOT NULL
)

--DML
--02.Insert
INSERT INTO Planets VALUES
('Mars'),
('Earth'),
('Jupiter'),
('Saturn')

INSERT INTO Spaceships VALUES
('Golf', 'VW', 3),
('WakaWaka', 'Wakanda', 4),
('Falcon9', 'SpaceX', 1),
('Bed', 'Vidolov', 6)

--03.Update
UPDATE Spaceships
SET LightSpeedRate += 1
WHERE Id BETWEEN 8 AND 12

--04.Delete
DELETE FROM TravelCards
WHERE JourneyId IN (1, 2, 3)

DELETE FROM Journeys
WHERE Id IN (1, 2, 3)

--Querying 
--05.Select all military journeys
SELECT Id, FORMAT(JourneyStart, 'dd/MM/yyyy') AS JourneyStart, FORMAT(JourneyEnd, 'dd/MM/yyyy') AS JourneyEnd
FROM Journeys
WHERE Purpose = 'Military'
ORDER BY JourneyStart

--06.Select all pilots
SELECT c.Id, CONCAT(c.FirstName, ' ', c.LastName) AS full_name
FROM Colonists AS c
JOIN TravelCards AS tc ON c.Id = tc.ColonistId
WHERE tc.JobDuringJourney = 'Pilot'
ORDER BY c.Id

--07.Count colonists
SELECT COUNT(*) AS [count]
FROM Colonists AS c
JOIN TravelCards AS tc ON c.Id = tc.ColonistId
JOIN Journeys AS j ON tc.JourneyId = j.Id
WHERE j.Purpose = 'Technical'

--08.Select spaceships with pilots younger than 30 years
SELECT s.[Name], s.Manufacturer
FROM Spaceships AS s
JOIN Journeys AS j ON j.SpaceshipId = s.Id
JOIN TravelCards AS tc ON j.Id = tc.JourneyId AND tc.JobDuringJourney = 'Pilot'
JOIN Colonists AS c ON tc.ColonistId = c.Id
WHERE DATEDIFF(YEAR, c.Birthdate, '2019-01-01') < 30
ORDER BY s.[Name]

--09.Select all planets and their journey count
SELECT p.[Name] AS PlanetName, COUNT(j.Id) AS JourneysCount
FROM Planets AS p
JOIN Spaceports AS s ON p.Id = s.PlanetId
JOIN Journeys AS j ON s.Id = j.DestinationSpaceportId
GROUP BY p.[Name]
ORDER BY COUNT(j.Id) DESC, p.[Name]

--10.Select Second Oldest Important Colonist
SELECT SubQuery.JobDuringJourney, SubQuery.FullName, SubQuery.JobRank
FROM
     (SELECT tc.JobDuringJourney, 
	 CONCAT(c.FirstName, ' ', c.LastName) AS FullName, 
	 DENSE_RANK() OVER (PARTITION BY tc.JobDuringJourney  ORDER BY c.Birthdate) AS JobRank
     FROM Journeys AS j
     JOIN TravelCards AS tc ON tc.JourneyId = j.Id
     JOIN Colonists AS c ON c.Id = tc.ColonistId) AS SubQuery
WHERE SubQuery.JobRank = 2
GO

--Programmability
--11.Get Colonists Count
CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR (30))
RETURNS INT
BEGIN
DECLARE @colonistsCoint INT = (SELECT COUNT(*) AS [Count]
                               FROM Planets AS p
                               JOIN Spaceports AS sp ON p.Id = sp.PlanetId
                               JOIN Journeys AS j ON sp.Id = j.DestinationSpaceportId
                               JOIN TravelCards AS tc ON tc.JourneyId = j.Id
                               JOIN Colonists AS c ON c.Id = tc.ColonistId
                               WHERE p.[Name] = @PlanetName)
RETURN @colonistsCoint
END
GO

SELECT dbo.udf_GetColonistsCount('Otroyphus')
GO

--12.Change Journey Purpose
CREATE PROC usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(11)) 
AS
BEGIN
  DECLARE @myJourneyId INT = (SELECT Id
                            FROM Journeys
							WHERE Id = @JourneyId)

  IF(@myJourneyId IS NULL)
     BEGIN;
       THROW 50001, 'The journey does not exist!', 1
     END

  DECLARE @myJourneyPurpose VARCHAR(11) = (SELECT Purpose
                                           FROM Journeys
							               WHERE Id = @JourneyId)

  IF(@myJourneyPurpose = @NewPurpose)
    BEGIN;
	  THROW 50002, 'You cannot change the purpose!', 1
	END

  UPDATE Journeys
  SET Purpose = @NewPurpose
  WHERE Id = @JourneyId
END
GO

EXEC usp_ChangeJourneyPurpose 4, 'Technical'	
EXEC usp_ChangeJourneyPurpose 2, 'Educational'	--You cannot change the purpose!
EXEC usp_ChangeJourneyPurpose 196, 'Technical'	--The journey does not exist!
