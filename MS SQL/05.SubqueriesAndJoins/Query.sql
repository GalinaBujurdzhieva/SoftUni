--01.Employee Address
USE SoftUni

SELECT TOP(5) EmployeeID, JobTitle, e.AddressID, AddressText
FROM Employees AS e
LEFT JOIN Addresses AS a
ON e.AddressID = a.AddressID
ORDER BY AddressID

--02.Addresses with Towns
SELECT TOP(50) e.FirstName, e.LastName, t.[Name] AS Town, a.AddressText 
FROM Employees AS e
LEFT JOIN Addresses AS a
ON e.AddressID = a.AddressID
LEFT JOIN Towns AS t
ON a.TownID = t.TownID
ORDER BY e.FirstName, e.LastName

--03.Sales Employees
SELECT e.EmployeeID, e.FirstName, e.LastName, d.[Name] AS 'DepartmentName'
FROM Employees AS e
LEFT JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE d.[Name] = 'Sales'
ORDER BY e.EmployeeID

--04.Employee Departments
SELECT TOP(5) e.EmployeeID, e.FirstName, e.Salary, d.[Name] AS DepartmentName
FROM Employees AS e
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE e.Salary > 15000
ORDER BY d.DepartmentID ASC

--05.Employees Without Projects
SELECT TOP(3) e.EmployeeID, e.FirstName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
ON e.EmployeeID = ep.EmployeeID
WHERE ep.EmployeeID IS NULL
ORDER BY e.EmployeeID

--06.Employees Hired After
SELECT e.FirstName, e.LastName, e.HireDate, d.[Name] AS DeptName
FROM Employees AS e
LEFT JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE DATEPART(YEAR, HireDate) >= 1999 AND (d.[Name] IN ('Sales', 'Finance'))
ORDER BY e.HireDate

--07.Employees With Project
SELECT TOP(5) e.EmployeeID, e.FirstName, p.[Name] AS ProjectName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects AS p
ON ep.ProjectID = p.ProjectID
WHERE p.StartDate > '2002-08-13' AND p.EndDate IS NULL
ORDER BY e.EmployeeID

--08.Employee 24
SELECT e.EmployeeID, e.FirstName,
CASE
    WHEN DATEPART(YEAR, p.[StartDate]) < 2005 THEN p.[Name]
	ELSE NULL
	END AS ProjectName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects AS p
ON ep.ProjectID = p.ProjectID
WHERE e.EmployeeID = 24

--09.Employee Manager
SELECT e.EmployeeID, e.FirstName, e.ManagerID, m.FirstName AS ManagerName
FROM Employees AS e
LEFT JOIN Employees AS m
ON e.ManagerID = m.EmployeeID
WHERE m.EmployeeID IN (3, 7)
ORDER BY e.EmployeeID

--10.Employees Summary
SELECT TOP(50) e.EmployeeID, e.FirstName + ' ' + e.LastName AS EmployeeName, m.FirstName + ' ' + m.LastName AS ManagerName, d.[Name] AS DepartmentName
FROM Employees AS e
LEFT JOIN Employees AS m
ON e.ManagerID = m.EmployeeID
LEFT JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID

--11.Min Average Salary
SELECT TOP(1) AVG(e.Salary) AS MinAverageSalary
FROM Employees AS e
GROUP BY e.DepartmentID
ORDER BY AVG(e.Salary)

--12.Highest Peaks in Bulgaria
USE [Geography]

SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation
FROM MountainsCountries AS mc
LEFT JOIN Mountains AS m
ON mc.MountainId = m.Id
LEFT JOIN Peaks AS p
ON m.Id = p.MountainId
WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
ORDER BY p.Elevation DESC

--13.Count Mountain Ranges
SELECT c.CountryCode, COUNT(*) 
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m
ON mc.MountainId = m.Id
GROUP BY c.CountryCode
HAVING c.CountryCode IN('BG', 'RU', 'US')

--14.Countries With or Without Rivers
SELECT TOP(5) c.CountryName, r.RiverName
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr
ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers AS r
ON cr.RiverId = r.Id
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName

--15.Continents and Currencies
SELECT Sub.ContinentCode, Sub.CurrencyCode, Sub.CurrencyCount AS Total
FROM 
(SELECT ContinentCode, CurrencyCode, COUNT(CurrencyCode) AS CurrencyCount, 
DENSE_RANK() OVER (PARTITION BY ContinentCode ORDER BY COUNT(CurrencyCode) DESC) AS CurencyRanking
FROM Countries AS c
GROUP BY c.ContinentCode, c.CurrencyCode
) 
AS Sub
WHERE Sub.CurencyRanking = 1 AND Sub.CurrencyCount != 1
ORDER BY Sub.ContinentCode

--16.Countries Without any Mountains
SELECT COUNT(*) AS [Count]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
WHERE mc.MountainId IS NULL

--17.Highest Peak and Longest River by Country
SELECT TOP(5) c.CountryName, MAX(p.Elevation) AS HighestPeakElevation, MAX(r.[Length]) AS LongestRiverLength
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m
ON mc.MountainId = m.Id
LEFT JOIN Peaks AS p
ON m.Id = p.MountainId
LEFT JOIN CountriesRivers AS cr
ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers AS r
ON cr.RiverId = r.Id
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC, c.CountryName

--18.Highest Peak Name and Elevation by Country
SELECT TOP(5) CountryName AS Country,
      ISNULL(PeakName, '(no highest peak)') AS [Highest Peak Name],
	  ISNULL(Elevation, 0) AS [Highest Peak Elevation],
	  ISNULL(MountainRange, '(no mountain)') AS Mountain
FROM
(
SELECT c.CountryName, p.PeakName, p.Elevation, m.MountainRange, DENSE_RANK() OVER (PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS Ranking
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m
ON mc.MountainId = m.Id
LEFT JOIN Peaks AS p
ON m.Id = p.MountainId
)
AS [Peak Ranking Subquery]
WHERE Ranking = 1
ORDER BY Country, [Highest Peak Name]
