--1.Create Database--
CREATE DATABASE Minions;

--2.Create Tables--
CREATE TABLE Minions 
(
Id INT PRIMARY KEY,
[Name] VARCHAR(30) NOT NULL, 
Age INT,
)

CREATE TABLE Towns 
(
Id INT PRIMARY KEY, 
[Name] VARCHAR(30) NOT NULL,
)

--3.Alter Minions Table--
ALTER TABLE Minions
ADD TownId INT

ALTER TABLE Minions
ADD FOREIGN KEY (TownId) REFERENCES Towns(Id)

--4.Insert Records in Both Tables--
INSERT INTO Towns VALUES
(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna')

INSERT INTO Minions VALUES
(1, 'Kevin', 22, 1),
(2, 'Bob', 15, 3),
(3, 'Steward', NULL, 2)

--5.Truncate Table Minions--
DELETE FROM Minions

--6.Drop All Tables--
DROP TABLE Minions
DROP TABLE Towns

--7.Create Table People--
CREATE TABLE People 
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(200) NOT NULL,
Picture VARBINARY(MAX),
Height DECIMAL(5,2),
[Weight] DECIMAL(5,2),
Gender CHAR(1) NOT NULL CHECK(Gender='m' OR Gender='f'),
Birthdate DATE NOT NULL,
Biography NVARCHAR(MAX)
)

INSERT INTO People ([Name], Picture, Height, [Weight], Gender, Birthdate, Biography) VALUES
('Galina', NULL, 1.68, 68.00, 'f', '1984-11-30', 'Likes cycling and programming.'),
('Daria', NULL, 1.55, 55, 'f', '2008-01-20', 'Likes motorcycles and music.'),
('Georgi', NULL, 1.80, 90, 'm', '1969-11-06', 'Likes cars.'),
('Rumiana', NULL, 1.75, 85, 'f', '1959-04-15', 'Likes reading books.'),
('Max', NULL, 0.30, 11, 'm', '2015-11-21', 'Likes running, barking and eating.')

--8.Create Table Users--
CREATE TABLE Users
(
Id BIGINT PRIMARY KEY IDENTITY,
Username VARCHAR(30) NOT NULL,
[Password] VARCHAR(26) NOT NULL,
ProfilePicture VARBINARY(MAX),
LastLoginTime DATETIME,
IsDeleted BIT NOT NULL,
)

--YYYY-MM-DD HH:MM:SS
INSERT INTO Users (Username, [Password], ProfilePicture, LastLoginTime, IsDeleted) VALUES
('Galina', '200108', NULL, '2022-03-09 10:51:19', 0),
('Asya', '3058792484', NULL, '2022-04-15 21:17:58', 0),
('Tatyana', 'Evricom', NULL, '2021-08-31 15:40:30', 1),
('Konstantin', 'I_love_cycling', NULL, '2022-05-01 20:15:45', 0),
('RegularUser', 'Nevermind', NULL, '2022-01-01 12:12:12', 0)

--9.Change Primary Key--
ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC07F14500AB

ALTER TABLE Users
ADD CONSTRAINT PK_IdUsername PRIMARY KEY (Id, Username)

--10.Add Check Constraint--
ALTER TABLE Users
ADD CONSTRAINT CH_PasswordIsAtLeastFiveSymbols
CHECK(LEN([Password]) >= 5)

--11.Set Default Value of a Field--
ALTER TABLE Users
ADD CONSTRAINT DF_LastLoginTimeDateTimeNow
DEFAULT GETDATE() FOR LastLoginTime

--12.Set Unique Field--
ALTER TABLE Users
DROP CONSTRAINT PK_IdUsername

ALTER TABLE Users
ADD CONSTRAINT PK_Id PRIMARY KEY (Id)

ALTER TABLE Users
ADD CONSTRAINT CH_UsernameIsAtLeastThreeSymbols
CHECK(LEN(Username) >= 3)

--13.Movies Database--
CREATE DATABASE Movies

USE Movies
CREATE TABLE Directors
(
Id INT PRIMARY KEY,
DirectorName VARCHAR(100) NOT NULL,
Notes VARCHAR(MAX)
)

CREATE TABLE Genres
(
Id INT PRIMARY KEY,
GenreName VARCHAR(100) NOT NULL,
Notes VARCHAR(MAX)
)

CREATE TABLE Categories
(
Id INT PRIMARY KEY,
CategoryName VARCHAR(100) NOT NULL,
Notes VARCHAR(MAX)
)

