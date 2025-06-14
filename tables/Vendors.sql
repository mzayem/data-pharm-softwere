﻿CREATE TABLE Vendors (
    VendorID INT PRIMARY KEY IDENTITY(1001,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150),
    Address NVARCHAR(250) NOT NULL,
    Town NVARCHAR(100) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    LicenceNo NVARCHAR(50),
    ExpiryDate DATE NOT NULL,
    SRACode VARCHAR(20) NOT NULL,
    GstNo VARCHAR(20) NOT NULL,
    NtnNo VARCHAR(20) NOT NULL,
    CompanyCode VARCHAR(20) NOT NULL,
    MaxDiscountAllowed DECIMAL(5,2) CHECK (MaxDiscountAllowed BETWEEN 0 AND 100.00) NOT NULL,
    PartyType NVARCHAR(50),
    Remarks NVARCHAR(250) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
