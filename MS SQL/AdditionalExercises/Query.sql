--01.Number of Users for Email Provider
USE Diablo

SELECT SubQuery.[Email Provider], COUNT(*) AS [Number Of Users] FROM
             (
			 SELECT RIGHT(Email, LEN(Email) - CHARINDEX('@', Email, 1)) as [Email Provider]
             FROM Users
			 ) SubQuery
GROUP BY SubQuery.[Email Provider]
ORDER BY [Number Of Users] DESC, [Email Provider]

--02.All Users in Games
SELECT g.[Name] AS Game, gt.[Name] as [Game Type], u.Username, ug.[Level], ug.Cash, c.[Name] AS [Character]
FROM Games AS g
JOIN GameTypes AS gt ON gt.Id = g.GameTypeId
JOIN UsersGames AS ug ON g.Id = ug.GameId
JOIN Users AS u ON u.Id = ug.UserId
JOIN Characters AS c ON c.Id = ug.CharacterId
ORDER BY ug.[Level] DESC, u.Username, g.[Name]

--03.Users in Games with Their Items
SELECT u.Username, g.[Name] AS [Game], COUNT(i.Id) AS [Items Count], SUM(i.Price) AS [Items Price]
FROM Users AS u
JOIN UsersGames AS ug ON u.Id = ug.UserId
JOIN Games AS g ON g.Id = ug.GameId
JOIN UserGameItems AS ugi ON ug.Id = ugi.UserGameId
JOIN Items AS i ON i.Id = ugi.ItemId
GROUP BY u.Username, g.[Name]
HAVING COUNT(i.Id) >= 10
ORDER BY COUNT(i.Id) DESC, SUM(i.Price) DESC, u.Username

--04.*User in Games with Their Statistics
SELECT 
       u.Username, 
	   g.[Name] AS Game, 
	   MAX(c.[Name]) AS [Character], 
	   SUM(statItems.Strength) + MAX(statCharacters.Strength) + MAX(statGamesTypes.Strength) AS Strength, 
	   SUM(statItems.Defence) + MAX(statCharacters.Defence) + MAX(statGamesTypes.Defence) AS Defence, 
	   SUM(statItems.Speed) + MAX(statCharacters.Speed) + MAX(statGamesTypes.Speed) AS Speed,
	   SUM(statItems.Mind) + MAX(statCharacters.Mind) + MAX(statGamesTypes.Mind) AS Mind,
	   SUM(statItems.Luck) + MAX(statCharacters.Luck) + MAX(statGamesTypes.Luck) AS Luck
FROM UsersGames AS ug
JOIN Users AS u ON u.Id = ug.UserId
JOIN Games AS g ON g.Id = ug.GameId
JOIN Characters AS c ON c.Id = ug.CharacterId
JOIN [Statistics] AS statCharacters ON statCharacters.Id = c.StatisticId
JOIN GameTypes AS gt ON gt.Id = g.GameTypeId
JOIN [Statistics] AS statGamesTypes ON statGamesTypes.Id = gt.BonusStatsId
JOIN UserGameItems AS ugi ON ug.Id = ugi.UserGameId
JOIN Items AS i ON i.Id = ugi.ItemId
JOIN [Statistics] AS statItems ON statItems.Id = I.StatisticId
GROUP BY u.Username, g.[Name]
ORDER BY Strength DESC, Defence DESC, Speed DESC, Mind DESC, Luck DESC

--05.All Items with Greater than Average Statistics
SELECT i.[Name], i.Price, i.MinLevel, s.Strength, s.Defence, s.Speed, s.Luck, s.Mind
FROM Items AS i
JOIN [Statistics] AS s ON i.StatisticId = s.Id
WHERE 
      s.Speed > (SELECT AVG(s.Speed) FROM [Statistics] AS s) AND
      s.Luck > (SELECT AVG(s.Luck) FROM [Statistics] AS s) AND
      s.Mind >  (SELECT AVG(s.Mind) FROM [Statistics] AS s)
ORDER BY i.[Name]

