CREATE DATABASE Bitbucket
GO

USE Bitbucket
--01.DDL
CREATE TABLE Users
(
Id INT PRIMARY KEY IDENTITY,
Username VARCHAR(30) NOT NULL,
[Password] VARCHAR(30) NOT NULL,
Email VARCHAR(50) NOT NULL
)

CREATE TABLE Repositories
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE RepositoriesContributors
(
RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
PRIMARY KEY (RepositoryId, ContributorId)
)

CREATE TABLE Issues
(
Id INT PRIMARY KEY IDENTITY,
Title VARCHAR(255) NOT NULL,
IssueStatus VARCHAR(6) NOT NULL,
CHECK(LEN(IssueStatus) <= 6),
RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
AssigneeId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Commits 
(
Id INT PRIMARY KEY IDENTITY, 
[Message] VARCHAR(255) NOT NULL,
IssueId INT FOREIGN KEY REFERENCES Issues(Id),
RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Files
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(100) NOT NULL,
Size DECIMAL(38,2) NOT NULL,
ParentId INT FOREIGN KEY REFERENCES Files(Id),
CommitId INT FOREIGN KEY REFERENCES Commits(Id) NOT NULL
)

--DML
--02.Insert
INSERT INTO Files VALUES
('Trade.idk', 2598.0, 1, 1),
('menu.net', 9238.31, 2, 2),
('Administrate.soshy', 1246.93, 3, 3),
('Controller.php', 7353.15, 4, 4),
('Find.java', 9957.86, 5, 5),
('Controller.json', 14034.87, 3, 6),
('Operate.xix', 7662.92, 7, 7)

INSERT INTO Issues VALUES
('Critical Problem with HomeController.cs file', 'open', 1, 4),
('Typo fix in Judge.html', 'open', 4, 3),
('Implement documentation for UsersService.cs', 'closed', 8, 2),
('Unreachable code in Index.cs', 'open', 9, 8)

--03. Update
UPDATE Issues
SET IssueStatus = 'closed'
WHERE AssigneeId = 6

--04.Delete
DELETE FROM RepositoriesContributors
WHERE RepositoryId = (SELECT Id 
                      FROM Repositories
                      WHERE [Name] = 'Softuni-Teamwork')

DELETE FROM Issues
WHERE RepositoryId = (SELECT Id 
                      FROM Repositories
                      WHERE [Name] = 'Softuni-Teamwork')

--Querying
--05.Commits
SELECT Id, [Message], RepositoryId, ContributorId
FROM Commits
ORDER BY Id, [Message], RepositoryId, ContributorId

--06.Front-end
SELECT Id, [Name], Size
FROM Files
WHERE Size > 1000 AND [Name] LIKE '%html%'
ORDER BY Size DESC, Id, [Name]

--07.Issue Assignment
SELECT i.Id, u.Username + ' : ' + i.Title AS IssueAssignee
FROM Issues AS i
JOIN Users AS u ON i.AssigneeId = u.Id
ORDER BY i.Id DESC, u.Username

--08.Single Files
SELECT p.Id, p.[Name], CAST(p.Size AS VARCHAR(20)) + 'KB' AS Size
FROM Files AS f
RIGHT JOIN Files AS p ON f.ParentId = p.Id
WHERE f.ParentId IS NULL
ORDER BY p.Id, p.[Name], p.Size DESC

--09.Commits in Repositories
SELECT TOP (5) r.Id, r.[Name], COUNT(*) AS Commits
FROM Commits AS c
JOIN Repositories AS r ON c.RepositoryId = r.Id
JOIN RepositoriesContributors AS rc ON r.Id = rc.RepositoryId
GROUP BY r.Id, r.[Name]
ORDER BY COUNT(*) DESC, r.Id, r.[Name]

--10.Average Size
SELECT u.Username, AVG(f.Size) AS Size
FROM Commits AS c
JOIN Users AS u ON c.ContributorId = u.Id
JOIN Files AS f ON c.Id = f.CommitId
GROUP BY u.Username
ORDER BY AVG(f.Size) DESC, u.Username
GO

--11.All User Commits
CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(30)) 
RETURNS INT
AS
BEGIN
DECLARE @commitsCount INT = (
                             SELECT COUNT(*) AS [Output]
                             FROM Users AS u
                             JOIN Commits AS c ON c.ContributorId = u.Id
							 WHERE u.Username = @username
                             GROUP BY u.Username
							 )
RETURN ISNULL(@commitsCount, 0)
END
GO

SELECT dbo.udf_AllUserCommits('UnveiledDenoteIana')
GO

--12.Search for Files
CREATE PROC usp_SearchForFiles(@fileExtension VARCHAR(10))
AS
BEGIN
      SELECT Id, [Name], CAST(Size AS VARCHAR(20)) + 'KB' AS Size
      FROM Files
      WHERE [Name] LIKE '%' + @fileExtension
      ORDER BY Id, [Name], Size DESC
END

EXEC usp_SearchForFiles 'txt'