--Id, Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes
CREATE TABLE Movies
(
Id INT PRIMARY KEY,
Title VARCHAR(500) NOT NULL,
DirectorId INT NOT NULL,
CopyrightYear INT NOT NULL,
[Length] TIME NOT NULL,
GenreId INT NOT NULL,
CategoryId INT NOT NULL,
Rating INT,
Notes VARCHAR(MAX)
)

--Directors (Id, DirectorName, Notes)
INSERT INTO Directors VALUES
(18121946, 'Steven Spielberg', 'An American film director, producer and screenwriter.'),
(17111942, 'Martin Charles Scorsese', 'Also famous American film director, producer and screenwriter.'),
(27031963, 'Quentin Jerome Tarantino', 'An American filmmaker, actor, film critic and author.'),
(30071970, 'Christopher Nolan', 'A British-American film director, producer and screenwriter.'),
(07041939, 'Francis Ford Coppola', 'Another very famous American film director, producer and screenwriter')

--Genres (Id, GenreName, Notes)
INSERT INTO Genres VALUES
(15, 'Action', 'An action story is similar to adventure, and the protagonist usually takes a risky turn, which leads to desperate situations (including explosions, fight scenes, daring escapes, etc.).' ),
(3, 'Adventure', 'An adventure story is about a protagonist who journeys to epic or distant places to accomplish something. It can have many other genre elements included within it, because it is a very open genre.'),
(99, 'Comedy', 'Comedy is a story that tells about a series of funny, or comical events, intended to make the audience laugh. It is a very open genre, and thus crosses over with many other genres on a frequent basis.'),
(25, 'Crime and mystery', 'A crime story is often about a crime that is being committed or was committed, but can also be an account of a criminals life. A mystery story follows an investigator as they attempt to solve a puzzle (often a crime). The details and clues are presented as the story continues and the protagonist discovers them and by the end of the story the mystery is solved.'),
(5, 'Fantasy', 'A fantasy story is about magic or supernatural forces, as opposed to technology as seen in science fiction. Depending on the extent of these other elements, the story may or may not be considered to be a "hybrid genre" series')

--Categories (Id, CategoryName, Notes)
INSERT INTO Categories VALUES
(77, 'Historical', NULL),
(8, 'Horror', 'A horror story is told to deliberately scare or frighten the audience, through suspense, violence or shock.'),
(13, 'Romance', 'Most often, a romance is understood to be "love stories", emotion-driven stories that are primarily focused on the relationship between the main characters of the story. Beyond the focus on the relationship, the biggest defining characteristic of the romance genre is that a happy ending is always guaranteed, perhaps marriage and living "happily ever after", or simply that the reader sees hope for the future of the romantic relationship.'),
(54, 'Science fiction', 'Science fiction (once known as scientific romance) is similar to fantasy, except stories in this genre use scientific understanding to explain the universe that it takes place in. It generally includes or is centered on the presumed effects or ramifications of computers or machines; travel through space, time or alternate universes; alien life-forms; genetic engineering; or other such things. The science or technology used may or may not be very thoroughly elaborated on.'),
(1, 'Thriller', 'A thriller is a story that is usually a mix of fear and excitement. It has traits from the suspense genre and often from the action, adventure or mystery genres, but the level of terror makes it borderline horror fiction at times as well. It generally has a dark or serious theme, which also makes it similar to drama.')

--Movies(Id, Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes)
INSERT INTO Movies VALUES
(1, 'Indiana Jones and the Kingdom of the Crystal Skull', 18121946, 2008, '02:02:15', 3, 19, 9, 'An American action-adventure film directed by Steven Spielberg and the fourth installment in the Indiana Jones series.'),
(5, 'Bridge of Spies', 18121946, 2015, '02:22:49', 13, 77, 8, NULL),
(24, 'Titanic', 16081954, 1997, '03:14:30', 77, 13, 10, 'An American epic romance and disaster film directed, written, produced, and co-edited by James Cameron. Incorporating both historical and fictionalized aspects, it is based on accounts of the sinking of the RMS Titanic, and stars Leonardo DiCaprio and Kate Winslet as members of different social classes who fall in love aboard the ship during its ill-fated maiden voyage.'),
(66, 'Once Upon a Time in Hollywood', 27031963, 2019, '02:51:02', 99, 17, 8, 'A co-production between the United States, United Kingdom, and China.'),
(7, 'Justice League', 30071970, 2017, '2:00:33', 3, 54, 7, 'An American superhero film based on the DC Comics superhero team of the same name. It is produced by Warner Bros. Pictures, RatPac-Dune Entertainment, Atlas Entertainment, and Cruel and Unusual Films and distributed by Warner Bros. Pictures.')