--06.Display All Items about Forbidden Game Type
SELECT i.[Name] AS Item, i.Price, i.MinLevel, gt.[Name] AS [Forbidden Game Type]
FROM Items AS i
LEFT JOIN GameTypeForbiddenItems AS gtfi ON gtfi.ItemId = i.Id
LEFT JOIN GameTypes AS gt ON gt.Id = gtfi.GameTypeId
ORDER BY gt.[Name] DESC, i.[Name]

--07.Buy Items for User in Game
--Alex Id = 5;
--EdinburghId = 221
--AlexEdinburghId = 235
--ItemsPrice = 2957.00
--AlexCash = 7659.00

BEGIN TRANSACTION
DECLARE @MyUserId INT = (SELECT Id
                           FROM Users
						   WHERE Username = 'Alex')
DECLARE @MyGameId INT = (SELECT Id
                           FROM Games
						   WHERE [Name] = 'Edinburgh')
DECLARE @MyUserGameId INT = (SELECT Id
                             FROM UsersGames
							 WHERE UserId = @MyUserId AND GameId = @MyGameId)

DECLARE @priceOfTheDesiredItems MONEY = (SELECT SUM(Price) 
                                FROM Items
								WHERE [Name] IN ('Blackguard', 'Bottomless Potion of Amplification', 'Eye of Etlich (Diablo III)', 'Gem of Efficacious Toxin', 'Golden Gorget of Leoric', 'Hellfire Amulet'))
DECLARE @AlexCash MONEY = (SELECT ug.Cash
                           FROM Users AS u 
						   JOIN UsersGames AS ug ON u.Id = ug.UserId
						   JOIN Games AS g ON g.Id = ug.GameId
						   WHERE UserId = @MyUserId 
						   AND GameId = @MyGameId)

    IF (@AlexCash >= @priceOfTheDesiredItems)
    BEGIN
	UPDATE UsersGames 
	SET Cash -= @priceOfTheDesiredItems
	WHERE UserId = @MyUserId AND GameId = @MyGameId

  INSERT INTO UserGameItems (ItemId, UserGameId) 
  (SELECT Id, @MyUserGameId FROM Items WHERE Name IN 
    ('Blackguard', 'Bottomless Potion of Amplification', 'Eye of Etlich (Diablo III)',
    'Gem of Efficacious Toxin', 'Golden Gorget of Leoric', 'Hellfire Amulet')) 
    END
COMMIT

SELECT u.Username, g.[Name], ug.Cash, i.[Name] AS [Item name]
FROM Users AS u
JOIN UsersGames AS ug ON u.Id = ug.UserId
JOIN Games AS g ON g.Id = ug.GameId
JOIN UserGameItems AS ugi ON ug.Id = ugi.UserGameId
JOIN Items AS i ON i.Id = ugi.ItemId
WHERE g.[Name] = 'Edinburgh'
ORDER BY i.[Name]

--08.Peaks and Mountains
USE [Geography]

SELECT p.PeakName, m.MountainRange AS Mountain, p.Elevation
FROM Peaks AS p
JOIN Mountains AS m ON p.MountainId = m.Id
ORDER BY p.Elevation DESC, p.PeakName

--09.Peaks with Mountain, Country and Continent
SELECT p.PeakName, m.MountainRange AS Mountain, coun.CountryName, cont.ContinentName
FROM Peaks AS p
JOIN Mountains AS m ON p.MountainId = m.Id
JOIN MountainsCountries AS mc ON p.MountainId = mc.MountainId
JOIN Countries AS coun ON coun.CountryCode = mc.CountryCode
JOIN Continents AS cont ON coun.ContinentCode = cont.ContinentCode
ORDER BY p.PeakName, coun.CountryName

--10.Rivers by Country
SELECT c.CountryName, cont.ContinentName, COUNT(r.Id) AS RiversCount, ISNULL(SUM(r.[Length]), 0) AS TotalLength
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
LEFT JOIN Continents AS cont ON c.ContinentCode = cont.ContinentCode
GROUP BY c.CountryName, cont.ContinentName
ORDER BY RiversCount DESC, TotalLength DESC, c.CountryName

