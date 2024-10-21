CREATE TABLE FlatDetails (
    FlatNumber INT PRIMARY KEY,
    Name NVARCHAR(100),
    PhoneNumber NVARCHAR(15),
    PaymentStatus NVARCHAR(50)
);

CREATE TABLE ContactUsMessages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Message NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE HouseRentals (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    HouseNumber NVARCHAR(50) NOT NULL,
    HouseRent DECIMAL(18, 2) NOT NULL,
    Date DATETIME NOT NULL
);

CREATE TABLE Houselists (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incrementing ID
    Name NVARCHAR(100) NOT NULL,       -- Flat/House name
    HouseNumber NVARCHAR(50) NOT NULL, -- House number or identifier
    HouseRent DECIMAL(10, 2) NOT NULL, -- Rent price, up to 10 digits with 2 decimal places
    CreatedAt DATETIME DEFAULT GETDATE() -- Timestamp for when the record was created
);

CREATE TABLE UserRegistrationDBd (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incrementing primary key
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,  -- Store hashed passwords
    DateOfBirth DATE NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    ApartmentName VARCHAR(100),
    Flatno VARCHAR(20),
    Pincode VARCHAR(10)
);

CREATE TABLE dbo.Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    TenantName NVARCHAR(100),
    ApartmentNumber NVARCHAR(50),
    RentAmount DECIMAL(18, 2),
    MaintenanceCharges DECIMAL(18, 2),
    GasCharges DECIMAL(18, 2),
    WaterCharges DECIMAL(18, 2),
    ElectricityCharges DECIMAL(18, 2),
    TotalAmount DECIMAL(18, 2),
    PaymentDate DATETIME DEFAULT GETDATE()
);

	