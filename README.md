# Project Setup Guide

## Software Requirements

Before running this project, ensure the following software is installed:

### Development Environment

* **Visual Studio 2022 (Windows)**
  https://visualstudio.microsoft.com/vs/community/

* **JetBrains Rider (macOS)**
  https://www.jetbrains.com/rider/download/?section=mac

### Frameworks & SDKs

* **.NET 8.0 SDK**
  https://dotnet.microsoft.com/en-us/download/dotnet/8.0

* **ASP.NET Core MVC 8.0**

* **Entity Framework Core 8.0**

### Database

* **Microsoft SQL Server**
  https://www.microsoft.com/en-us/sql-server/sql-server-downloads

* **SQL Server Management Studio (SSMS)**
  https://learn.microsoft.com/en-us/ssms/install/install

### Operating System

* Windows 10 / Windows 11
* macOS (with .NET 8.0 installed)

### Web Browser

Any modern browser, including:

* Google Chrome
* Microsoft Edge
* Mozilla Firefox
* Safari

---

# Installation

### 1. Extract Project Files

Extract the project files to a local directory on your machine.

### 2. Open the Solution

Open the `.sln` file using Visual Studio 2022 or JetBrains Rider.

### 3. Verify .NET Installation

Ensure that the .NET 8.0 SDK is installed:

```bash
dotnet --version
```

### 4. Restore NuGet Packages

Restore all required packages:

**Visual Studio**

```
Build → Restore NuGet Packages
```

Or via CLI:

```bash
dotnet restore
```

### 5. Configure Database Connection

Open:

```text
appsettings.Development.json
```

Verify the connection string under:

```json
"ConnectionStrings": {
  "DefaultConnection": "Your SQL Server Connection String"
}
```

### 6. Create / Update Database

Open **Package Manager Console** and run:

```powershell
Update-Database -Verbose
```

Alternatively, using the .NET CLI:

```bash
dotnet ef database update
```

### 7. Build the Solution

**Visual Studio**

```
Build → Build Solution
```

or press:

```
Ctrl + Shift + B
```

### 8. Run the Application

**Visual Studio**

```
Debug → Start Without Debugging
```

or press:

```
Ctrl + F5
```

### 9. Open in Browser

The application will automatically launch in your default web browser.

---

# Database Setup

This application uses:

* Microsoft SQL Server
* Entity Framework Core 8.0
* Code-First Migrations

Database creation and updates are handled automatically through Entity Framework Core migrations.

## Steps

### 1. Open Package Manager Console

In Visual Studio:

```
Tools → NuGet Package Manager → Package Manager Console
```

### 2. Verify EF Core Packages

Ensure Entity Framework Core 8.0 packages are installed in the relevant projects.

### 3. Set Startup Project

Set the appropriate startup project before running migrations.

### 4. Apply Migrations

```powershell
Update-Database
```

### 5. Database Creation

Entity Framework Core will automatically:

* Create the database
* Create all required tables
* Configure relationships and constraints
* Apply migration scripts

### 6. Verify Database

Open SQL Server Management Studio (SSMS) and confirm that the database and tables have been created successfully.

---

# Running the Application

Follow these steps to start the application:

1. Open the project in Visual Studio 2022 or Rider.
2. Ensure SQL Server is running.
3. Execute `Update-Database` if the database has not yet been created.
4. Build the solution.
5. Run the application.
6. The application will open automatically in your default browser or can be accessed via the generated localhost URL.
7. Log in using the default credentials below.
8. Explore the application's features through the web interface.

---

# Default Login Credentials

The application uses Entity Framework Core seed data to generate initial records during first-time setup.

An administrator account is automatically created when the database is initialized.

## Administrator Account

| Field    | Value                         |
| -------- | ----------------------------- |
| Username | `dhirendrasaithwar@gmail.com` |
| Password | `admin123`                    |

> **Note:** These credentials are intended for development, testing, and assessment purposes only. It is strongly recommended to change the default password in production environments.

---

# Technology Stack

* ASP.NET Core MVC 8.0
* Entity Framework Core 8.0
* Microsoft SQL Server
* C#
* Razor Views
* Bootstrap
* .NET 8.0

---

# Troubleshooting

### Migration Errors

Ensure:

* SQL Server is running.
* Connection strings are correct.
* Entity Framework Core packages are installed.

### Build Errors

Try:

```bash
dotnet restore
dotnet build
```

### Database Connection Issues

Verify the connection string in:

```text
appsettings.Development.json
```

and confirm that SQL Server is accessible.

---

# License

This project is provided for educational and academic purposes.