--11.Count of Countries by Currency
SELECT curr.CurrencyCode, curr.[Description] AS Currency, COUNT(coun.CurrencyCode) AS NumberOfCountries
FROM Currencies AS curr
LEFT JOIN Countries AS coun ON curr.CurrencyCode = coun.CurrencyCode
GROUP BY curr.CurrencyCode, curr.[Description]
ORDER BY NumberOfCountries DESC, curr.[Description]

--12.Population and Area by Continent
SELECT cont.ContinentName, SUM(coun.AreaInSqKm), SUM(CAST(coun.[Population] AS BIGINT)) AS CountriesPopulation
FROM Continents AS cont
JOIN Countries AS coun ON cont.ContinentCode = coun.ContinentCode
GROUP BY cont.ContinentName
ORDER BY CountriesPopulation DESC

--13.Monasteries by Country
--13.1
CREATE TABLE Monasteries
(
Id INT PRIMARY KEY IDENTITY, 
[Name] VARCHAR(100) NOT NULL, 
CountryCode CHAR(2) FOREIGN KEY REFERENCES Countries(CountryCode) NOT NULL,
)

--13.2
INSERT INTO Monasteries(Name, CountryCode) VALUES
('Rila Monastery “St. Ivan of Rila”', 'BG'), 
('Bachkovo Monastery “Virgin Mary”', 'BG'),
('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
('Kopan Monastery', 'NP'),
('Thrangu Tashi Yangtse Monastery', 'NP'),
('Shechen Tennyi Dargyeling Monastery', 'NP'),
('Benchen Monastery', 'NP'),
('Southern Shaolin Monastery', 'CN'),
('Dabei Monastery', 'CN'),
('Wa Sau Toi', 'CN'),
('Lhunshigyia Monastery', 'CN'),
('Rakya Monastery', 'CN'),
('Monasteries of Meteora', 'GR'),
('The Holy Monastery of Stavronikita', 'GR'),
('Taung Kalat Monastery', 'MM'),
('Pa-Auk Forest Monastery', 'MM'),
('Taktsang Palphug Monastery', 'BT'),
('Sümela Monastery', 'TR')

--13.3
ALTER TABLE Countries
ADD IsDeleted BIT NOT NULL
CONSTRAINT DF_IsDeletedDefaultValue
DEFAULT (0)

--13.4
UPDATE Countries
SET IsDeleted = 1
WHERE CountryCode IN (SELECT c.CountryCode
                      FROM Rivers AS r
                      JOIN CountriesRivers AS cr ON r.Id = cr.RiverId
                      JOIN Countries AS c ON cr.CountryCode = c.CountryCode
                      GROUP BY c.CountryCode
					  HAVING COUNT(r.RiverName) > 3)

--13.5
SELECT m.[Name] AS Monastery, coun.CountryName AS Country
FROM Monasteries AS m
JOIN Countries AS coun ON m.CountryCode = coun.CountryCode
WHERE IsDeleted != 1
ORDER BY m.[Name]

--14.Monasteries by Continents and Countries

--14.1
UPDATE Countries
SET CountryName = 'Burma'
WHERE CountryName = 'Myanmar'

--14.2
DECLARE @TanzaniaCountryCode CHAR(2) = (SELECT CountryCode
                                        FROM Countries
                                        WHERE CountryName = 'Tanzania')

INSERT INTO Monasteries([Name], CountryCode) VALUES
('Hanga Abbey', @TanzaniaCountryCode)

--14.3
DECLARE @MyanmarCountryCode CHAR(2) = (SELECT CountryCode
                                        FROM Countries
                                        WHERE CountryName = 'Myanmar')

INSERT INTO Monasteries([Name], CountryCode) VALUES
('Myin-Tin-Daik', @MyanmarCountryCode)

--14.4
SELECT cont.ContinentName, coun.CountryName, COUNT(m.Name) AS MonasteriesCount
FROM Countries AS coun
LEFT JOIN Continents AS cont ON coun.ContinentCode = cont.ContinentCode
LEFT JOIN Monasteries AS m ON m.CountryCode = coun.CountryCode
WHERE coun.IsDeleted = 0
GROUP BY coun.CountryName, cont.ContinentName
ORDER BY COUNT(m.[Name]) DESC, coun.CountryName
