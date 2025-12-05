# 🏗️ Firmeza ERP Platform

A comprehensive, modular ERP solution for the construction industry, built with **.NET 8**, **Vue.js**, and **PostgreSQL**, following **Clean Architecture** principles.

![License](https://img.shields.io/badge/license-MIT-green)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Vue.js](https://img.shields.io/badge/Vue.js-3.0-green)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)

## 📖 Overview

**Firmeza** is an administrative and commercial ecosystem designed to manage products, customers, and sales efficiently. It consists of three integrated modules:

1.  **ERP.Admin (Backoffice):** A server-side rendered web application (Razor Pages) for administrators to manage inventory, users, and view business metrics.
2.  **ERP.Api (Backend Core):** A robust RESTful API that serves as the central logic hub, handling authentication (JWT), business rules, and data persistence.
3.  **ERP.Client (Storefront):** A modern SPA (Single Page Application) built with Vue 3, allowing customers to browse the catalog, manage their cart, and place orders.

## ✨ Key Features

### 🏢 Admin Module (S1)
* **Dashboard:** Real-time metrics for products, customers, and sales.
* **Product Management:** Full CRUD with soft delete and **Bulk Import via Excel**.
* **Customer Management:** Administration of client data with duplicate validation.
* **Reporting:** Automatic **PDF Receipt Generation** and sales history.
* **AI Assistant:** Integrated Chat Widget (powered by Google Gemini) for system navigation and support.
* **Support System:** Email-based support ticket system.

### 🌐 API Module (S2)
* **Security:** JWT (JSON Web Token) Authentication & Authorization.
* **Documentation:** Full API documentation via **Swagger UI**.
* **Architecture:** Hexagonal/N-Layer architecture separating Domain, Infrastructure, and Presentation.
* **Services:** Centralized services for Email (SMTP), PDF (QuestPDF), and AI.

### 🛍️ Client Module (S4)
* **Storefront:** Responsive catalog with "Liquid Glass" UI design.
* **Shopping Cart:** Real-time cart management using **Pinia** state store.
* **User Experience:** Dark/Light mode toggle, animated interactions, and seamless checkout.
* **Customer Portal:** Profile management and order history.

## 🛠️ Tech Stack

| Layer | Technology |
| :--- | :--- |
| **Backend** | .NET 8, ASP.NET Core Web API, Razor Pages |
| **Frontend** | Vue.js 3, Vite, Tailwind CSS v4 |
| **Database** | PostgreSQL, Entity Framework Core |
| **Auth** | ASP.NET Core Identity, JWT Bearer |
| **Tools** | Docker, Docker Compose, Swagger, xUnit |
| **Integrations** | Google Gemini (AI), EPPlus (Excel), QuestPDF (PDF) |

## 🚀 How to Run (Docker) - Recommended

Run the entire platform (Database + Admin + API + Client) with a single command.

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/tu-usuario/modular-erp-dotnet.git](https://github.com/tu-usuario/modular-erp-dotnet.git)
    cd modular-erp-dotnet
    ```

2.  **Set up Environment Variables:**
    Ensure your `docker-compose.yml` or `.env` has valid API Keys for:
    * `AiSettings__ApiKey` (Google Gemini)
    * `EmailSettings__Password` (Gmail App Password)

3.  **Launch:**
    ```bash
    docker compose up --build
    ```

4.  **Access the Modules:**
    * **Storefront (Client):** [http://localhost:3000](http://localhost:3000)
    * **Backoffice (Admin):** [http://localhost:8080](http://localhost:8080)
    * **API Docs (Swagger):** [http://localhost:5000/swagger](http://localhost:5000/swagger)

## 💻 How to Run (Local Development)

### Prerequisites
* .NET 8 SDK
* Node.js (v18+)
* PostgreSQL (Running on default port or Docker)

### 1. Database & Migrations
Update `appsettings.json` in `src/ERP.Admin` and `src/ERP.Api` with your local connection string.
```bash
dotnet ef database update -p src/ERP.Infrastructure -s src/ERP.Admin
```
### 2. Start Backend (API & Admin)
Run both projects simultaneously using your IDE (Rider/VS) or separate terminals:
```bash
# Terminal 1 (API)
dotnet run --project src/ERP.Api

# Terminal 2 (Admin)
dotnet run --project src/ERP.Admin
```

### 3.Start Frontend (Client)
```bash
cd src/ERP.Client
npm install
npm run dev
```
### 🔑 Default Credentials
Administrator (Backoffice):
- Email: admin@firmeza.com
- Password: Admin123!

Customer (Storefront):
- Register a new account via the Client App or use the API to create one.

### 🧪 Testing
Run unit tests for the Core Domain logic, Api and client:
```bash
# Domain logic test
dotnet test tests/ERP.Core.Tests
```

```bash
# Api test
dotnet test tests/ERP.Api.Tests
```

```bash
# Client logic test
npm run test src/ERP.Client
```