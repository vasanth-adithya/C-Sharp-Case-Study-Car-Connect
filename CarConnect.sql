create database CarConnect;

drop database CarConnect

use CarConnect;

-- Customer Table
CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(20),
    Address VARCHAR(255),
    Username VARCHAR(50) UNIQUE,
    Password VARCHAR(255), -- Assuming hashed passwords are 255 characters long
    RegistrationDate DATE
);

-- Vehicle Table
CREATE TABLE Vehicle (
    VehicleID INT PRIMARY KEY IDENTITY(1,1),
    Model VARCHAR(50),
    Make VARCHAR(50),
    Year INT,
    Color VARCHAR(50),
    RegistrationNumber VARCHAR(20) UNIQUE,
    Availability BIT,
    DailyRate DECIMAL(10, 2) -- Assuming a decimal for daily rate with 2 decimal places
);

-- Reservation Table
CREATE TABLE Reservation (
    ReservationID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    VehicleID INT,
    StartDate DATE,
    EndDate DATE,
    TotalCost DECIMAL(10, 2), -- Assuming a decimal for total cost with 2 decimal places
    Status VARCHAR(20),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) on delete cascade,
    FOREIGN KEY (VehicleID) REFERENCES Vehicle(VehicleID) on delete cascade
);

-- Admin Table
CREATE TABLE Admin (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(20),
    Username VARCHAR(50) UNIQUE,
    Password VARCHAR(255), -- Assuming hashed passwords are 255 characters long
    Role VARCHAR(50),
    JoinDate DATE
);


-- Inserting sample data into the Customer table
INSERT INTO Customer (FirstName, LastName, Email, PhoneNumber, Address, Username, Password, RegistrationDate)
VALUES
    ('Vasanth', 'Adithya', 'vasanthadithya@email.com', '7555117359', 'Gajwel', 'Vasanth', 'vasanth password', '2023-01-01'),
    ('Sharath', 'Chandra', 'sharath.chandra@email.com', '915557405', 'Pregnapur', 'Sharath', 'sharath_password', '2023-02-15'),
    ('Srinivas', 'Vadakattu', 'srinivas.vadakattu@email.com', '7555082410', 'Kukatpally', 'Srinivas', 'srinivas_password', '2023-03-20'),
    ('Gopi', 'Krishna', 'gopi.krishna@email.com', '7555867836', 'Chandanagar', 'Gopi', 'gopi_password', '2023-04-10'),
    ('Koushik', 'Gundu', 'koushik.gundu@email.com', '8555613691', 'Karimnagar', 'Koushik', 'koushik_password', '2023-05-05');

-- Inserting sample data into the Vehicle table
INSERT INTO Vehicle (Model, Make, Year, Color, RegistrationNumber, Availability, DailyRate)
VALUES
    ('Supra', 'Toyota', 2022, 'Blue', 'TS36L5555', 1, 50.00),
    ('Accord', 'Honda', 2021, 'Red', 'TS18E4444', 0, 55.00),
    ('Mustang', 'Ford', 2023, 'Yellow', 'TS01Z1111', 1, 70.00),
    ('Civic', 'Honda', 2022, 'Silver', 'TS23B3333', 1, 60.00),
    ('Verna', 'Hyundai', 2021, 'Black', 'TS30J8888', 0, 55.00);

-- Inserting sample data into the Reservation table
INSERT INTO Reservation (CustomerID, VehicleID, StartDate, EndDate, TotalCost, Status)
VALUES
    (1, 3, '2023-02-01', '2023-02-05', 280.00, 'Confirmed'),
    (2, 1, '2023-03-10', '2023-03-15', 275.00, 'Pending'),
    (3, 5, '2023-04-20', '2023-04-25', 275.00, 'Completed'),
    (4, 2, '2023-05-05', '2023-05-10', 330.00, 'Confirmed'),
    (5, 4, '2023-06-15', '2023-06-20', 300.00, 'Pending');

-- Inserting sample data into the Admin table
INSERT INTO Admin (FirstName, LastName, Email, PhoneNumber, Username, Password, Role, JoinDate)
VALUES
    ('Krishna', 'Chaithanya', 'krishna@email.com', '7555243680', 'Krishna', 'krishna_password', 'Super Admin', '2023-01-01'),
    ('Tharun', 'Bhaskar', 'tharun.bhaskar@email.com', '7555501515', 'Tharun', 'tharun_password', 'Fleet Manager', '2023-02-15'),
    ('Karunakar', 'Reddy', 'karunakar.reddy@email.com', '7555151104', 'Karunakar', 'karunakar_password', 'Super Admin', '2023-03-20'),
    ('Sathwik', 'Soma', 'sathwik.soma@email.com', '9055506264', 'Sathwik', 'sathwik_password', 'Fleet Manager', '2023-04-10'),
    ('Nikhil', 'Artham', 'nikhil.artham@email.com', '9755503858', 'Nikhil', 'nikhil_password', 'Super Admin', '2023-05-05');


select * from Customer
select * from Admin
select * from vehicle
select * from Reservation
