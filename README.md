# FUMiniHotelSystem - WPF Hotel Management (EF Core Version)

This is a desktop application for managing a mini-hotel system, built for the PRN212 course. This project uses **.NET 10**, **WPF**, and **Entity Framework Core** to connect to a **MS SQL Server** database.

The application provides separate management portals for Admins and Customers, following a strict 3-Layer Architecture and the MVVM design pattern.

## ✨ Features

### 👨‍💼 Admin Portal
* **Authentication:** Login using credentials stored in `appsettings.json`.
* **Customer Management:** Full CRUD (Create, Read, Update, Delete) functionality for customer accounts.
* **Room Management:** Full CRUD functionality. Critically, **Delete** is a soft delete (changes status) if the room is part of any booking, otherwise it's a hard delete.
* **Booking Management:** Full CRUD (Create, Read, Update, Delete) for booking reservations and their details.
* **Booking Reports:** View booking statistics and revenue reports based on a selected date range, sorted in descending order.

### 👩‍💻 Customer Portal
* **Authentication:** Login using customer accounts stored in the SQL database.
* **Book a Room:** Customers can search for available rooms based on check-in/check-out dates and create new bookings.
* **Manage Profile:** Customers can view and update their personal information.
* **View Booking History:** Customers can view a list of all their reservations and expand each one to see the `BookingDetail` (which rooms, dates, and prices).

### 🚀Common Features
* **Logout:** Securely log out of the system to return to the login screen.
* **Styled UI:** A consistent and clean user interface managed by a central WPF Style `ResourceDictionary`.

## 🛠️ Technical Architecture

This project strictly follows the requirements for Assignment 02:

* **Technology:** .NET 10, C#, WPF
* **Database:** MS SQL Server.
* **ORM:** Entity Framework Core (EF Core) for all database operations.
* **Architecture:** 3-Layer Architecture (`WPF UI` -> `BusinessLayer (BLL)` -> `DataAccessLayer (DAL)`).
* **Design Patterns:**
    * **MVVM (Model-View-ViewModel):** Decouples the UI (View) from the logic (ViewModel).
    * **Repository Pattern:** Abstracts the data access logic using interfaces.
    * **DbContext (Unit of Work):** Replaced the Singleton `InMemoryDb` with `FUMiniHotelContext` for database transactions.
* **Database Schema:** Follows the provided schema with `BookingReservation` and `BookingDetail` to allow for multiple rooms per reservation.
* **Querying:** LINQ to Entities for all data retrieval and filtering.
* **Configuration:** The database connection string is loaded from `appsettings.json`.

## 🚀 How to Run

1.  **Clone** this repository.
2.  **Run the SQL Script:** Execute the provided `FUMiniHotelManagement.sql` script in your MS SQL Server instance to create the `FUMiniHotelManagement` database and seed initial data.
3.  **Update Connection String:** Open `WPF/appsettings.json` and ensure the `DefaultConnection` string points to your SQL Server instance.
4.  **Open Solution:** Open the `.slnx` file in Visual Studio (2019 or newer).
5.  **Install SDK:** Ensure you have the **.NET 10 SDK** installed.
6.  **Rebuild Solution:** Right-click the solution and select **Rebuild Solution** to restore all NuGet packages (like EF Core).
7.  **Run:** Set the `WPF` project as the Startup Project and press **F5**. The `Login` window will appear.

### Default Logins

**Admin Login (from `appsettings.json`):**
* **Email:** `admin@FUMiniHotelSystem.com` 
* **Password:** `@@abc123@@` 

**Customer Login (from SQL Script):**
* *(Check the SQL script for customer data, e.g., `a@example.com` / `123456`)*