--14.Car Rental Database--
CREATE DATABASE CarRental

--Categories (Id, CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
CREATE TABLE Categories
(
Id INT PRIMARY KEY,
CategoryName VARCHAR(100) NOT NULL,
DailyRate INT,
WeeklyRate INT,
MonthlyRate INT,
WeekendRate INT
)

--Cars (Id, PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available)
CREATE TABLE Cars
(
Id INT PRIMARY KEY,
PlateNumber VARCHAR(15) NOT NULL,
Manufacturer VARCHAR(50) NOT NULL,
Model VARCHAR(50) NOT NULL,
CarYear INT NOT NULL,
CategoryId INT NOT NULL,
Doors INT NOT NULL,
Picture VARBINARY(MAX),
Condition VARCHAR(15) NOT NULL,
Available BIT NOT NULL
)

--Employees (Id, FirstName, LastName, Title, Notes)
CREATE TABLE Employees
(
Id INT PRIMARY KEY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
Title VARCHAR(50) NOT NULL,
Notes VARCHAR(MAX)
)

--Customers (Id, DriverLicenceNumber, FullName, Address, City, ZIPCode, Notes)
CREATE TABLE Customers
(
Id INT PRIMARY KEY,
DriverLicenceNumber INT NOT NULL,
FullName VARCHAR(100) NOT NULL,
[Address] VARCHAR(MAX),
City VARCHAR(100),
ZIPCode INT,
Notes VARCHAR(MAX)
)

--RentalOrders (Id, EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, TotalKilometrage, StartDate, EndDate, TotalDays, RateApplied, TaxRate, OrderStatus, Notes)
CREATE TABLE RentalOrders
(
Id INT PRIMARY KEY,
EmployeeId INT NOT NULL,
CustomerId INT NOT NULL,
CarId INT NOT NULL,
TankLevel INT,
KilometrageStart INT NOT NULL,
KilometrageEnd INT NOT NULL,
TotalKilometrage INT NOT NULL,
StartDate DATE NOT NULL,
EndDate DATE NOT NULL,
TotalDays INT NOT NULL,
RateApplied INT,
TaxRate INT NOT NULL,
OrderStatus VARCHAR(50) NOT NULL,
Notes VARCHAR(MAX)
)

--Categories (Id, CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
INSERT INTO Categories VALUES
(3, 'Sport', NULL, 4, 6, 9),
(14, 'Family', 2, NULL, NULL, NULL),
(23, 'Classic', 6, 7, 8, 10)

--Cars (Id, PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available)
INSERT INTO Cars VALUES
(7, 'CT1857PP', 'Mitsubishi', 'Lancer', 2003, 14, 5, NULL, 'Used', 1),
(1, 'CT2133VN', 'Subaru', 'Impreza', 2008, 3, 5, NULL, 'Used', 0),
(15, 'CT1217CA', 'BMW', 'M4', 2021, 3, 3, NULL, 'New', 1)

--Employees (Id, FirstName, LastName, Title, Notes)
INSERT INTO Employees VALUES
(78, 'Rumen', 'Georgiev', 'Еxpert customer relationship', NULL),
(95, 'Ivan', 'Dimitrov', 'Employee', NULL),
(101, 'Valentin', 'Popov', 'Manager', NULL)

--Customers (Id, DriverLicenceNumber, FullName, Address, City, ZIPCode, Notes)
INSERT INTO Customers VALUES
(551, 984895622, 'Galina Bujurdzhieva', 'Vasil Levski 26', 'Kazanlak', 6100, NULL),
(777, 126597661, 'Georgi Valkov', '23-ti Pehoten Shipchenski Polk 68', 'Stara Zagora', 6000, NULL),
(1001, 456549871, 'Tatyana Pavlova', 'General Stoletov 32', 'Sliven', 8800, NULL)

--RentalOrders (Id, EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, TotalKilometrage, StartDate, EndDate, TotalDays, RateApplied, TaxRate, OrderStatus, Notes)
INSERT INTO RentalOrders VALUES
(458, 78, 551, 7, 25, 216524, 217874, 1350, '2022-03-09', '2022-03-29', 20, NULL, 700, 'Complete', 'Good client'),
(823, 95, 777, 1, 0, 136158, 136558, 400, '2021-07-08', '2021-07-12', 4, 9, 450, 'Complete', 'Good client'),
(1287, 101, 1001, 15, 33, 1500, 1800, 300, '2022-04-25', '2022-04-26', 1, 10, 900, 'Complete', 'Good client')

--15.Hotel Database--
CREATE DATABASE Hotel

--Employees (Id, FirstName, LastName, Title, Notes)
CREATE TABLE Employees
(
Id INT PRIMARY KEY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
Title VARCHAR(50) NOT NULL,
Notes VARCHAR(MAX)
)

--Customers (AccountNumber, FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes)
CREATE TABLE Customers
(
AccountNumber VARCHAR(50) PRIMARY KEY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
PhoneNumber VARCHAR(50) NOT NULL,
EmergencyName VARCHAR(50) NOT NULL,
EmergencyNumber VARCHAR(50) NOT NULL,
Notes VARCHAR(MAX)
)

--RoomStatus (RoomStatus, Notes)
CREATE TABLE RoomStatus
(
RoomStatus VARCHAR(10) PRIMARY KEY,
Notes VARCHAR(MAX)
)

--RoomTypes (RoomType, Notes)
CREATE TABLE RoomTypes
(
RoomTypes VARCHAR(10) PRIMARY KEY,
Notes VARCHAR(MAX)
)

--BedTypes (BedType, Notes)
CREATE TABLE BedTypes
(
BedTypes VARCHAR(10) PRIMARY KEY,
Notes VARCHAR(MAX)
)

--Rooms (RoomNumber, RoomType, BedType, Rate, RoomStatus, Notes)
CREATE TABLE Rooms
(
RoomNumber INT PRIMARY KEY,
RoomType VARCHAR(10) NOT NULL,
BedType VARCHAR(10) NOT NULL,
Rate INT,
RoomStatus VARCHAR(10) NOT NULL,
Notes VARCHAR(MAX)
)

--Payments (Id, EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, TotalDays, AmountCharged, TaxRate, TaxAmount, PaymentTotal, Notes)
CREATE TABLE Payments
(
Id INT PRIMARY KEY,
EmployeeId INT NOT NULL,
PaymentDate DATE NOT NULL,
AccountNumber VARCHAR(50) NOT NULL,
FirstDateOccupied DATE,
LastDateOccupied DATE,
TotalDays INT,
AmountCharged DECIMAL(38,2) NOT NULL,
TaxRate DECIMAL(38,2) NOT NULL,
TaxAmount DECIMAL(38,2) NOT NULL,
PaymentTotal DECIMAL(38,2) NOT NULL,
Notes VARCHAR(MAX)
)

--Occupancies (Id, EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge, Notes)
CREATE TABLE Occupancies
(
Id INT PRIMARY KEY,
EmployeeId INT NOT NULL,
DateOccupied DATE NOT NULL,
AccountNumber VARCHAR(50) NOT NULL,
RoomNumber INT NOT NULL,
RateApplied INT,
PhoneCharge DECIMAL(38,2),
Notes VARCHAR(MAX)
)

--Employees (Id, FirstName, LastName, Title, Notes)
INSERT INTO Employees VALUES
(78, 'Rumen', 'Georgiev', 'Еxpert customer relationship', NULL),
(95, 'Ivan', 'Dimitrov', 'Employee', NULL),
(101, 'Valentin', 'Popov', 'Manager', NULL)

--Customers (AccountNumber, FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes)
INSERT INTO Customers VALUES
('BGSF2359652967', 'Galina', 'Bujurdzhieva', '359898464437', 'Vasil Petkov', '359877482315', NULL),
('DNBR525236236235', 'Jorg', 'Gotthard', '00497082-942953', 'Susi Gotthard', '00497082-942716', NULL),
('UKLO813469852145', 'Andrew', 'Sphikas', '00447545047167', 'Renata Williams', '00447545047168', NULL)

--RoomStatus (RoomStatus, Notes)
INSERT INTO RoomStatus VALUES
('Free', NULL),
('Occupied', NULL),
('Cleaning', NULL)

--RoomTypes (RoomType, Notes)
INSERT INTO RoomTypes VALUES
('Single', NULL),
('Double', NULL),
('Apartment', NULL)

--BedTypes (BedType, Notes)
INSERT INTO BedTypes VALUES
('Standard', NULL),
('Full', NULL),
('King', NULL)

--Rooms (RoomNumber, RoomType, BedType, Rate, RoomStatus, Notes)
INSERT INTO Rooms VALUES
(17, 'Single', 'Standard', 7, 'Cleaning', NULL),
(1, 'Apartment', 'King', 9, 'Occupied', NULL),
(31, 'Double', 'Full', 5, 'Free', NULL)

--Payments (Id, EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, TotalDays, AmountCharged, TaxRate, TaxAmount, PaymentTotal, Notes)
INSERT INTO Payments VALUES
(185, 78, '2022-02-18', 'BGSF2359652967', '2015-01-18', '2022-04-25', 1548, 458.76, 3.5, 65.42, 457.39, NULL),
(731, 101, '2022-04-30', 'UKLO813469852145', '2017-07-08', '2022-05-01', 996, 843.17, 5.16, 89.05, 658.78, NULL),
(222, 95, '2022-01-18', 'DNBR525236236235', '2015-02-03', '2022-04-15', 2001, 657.18, 4.44, 76.17, 555.55, NULL)

--Occupancies (Id, EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge, Notes)
INSERT INTO Occupancies VALUES
(5, 95, '2022-04-15', 'DNBR525236236235', 17, NULL, NULL, NULL),
(7, 101, '2022-05-01', 'UKLO813469852145', 1, 6, 45.14, NULL),
(9, 78, '2022-04-25', 'BGSF2359652967', 31, 8, 11.01, NULL)

--16.Create SoftUni Database--
CREATE DATABASE SoftUni

--Towns (Id, Name)
CREATE TABLE Towns
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(100) NOT NULL,
)

--Addresses (Id, AddressText, TownId)
CREATE TABLE Addresses
(
Id INT PRIMARY KEY IDENTITY,
AddressText VARCHAR(MAX) NOT NULL,
TownId INT FOREIGN KEY REFERENCES Towns(Id)
)

--Departments (Id, Name)
CREATE TABLE Departments
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(100) NOT NULL
)

