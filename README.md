# 🏀 SportsVault API

**SportsVault** is a secure, role-based user management and authentication API built with **ASP.NET Core 8**, **Entity Framework Core**, and **SQL Server**.  
It provides a modular backend foundation for any sports-related or membership-based web platform where user creation, authentication, and role control are required.

---

## ⚙️ Core Features

### **User Management**
- Create, list, and delete users  
- Prevent duplicate email registrations with unique constraints  
- Promote users from **Customer** to **Admin**  
- Retrieve all registered users (**Admin-only**)

### **Authentication & Authorization**
- Email + password login using **JWT access tokens**  
- **BCrypt** password hashing for strong credential security  
- Refresh token rotation mechanism for session renewal  
- Role-based access control via `[Authorize(Roles = "Admin")]`

### **Token System**
- **Access token (JWT):** short-lived, used for authorization  
- **Refresh token:** long-lived, opaque string stored securely in the database  
- Automatic refresh and rotation for enhanced security

### **Error Handling**
- Returns meaningful HTTP responses (`400`, `401`, `403`, `404`, `409`)  
- Integrated **Swagger documentation** with example responses and summaries

### **API Documentation**
- Integrated **Swagger (Swashbuckle)** with JWT Bearer authentication  
- Each endpoint annotated with `[SwaggerOperation]` for clarity

---

## 🧩 Technology Stack

| Layer | Technology |
|-------|-------------|
| **Framework** | ASP.NET Core 8 Web API |
| **ORM** | Entity Framework Core |
| **Database** | SQL Server (LocalDB or full SQL instance) |
| **Security** | JWT, BCrypt password hashing |
| **Docs/UI** | Swagger / OpenAPI |
| **Language** | C# |
| **Hosting** | Cross-platform (Windows/Linux) |

---

## 🔐 Example Endpoints

| Method | Endpoint | Description | Access |
|--------|-----------|--------------|--------|
| **POST** | `/api/v1/User` | Create a new user | Public |
| **POST** | `/api/v1/User/login` | Authenticate user | Public |
| **POST** | `/api/v1/User/refresh-token` | Refresh tokens | Public |
| **GET** | `/api/v1/User/all` | List all users | Admin |
| **PATCH** | `/api/v1/User/promote` | Promote Customer → Admin | Admin |
| **DELETE** | `/api/v1/User/delete` | Delete user by email | Admin |

---

## 🧠 Purpose

**SportsVault** demonstrates a complete, secure authentication and user-management backend suitable for integration with modern web or mobile applications.  
It is ideal as a **template** for enterprise systems, admin dashboards, or any multi-role application requiring account creation, login, and role promotion.
