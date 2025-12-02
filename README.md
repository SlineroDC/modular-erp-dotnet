# .NET ERP Platform

A general ERP platform for business administration, built with .NET 8 (Razor, API, Blazor) and PostgreSQL. (Hypothetical project name: **Firmeza**).

![License](https://img.shields.io/badge/license-MIT-green)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)

## 📖 About This Project

This project is an administrative application (**S1**) built with **ASP.NET Core Razor Pages**. Its main goal is to manage and update a business's products, customers, and sales from a modern, responsive panel.

This is the first module of a microservice-oriented architecture that includes:
* **S1:** Admin Panel (Razor Pages) - *Completed*
* **S2:** Central REST API (ASP.NET Core) - *Planned*
* **S3:** Client Portal (Blazor) - *Planned*
* **S4:** Unit Tests (xUnit) - *Completed*
* **S5:** Deployment (Docker) - *Completed*

## ✨ Key Features

* **Product Management:** Full CRUD, Soft Delete, Pagination, and **Bulk Import from Excel** (EPPlus).
* **Customer Management:** CRUD with duplicate validation and soft delete.
* **Point of Sale (POS):** Interactive shopping cart to register sales in real-time.
* **Invoicing:** Automatic **PDF Receipt Generation** (QuestPDF).
* **AI Assistant:** Integrated Chat Widget powered by **Google Gemini** (or Ollama) to help users navigate the system.
* **Support System:** Integrated email contact form (SMTP).
* **UI/UX:** Modern interface built with **Tailwind CSS**, fully responsive, with **Dark Mode** support.

## 🛠️ Tech Stack

* **Framework:** .NET 8 (ASP.NET Core Razor Pages)
* **Database:** PostgreSQL (Entity Framework Core)
* **Auth:** ASP.NET Core Identity
* **Frontend:** Tailwind CSS, Vanilla JS
* **Libraries:** EPPlus (Excel), QuestPDF (PDF)
* **Testing:** xUnit
* **Containerization:** Docker & Docker Compose

## 🚀 How to Run (Docker) - Recommended

The easiest way to run the application is using Docker Compose, which sets up the database and app automatically.

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/tu-usuario/dotnet-erp-platform.git](https://github.com/tu-usuario/dotnet-erp-platform.git)
    cd modular-erp-dotnet
    ```

2.  **Run with Docker Compose:**
    ```bash
    docker compose up --build
    ```

3.  **Access the App:**
    Open your browser at `http://localhost:8080`.

> **Note:** The database is created and migrated automatically on startup. A default admin user is also created.

## 💻 How to Run (Local Development)

If you want to run it without Docker (e.g., in Rider or Visual Studio):

1.  **Prerequisites:** .NET 8 SDK, PostgreSQL, Node.js (for Tailwind).
2.  **Configuration:** Update `appsettings.json` in `src/ERP.Admin` with your local Postgres credentials and API Keys.
3.  **Build Frontend:**
    ```bash
    npm install
    npm run build
    ```
4.  **Run Backend:**
    ```bash
    dotnet run --project src/ERP.Admin
    ```

## 🔑 Default Credentials

Use these credentials to log in to the Admin Panel:

| Role | Email | Password |
| :--- | :--- | :--- |
| **Administrator** | `admin@firmeza.com` | `Admin123!` |

## 🧪 Testing

To run the unit tests for the Domain layer:

```bash
dotnet test tests/ERP.Core.Tests