--Employees (Id, FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId)
CREATE TABLE Employees
(
Id INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(50) NOT NULL,
MiddleName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
JobTitle VARCHAR(100) NOT NULL,
DepartmentId INT FOREIGN KEY REFERENCES Departments(Id),
HireDate DATE NOT NULL, 
Salary DECIMAL(38,2) NOT NULL, 
AddressId INT FOREIGN KEY REFERENCES Addresses(Id)
)

--17.Backup Database--
--18.Basic Insert--

--Sofia, Plovdiv, Varna, Burgas
INSERT INTO Towns VALUES
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

--Engineering, Sales, Marketing, Software Development, Quality Assurance
INSERT INTO Departments VALUES
('Engineering'),
('Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance')

--Employees (Id, FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId)
/*
Name	               Job Title	       Department	          Hire Date	    Salary
Ivan Ivanov Ivanov	   .NET Developer	   Software Development	  01/02/2013	3500.00
Petar Petrov Petrov	   Senior Engineer	   Engineering	          02/03/2004	4000.00
Maria Petrova Ivanova  Intern	           Quality Assurance	  28/08/2016	525.25
Georgi Teziev Ivanov   CEO	               Sales	              09/12/2007	3000.00
Peter Pan Pan	       Intern	           Marketing	          28/08/2016	599.88
*/

