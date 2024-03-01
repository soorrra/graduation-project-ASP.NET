IF OBJECT_ID('BookingPet')
	IS NOT NULL DROP TABLE BookingPet;
IF OBJECT_ID('Booking')
	IS NOT NULL DROP TABLE Booking;
IF OBJECT_ID('Pet')
	IS NOT NULL DROP TABLE Pet;
IF OBJECT_ID('SitterPetType')
	IS NOT NULL DROP TABLE SitterPetType;
IF OBJECT_ID('PetType')
	IS NOT NULL DROP TABLE PetType;
IF OBJECT_ID('SitterAvailability')
	IS NOT NULL DROP TABLE SitterAvailability;
IF OBJECT_ID('Sitter')
	IS NOT NULL DROP TABLE Sitter;
IF OBJECT_ID('Availability')
	IS NOT NULL DROP TABLE Availability;
IF OBJECT_ID('User')
	IS NOT NULL DROP TABLE [User];
IF OBJECT_ID('UserType')
	IS NOT NULL DROP TABLE UserType;
	GO

CREATE TABLE UserType (
	userType				VARCHAR(50) PRIMARY KEY
);

INSERT INTO UserType VALUES ('Admin');
INSERT INTO UserType VALUES ('Sitter');
INSERT INTO UserType VALUES ('Customer');

CREATE TABLE [User] (
	userID					INT PRIMARY KEY IDENTITY (1,1),
	firstName				VARCHAR(50) NOT NULL,
	lastName				VARCHAR(50) NOT NULL,
	phoneNumber				CHAR(10) NOT NULL,
	email					VARCHAR(50) NOT NULL,
	streetAddress			VARCHAR(50) NOT NULL,
	city					VARCHAR(50) NOT NULL,
	postalCode				CHAR(6) NOT NULL,
	userType				VARCHAR(50) NOT NULL,
	profileImage			VARCHAR(8000)
	FOREIGN KEY(userType) REFERENCES UserType(userType)
);

INSERT INTO [User](firstName, lastName, phoneNumber, email, streetAddress, city, postalCode, userType) VALUES ('Admin', 'Admin', '6045551234', 'admin@petsitting.com', '11 Main St', 'Vancouver', 'V6A0A1', 'Admin');
INSERT INTO [User](firstName, lastName, phoneNumber, email, streetAddress, city, postalCode, userType) VALUES ('Doug', 'Sitter', '6046661234', 'doug@gmail.com', '200 Victoria Drive', 'Vancouver', 'V6A1A2', 'Sitter');
INSERT INTO [User](firstName, lastName, phoneNumber, email, streetAddress, city, postalCode, userType) VALUES ('Jane', 'Customer', '6047771234', 'jane@gmail.com', '555 Broadway Ave', 'Vancouver', 'V6A2A3', 'Customer');

CREATE TABLE Sitter (
	sitterID				INT PRIMARY KEY IDENTITY (1,1),
	ratePerPetPerDay		MONEY NOT NULL,
	profileBio				VARCHAR(1000),
	userID					INT NOT NULL,
	FOREIGN KEY(userID) REFERENCES [User](userID)
);

INSERT INTO Sitter(ratePerPetPerDay, profileBio, userID) VALUES ($200, 'I love taking dogs for walks and caring for cats!', 2);

CREATE TABLE PetType (
	petType					VARCHAR(25) PRIMARY KEY,
);

INSERT INTO PetType VALUES ('Dog');
INSERT INTO PetType VALUES ('Cat');

CREATE TABLE SitterPetType (
	sitterID				INT,
	petType					VARCHAR(25),
	PRIMARY KEY(sitterID, petType),
	FOREIGN KEY(sitterID) REFERENCES Sitter(sitterID),
	FOREIGN KEY(petType) REFERENCES PetType(petType)
);

INSERT INTO SitterPetType VALUES (1, 'Dog');
INSERT INTO SitterPetType VALUES (1, 'Cat');

CREATE TABLE Pet (
	petID					INT PRIMARY KEY IDENTITY (1,1),
	[name]					VARCHAR(50) NOT NULL,
	birthYear				INT NOT NULL,
	sex						CHAR(1) NOT NULL,
	petSize					VARCHAR(20) NOT NULL,
	instructions			VARCHAR(2000) NOT NULL,
	userID					INT NOT NULL,
	petType					VARCHAR(25) NOT NULL,
	petImage				VARCHAR(8000)
	FOREIGN KEY(userID) REFERENCES [User](userID),
	FOREIGN KEY(petType) REFERENCES PetType(petType)
);

INSERT INTO Pet([name], birthYear, sex, petSize, instructions, userID, petType) VALUES ('Bella', 2015, 'F', '20-50 lbs', 'Two scoops of kibble morning and night and three walks per day', 3, 'Dog');

CREATE TABLE [Availability] (
	availabilityID			INT PRIMARY KEY IDENTITY (1,1),
	startDate				DATE NOT NULL,
	endDate					DATE NOT NULL
);

INSERT INTO [Availability](startDate, endDate) VALUES ('2022-12-10', '2022-12-15');

CREATE TABLE SitterAvailability (
	sitterID				INT,
	availabilityID			INT,
	PRIMARY KEY(sitterID, availabilityID),
	FOREIGN KEY(sitterID) REFERENCES Sitter(sitterID),
	FOREIGN KEY(availabilityID) REFERENCES [Availability]
);

INSERT INTO SitterAvailability VALUES (1, 1);

CREATE TABLE Booking (
	bookingID				INT PRIMARY KEY IDENTITY (1,1),
	price					MONEY NOT NULL,
	paymentID				VARCHAR(255),
	startDate				DATE NOT NULL,
	endDate					DATE NOT NULL,
	specialRequests			VARCHAR(2000),
	rating					INT,
	review					VARCHAR(2000),
	complaint				VARCHAR(2000),
	sitterID				INT NOT NULL,
	userID					INT NOT NULL
	FOREIGN KEY(sitterID) REFERENCES Sitter(sitterID),
	FOREIGN KEY(userID) REFERENCES [User](userID)
);

INSERT INTO Booking(price, startDate, endDate, specialRequests, sitterID, userID) VALUES ($200, '2022-12-10', '2022-12-12', 'Please give my dog a treat after going for a walk', 1, 3)

CREATE TABLE BookingPet (
	bookingID				INT,
	petID					INT,
	FOREIGN KEY(bookingID) REFERENCES Booking(bookingID),
	FOREIGN KEY(petID) REFERENCES Pet(petID)
);

INSERT INTO BookingPet VALUES (1, 1)

SELECT * FROM UserType
SELECT * FROM [User]
SELECT * FROM Pet
SELECT * FROM PetType
SELECT * FROM Sitter
SELECT * FROM SitterPetType
SELECT * FROM Availability
SELECT * FROM SitterAvailability
SELECT * FROM Booking
SELECT * FROM BookingPet


