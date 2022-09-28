--01.DDL
CREATE DATABASE CigarShop
GO 

USE CigarShop

CREATE TABLE Sizes
(
Id INT PRIMARY KEY IDENTITY,
[Length] INT NOT NULL,
CHECK([Length] BETWEEN 10 AND 25),
RingRange DECIMAL(3,2) NOT NULL,
CHECK (RingRange BETWEEN 1.5 AND 7.5)
)

CREATE TABLE Tastes
(
Id INT PRIMARY KEY IDENTITY,
TasteType VARCHAR(20) NOT NULL,
TasteStrength VARCHAR(15) NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Brands
(
Id INT PRIMARY KEY IDENTITY,
BrandName VARCHAR(30) UNIQUE NOT NULL,
BrandDescription VARCHAR(MAX)
)

CREATE TABLE Cigars
(
Id INT PRIMARY KEY IDENTITY,
CigarName VARCHAR(80) NOT NULL,
BrandId INT FOREIGN KEY REFERENCES Brands(Id) NOT NULL,
TastId INT FOREIGN KEY REFERENCES Tastes(Id) NOT NULL,
SizeId INT FOREIGN KEY REFERENCES Sizes(Id) NOT NULL,
PriceForSingleCigar DECIMAL(38,2) NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Addresses
(
Id INT PRIMARY KEY IDENTITY,
Town VARCHAR(30) NOT NULL,
Country NVARCHAR(30) NOT NULL,
Streat NVARCHAR(100) NOT NULL,
ZIP VARCHAR(20) NOT NULL
)

CREATE TABLE Clients
(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
Email NVARCHAR(50) NOT NULL,
AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL
)

CREATE TABLE ClientsCigars
(
ClientId INT FOREIGN KEY REFERENCES Clients(Id) NOT NULL,
CigarId INT FOREIGN KEY REFERENCES Cigars(Id) NOT NULL,
PRIMARY KEY (ClientId, CigarId)
)
--DML
--02.Insert

INSERT INTO Addresses VALUES
('Sofia', 'Bulgaria', '18 Bul. Vasil levski', '1000'),
('Athens', 'Greece', '4342 McDonald Avenue', '10435'),
('Zagreb', 'Croatia', '4333 Lauren Drive', '10000')

INSERT INTO Cigars VALUES
('COHIBA ROBUSTO', 9, 1, 5, 15.50, 'cohiba-robusto-stick_18.jpg'),
('COHIBA SIGLO I', 9, 1, 10, 410.00, 'cohiba-siglo-i-stick_12.jpg'),
('HOYO DE MONTERREY LE HOYO DU MAIRE', 14, 5, 11, 7.50, 'hoyo-du-maire-stick_17.jpg'),
('HOYO DE MONTERREY LE HOYO DE SAN JUAN', 14, 4, 15, 32.0, 'hoyo-de-san-juan-stick_20.jpg'),
('TRINIDAD COLONIALES', 2, 3, 8, 85.21, 'trinidad-coloniales-stick_30.jpg')

--03.Update
UPDATE Cigars
SET PriceForSingleCigar *= 1.2
WHERE TastId = (SELECT Id
                FROM Tastes
				WHERE TasteType = 'Spicy')

UPDATE Brands
SET BrandDescription = 'New description'
WHERE BrandDescription IS NULL

--04.Delete
DELETE FROM ClientsCigars
WHERE ClientId IN (SELECT Id
                  FROM Clients
				  WHERE AddressId IN (SELECT Id
                              FROM Addresses
			                  WHERE Country LIKE 'C%'))

DELETE FROM Clients
WHERE AddressId IN (SELECT Id
            FROM Addresses
			WHERE Country LIKE 'C%')

DELETE FROM Addresses
WHERE Country LIKE 'C%'

--Querying 
--05.Cigars by Price
SELECT CigarName, PriceForSingleCigar, ImageURL
FROM Cigars
ORDER BY PriceForSingleCigar, CigarName DESC

--06.Cigars by Taste
SELECT c.Id, c.CigarName, c.PriceForSingleCigar, t.TasteType, t.TasteStrength
FROM Cigars AS c
JOIN Tastes AS t ON c.TastId = t.Id
WHERE TasteType IN ('Earthy', 'Woody')
ORDER BY c.PriceForSingleCigar DESC

--07.Clients without Cigars
SELECT cl.Id, cl.FirstName + ' ' + cl.LastName AS ClientName, cl.Email
FROM Clients AS cl
LEFT JOIN ClientsCigars AS clcg ON cl.Id = clcg.ClientId
LEFT JOIN Cigars AS cg ON clcg.CigarId = cg.Id
WHERE clcg.ClientId IS NULL
ORDER BY ClientName

--08.First 5 Cigars
SELECT TOP 5 c.CigarName, c.PriceForSingleCigar, c.ImageURL
FROM Cigars AS c
JOIN Sizes AS s ON c.SizeId = s.Id
WHERE (s.[Length] >= 12 AND (c.CigarName LIKE '%ci%' OR c.PriceForSingleCigar > 50) AND s.RingRange > 2.55)
ORDER BY c.CigarName, c.PriceForSingleCigar DESC

--09.Clients with ZIP Codes
SELECT cl.FirstName + ' ' + cl.LastName AS FullName, a.Country, a.ZIP, '$' + CAST(MAX(cg.PriceForSingleCigar) AS NVARCHAR(10)) AS CigarPrice
FROM Clients AS cl
JOIN Addresses AS a ON cl.AddressId = a.Id
JOIN ClientsCigars AS clcg ON clcg.ClientId = cl.Id
JOIN Cigars AS cg ON clcg.CigarId = cg.Id
WHERE ISNUMERIC(a.ZIP) = 1
GROUP BY cl.FirstName + ' ' + cl.LastName, a.Country, a.ZIP

--10.Cigars by Size
SELECT cl.LastName, AVG(s.[Length]) AS CiagrLength, CEILING(AVG(s.RingRange)) AS CiagrRingRange
FROM ClientsCigars AS clcg
JOIN Clients AS cl ON cl.Id = clcg.ClientId
JOIN Cigars AS cg ON clcg.CigarId = cg.Id
JOIN Sizes AS s ON s.Id = cg.SizeId
GROUP BY cl.LastName
ORDER BY AVG(s.[Length]) DESC
GO

--11.Client with Cigars
CREATE FUNCTION udf_ClientWithCigars(@name NVARCHAR(30))
RETURNS NVARCHAR(30)
BEGIN
DECLARE @result NVARCHAR(30) = (SELECT COUNT(*)
                                FROM Clients AS cl
                                JOIN ClientsCigars AS clcg ON cl.Id = clcg.ClientId
                                JOIN Cigars AS cg ON cg.Id = clcg.CigarId
                                WHERE cl.FirstName = @name)
RETURN @result
END
GO

SELECT dbo.udf_ClientWithCigars('Betty') AS [Output]
GO

--12.Search for Cigar with Specific Taste
CREATE PROC usp_SearchByTaste(@taste VARCHAR(20))
AS
BEGIN
SELECT cg.CigarName, '$' + CAST(cg.PriceForSingleCigar AS NVARCHAR(20)) AS Price, ts.TasteType, br.BrandName, CAST(sz.[Length] AS NVARCHAR(20)) + ' cm' AS CigarLength, CAST(sz.RingRange AS NVARCHAR(20)) + ' cm' AS CigarRingRange
FROM Cigars AS cg
JOIN Tastes AS ts ON cg.TastId = ts.Id
JOIN Brands AS br ON cg.BrandId = br.Id
JOIN Sizes AS sz ON cg.SizeId = sz.Id
WHERE ts.TasteType = @taste
ORDER BY sz.[Length], sz.RingRange DESC 
END

EXEC usp_SearchByTaste 'Woody'