INSERT INTO Employees (FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary) 
VALUES
('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, '2013-02-01', 3500.00),
('Petar', 'Petrov', 'Petrov', 'Senior Engineer', 1, '2004-03-02', 4000.00),
('Maria', 'Petrova', 'Ivanova', 'Intern', 5, '2016-08-28', 525.25),
('Georgi', 'Teziev', 'Ivanov', 'CEO', 2, '2007-12-09', 3000.00),
('Peter', 'Pan', 'Pan', 'Intern', 3, '2016-08-28', 599.88)

--19.Basic Select All Fields--
SELECT * FROM Towns

SELECT * FROM Departments

SELECT * FROM Employees

--20.Basic Select All Fields and Order Them--
SELECT * FROM Towns
ORDER BY [Name] ASC

SELECT * FROM Departments
ORDER BY [Name] ASC

SELECT * FROM Employees
ORDER BY Salary DESC

--21.Basic Select Some Fields--
SELECT [Name] FROM Towns
ORDER BY [Name] ASC

SELECT [Name] FROM Departments
ORDER BY [Name] ASC

SELECT FirstName, LastName, JobTitle, Salary FROM Employees
ORDER BY Salary DESC

--22. Increase Employees Salary--
UPDATE Employees
SET Salary *= 1.10

SELECT Salary FROM Employees

--23.Decrease Tax Rate--
UPDATE Payments
SET TaxRate *= 0.97

SELECT TaxRate FROM Payments

--24.Delete All Records--
DELETE FROM Occupancies

