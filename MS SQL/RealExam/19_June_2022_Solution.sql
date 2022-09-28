--01.DDL
CREATE DATABASE Zoo
GO

USE Zoo

CREATE TABLE Owners
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
PhoneNumber VARCHAR(15) NOT NULL,
[Address] VARCHAR(50)
)

CREATE TABLE AnimalTypes
(
Id INT PRIMARY KEY IDENTITY,
AnimalType VARCHAR(30) NOT NULL
)

CREATE TABLE Cages
(
Id INT PRIMARY KEY IDENTITY,
AnimalTypeId INT FOREIGN KEY REFERENCES AnimalTypes (Id) NOT NULL
)

CREATE TABLE Animals
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(30) NOT NULL,
BirthDate DATE NOT NULL,
OwnerId INT FOREIGN KEY REFERENCES Owners(Id),
AnimalTypeId INT FOREIGN KEY REFERENCES AnimalTypes (Id) NOT NULL
)

CREATE TABLE AnimalsCages
(
CageId INT FOREIGN KEY REFERENCES Cages (Id) NOT NULL,
AnimalId INT FOREIGN KEY REFERENCES Animals (Id) NOT NULL,
PRIMARY KEY (CageId,AnimalId)
)

CREATE TABLE VolunteersDepartments
(
Id INT PRIMARY KEY IDENTITY,
DepartmentName VARCHAR(30) NOT NULL
)

CREATE TABLE Volunteers
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
PhoneNumber VARCHAR(15) NOT NULL,
[Address] VARCHAR(50),
AnimalId INT FOREIGN KEY REFERENCES Animals(Id),
DepartmentId INT FOREIGN KEY REFERENCES VolunteersDepartments(Id) NOT NULL
)

--DML
--02.Insert
INSERT INTO Volunteers VALUES
('Anita Kostova', '0896365412', 'Sofia, 5 Rosa str.', 15, 1),
('Dimitur Stoev', '0877564223', NULL, 42, 4),
('Kalina Evtimova', '0896321112', 'Silistra, 21 Breza str.', 9, 7),
('Stoyan Tomov', '0898564100', 'Montana, 1 Bor str.', 18, 8),
('Boryana Mileva', '0888112233', NULL, 31, 5)

INSERT INTO Animals VALUES
('Giraffe', '2018-09-21', 21, 1),
('Harpy Eagle', '2015-04-17', 15, 3),
('Hamadryas Baboon', '2017-11-02', NULL, 1),
('Tuatara', '2021-06-30', 2, 4)

--03.Update
SELECT *
FROM Animals
WHERE OwnerId IS NULL

SELECT Id
FROM Owners
WHERE [Name] = 'Kaloqn Stoqnov'

UPDATE Animals
SET OwnerId = (SELECT Id
               FROM Owners
               WHERE [Name] = 'Kaloqn Stoqnov')
WHERE OwnerId IS NULL

--04.Delete
SELECT Id
FROM VolunteersDepartments
WHERE DepartmentName = 'Education program assistant'

DELETE
FROM Volunteers
WHERE DepartmentId = (SELECT Id
                      FROM VolunteersDepartments
                      WHERE DepartmentName = 'Education program assistant')

DELETE 
FROM VolunteersDepartments
WHERE DepartmentName = 'Education program assistant'

--Querying
--05.Volunteers
SELECT [Name], PhoneNumber, [Address], AnimalId, DepartmentId
FROM Volunteers
ORDER BY [Name], AnimalId, DepartmentId

--06.Animals data
SELECT [Name], animt.AnimalType, FORMAT(a.BirthDate, 'dd.MM.yyyy')
FROM Animals AS a
LEFT JOIN AnimalTypes AS animt ON a.AnimalTypeId = animt.Id 
ORDER BY a.[Name]

--07.Owners and Their Animals
SELECT TOP 5 o.[Name] AS [Owner], COUNT(*) AS CountOfAnimals
FROM Owners AS o
LEFT JOIN Animals AS a ON a.OwnerId = o.Id
GROUP BY o.[Name]
ORDER BY CountOfAnimals DESC, [Owner]

--08.Owners, Animals and Cages
SELECT CONCAT(o.[Name], '-', a.[Name]) AS OwnersAnimals, o.PhoneNumber, c.Id AS CageId
FROM Owners AS o
JOIN Animals AS a ON o.Id = a.OwnerId
JOIN AnimalTypes AS animt ON a.AnimalTypeId = animt.Id
JOIN AnimalsCages AS ac ON a.Id = ac.AnimalId
JOIN Cages AS c ON c.Id = ac.CageId
WHERE animt.AnimalType = 'Mammals'
ORDER BY o.[Name], a.[Name] DESC

--09.Volunteers in Sofia
--TRIM()

SELECT v.[Name], v.PhoneNumber, 
CASE 
   WHEN CHARINDEX('Sofia, ', v.[Address], 1) > 0 THEN TRIM(REPLACE(v.[Address], 'Sofia, ', ''))
   ELSE TRIM(REPLACE(v.[Address], 'Sofia ,', ''))
END AS [Address]
FROM Volunteers AS v
JOIN VolunteersDepartments AS vd ON v.DepartmentId = vd.Id
WHERE vd.DepartmentName = 'Education program assistant' AND v.[Address] LIKE '%Sofia%'
ORDER BY v.[Name]

--10.Animals for Adoption
SELECT a.[Name], DATEPART(YEAR, a.BirthDate) AS BirthYear, animt.AnimalType
FROM Animals AS a
LEFT JOIN AnimalTypes AS animt ON animt.Id = a.AnimalTypeId
WHERE DATEDIFF(YEAR, a.BirthDate, '2022-01-01') < 5 AND a.OwnerId IS NULL AND animt.AnimalType <> 'Birds'
ORDER BY a.[Name]
GO

--11. All Volunteers in a Department
CREATE FUNCTION udf_GetVolunteersCountFromADepartment (@VolunteersDepartment VARCHAR(30)) 
RETURNS INT
AS
BEGIN
DECLARE @volunteersCount INT = (SELECT COUNT(*)
                                FROM Volunteers AS v
                                JOIN VolunteersDepartments AS vd ON v.DepartmentId = vd.Id
                                WHERE vd.DepartmentName = @VolunteersDepartment
                                GROUP BY vd.DepartmentName)
RETURN ISNULL(@volunteersCount, 0)
END
GO

SELECT dbo.udf_GetVolunteersCountFromADepartment ('Education program assistant')
SELECT dbo.udf_GetVolunteersCountFromADepartment ('Guest engagement')
SELECT dbo.udf_GetVolunteersCountFromADepartment ('Zoo events')
GO

--12.Animals with Owner or Not
CREATE PROC usp_AnimalsWithOwnersOrNot(@AnimalName VARCHAR(30))
AS
BEGIN
      SELECT 
      a.[Name],
      CASE WHEN o.[Name] IS NOT NULL THEN o.[Name]
      ELSE 'For adoption'
      END AS OwnersName
      FROM Animals AS a
      LEFT JOIN Owners AS o ON a.OwnerId = o.Id
      WHERE a.[Name] = @AnimalName
END
GO

EXEC usp_AnimalsWithOwnersOrNot 'Pumpkinseed Sunfish'
EXEC usp_AnimalsWithOwnersOrNot 'Hippo'
EXEC usp_AnimalsWithOwnersOrNot 'Brown bear'
