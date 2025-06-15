# 💊 Data Pharma Software

An ASP.NET Web Forms application for managing pharmaceutical vendor data with Entity Framework, Bootstrap, and SQL Server.

---

## 📁 Project Structure

```
/data-pharm-softwere
│
├── content/              → Bootstrap and other static assets (CSS, fonts, etc.)
├── script/               → JavaScript files (e.g., jQuery, custom scripts)
├── migrations/           → Entity Framework migration files
├── Modals/               → EF Models used in data access
├── Pages/                → All .aspx pages and Layout.master
├── web.config            → Main configuration file
├── Global.asax           → Application settings and route mappings
├── packages.config       → NuGet package definitions
└── README.md             → This file
```

---

## 🧩 Packages Used

These NuGet packages are defined in `packages.config` and used in this project:

| Package                                                                                                                                  | Version  | Description                            |
| ---------------------------------------------------------------------------------------------------------------------------------------- | -------- | -------------------------------------- |
| [Bootstrap](https://www.nuget.org/packages/bootstrap/)                                                                                   | 5.3.6    | Front-end framework for UI styling     |
| [BouncyCastle](https://www.nuget.org/packages/BouncyCastle/)                                                                             | 1.8.9    | Cryptographic library                  |
| [EntityFramework](https://www.nuget.org/packages/EntityFramework/)                                                                       | 6.5.1    | ORM for database operations            |
| [iTextSharp](https://www.nuget.org/packages/iTextSharp/)                                                                                 | 5.5.13.3 | PDF generation and manipulation        |
| [jQuery](https://www.nuget.org/packages/jQuery/)                                                                                         | 3.7.1    | JavaScript library for DOM scripting   |
| [Microsoft.CodeDom.Providers.DotNetCompilerPlatform](https://www.nuget.org/packages/Microsoft.CodeDom.Providers.DotNetCompilerPlatform/) | 2.0.1    | Enables C# 6 and Roslyn compiler usage |

---

## 🛠 Installation

### 🔹 Prerequisites

- Visual Studio 2019 or 2022
- .NET Framework 4.7.2
- SQL Server (LocalDB or full)

### 🔹 Setup Steps

1. **Clone the repository**

   ```bash
   git clone https://github.com/mzayem/data-pharm-softwere.git
   cd data-pharm-softwere
   ```

2. **Open the solution** in Visual Studio.

3. **Restore NuGet packages**

   - Right-click on the solution > **Restore NuGet Packages**
   - Or run:
     ```bash
     nuget restore data-pharm-softwere.sln
     ```

4. **Build and run** the project (press `F5` or `Ctrl+F5` in Visual Studio).

---

## 🗄️ Database Setup

1. Create a new SQL Server database named `DataPharma`.
2. Update the connection string in `web.config` file to match your SQL Server instance.
   Example:
   ```xml
   <connectionStrings>
       <add name="DefaultConnection" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DataPharma;Integrated Security=True" providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```
3. Run Entity Framework migrations or use the code-first approach to generate tables.

---

## 🚀 Deployment Guide

### Deploy on IIS

1. Publish your project from Visual Studio (Right-click project > Publish > Folder).
2. Copy the published folder to your IIS server.
3. Add a new site in IIS Manager and point it to your published folder.
4. Ensure Application Pool uses .NET Framework v4.0 or higher.
5. Bind domain and check firewall settings.

### Deploy on Azure (Optional)

1. Create a Web App on Azure Portal.
2. Use the publish profile from Azure in Visual Studio to deploy.
3. Configure database connection string in Azure Configuration settings.

---

## 📤 Features

- Inventory management
- vendor listing and search
- Add, edit, delete vendor, group, and product data
- PDF export of records
- Responsive UI with Bootstrap 5
- Database operations via Entity Framework 6

---

## 📄 License

This project is licensed under the [MIT License](https://github.com/mzayem/data-pharm-softwere/blob/master/LICENSE).

---

## 🙋‍♂️ Author

**Muhammad Zayem**
