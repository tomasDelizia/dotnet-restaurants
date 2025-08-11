# Restaurants Project

This guide will help you set up the .NET environment for the Restaurants project, including initializing user-secrets and configuring connection strings and API keys.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed
- Git installed

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/tomasDelizia/dotnet-restaurants.git
cd Restaurants
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Initialize User-Secrets

User-secrets allow you to store sensitive information (like connection strings and API keys) securely during development.

```bash
dotnet user-secrets init
```

### 4. Add Connection Strings

Replace `<YourConnectionString>` with your actual SQL Server database connection string.

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<YourConnectionString>"
```

### 5. Add License Key

Replace `<YourLicenseKey>` with your actual License key from [LuckyPennySoftware](https://luckypennysoftware.com/).

```bash
dotnet user-secrets set "LuckyPennySoftware:LicenseKey" "<YourLicenseKey>"
```



### 6. Run the Application

```bash
dotnet run
```

## Notes

- User-secrets are only available in development and are not deployed.
- For production, use environment variables or a secure secrets manager.

## Useful Links

- [Microsoft Docs: User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
