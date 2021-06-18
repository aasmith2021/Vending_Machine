/* Deployment script for VendingMachine */
USE master;
GO

IF DB_ID('VendingMachine') IS NOT NULL
BEGIN
	ALTER DATABASE VendingMachine SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE VendingMachine;
END
GO

CREATE DATABASE VendingMachine;
GO

USE VendingMachine
GO

CREATE TABLE inventory
(
	id				INT				NOT NULL IDENTITY(1,1),
	slot_number		VARCHAR (32)	NOT NULL,
	item_name		VARCHAR (50)	NOT NULL,
	item_price		MONEY			NOT NULL,
	item_category	VARCHAR (32)	NOT NULL,
	CONSTRAINT PK_inventory PRIMARY KEY (id),
	CONSTRAINT UQ_inventory_unique_slot_number UNIQUE (slot_number)
);

CREATE TABLE transaction_log
(
	id						INT				NOT NULL IDENTITY(1,1),
	transaction_date_time	DATETIME		NOT NULL,
	transaction_name		VARCHAR (72)	NOT NULL,
	balance_before			MONEY			NOT NULL,
	balance_after			MONEY			NOT NULL,
	CONSTRAINT PK_log PRIMARY KEY (id)
);

CREATE TABLE sales_report
(
	id						INT				NOT NULL IDENTITY(1,1),
	report_date_time		DATETIME		NOT NULL,
	item_id					INT				NOT NULL,
	quantity_sold			INT				NOT NULL,
	total_sales_for_report	MONEY			NOT NULL,
	CONSTRAINT PK_sales_report PRIMARY KEY (id),
	CONSTRAINT FK_sales_report_inventory FOREIGN KEY (item_id) REFERENCES inventory(id)
);

INSERT INTO inventory
VALUES
	('A1', 'Potato Crisps', 3.05, 'Chip'),
	('A2', 'Stackers', 1.45, 'Chip'),
	('A3', 'Grain Waves', 2.75, 'Chip'),
	('A4', 'Cloud Popcorn', 3.65, 'Chip'),
	('B1', 'Moonpie', 1.80, 'Candy'),
	('B2', 'Cowtales', 1.50, 'Candy'),
	('B3', 'Wonka Bar', 1.50, 'Candy'),
	('B4', 'Crunchie', 1.75, 'Candy'),
	('C1', 'Cola', 1.25, 'Drink'),
	('C2', 'Dr. Salt', 1.50, 'Drink'),
	('C3', 'Mountain Melter', 1.50, 'Drink'),
	('C4', 'Heavy', 1.50, 'Drink'),
	('D1', 'U-Chews', 0.85, 'Gum'),
	('D2', 'Little League Chew', 0.95, 'Gum'),
	('D3', 'Chiclets', 0.75, 'Gum'),
	('D4', 'Triplemint', 0.75, 'Gum');
