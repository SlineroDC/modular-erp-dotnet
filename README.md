# .NET ERP Platform

A general ERP platform for business administration, built with .NET (Razor, API, Blazor) and PostgreSQL. (Hypothetical project name: Firmeza).

## About This Project

This project is an administrative application (S1) built with ASP.NET Razor Pages. Its main goal is to manage and update a business's products and clients from a Razor panel, including data import/export via Excel files.

This is the first module of a microservice-oriented platform that will also include:
* **S1:** Admin Panel (Razor Pages)
* **S2:** Central REST API (ASP.NET Core)
* **S3:** Client Portal (Blazor)
* **S4:** Unit Tests (xUnit) 
* **S5:** Deployment (Docker) 

## Tech Stack (S1)

* .NET 8 (or higher) 
* ASP.NET Core Razor Pages 
* Entity Framework Core 
* PostgreSQL 
* ASP.NET Core Identity 
* EPPlus (Excel Import/Export) 
* QuestPDF / iTextSharp (PDF Generation) 

## How to Run (Fase 1)

1. Clone the repository: `git clone https://github.com/tu-usuario/dotnet-erp-platform.git`
2. Open `dotnet-erp-.sln` in Rider or Visual Studio.
3. Update the connection string in `appsettings.json` in the Admin project. 
4. Run the database migrations: `dotnet ef database update` 
5. Run the Admin project.
