# 🛍️ E-BangApp

**E-BangApp** is a distributed backend system for an online store, built with a microservices architecture. The project uses **ASP.NET Core** as the main API and separate microservices for email sending, notifications, and file storage in **Azure Blob Storage**. Services communicate asynchronously via **RabbitMQ**. The Notification service uses **SignalR** for real-time communication.

---

## ⚙️ Technologies

- **ASP.NET Core** (API)
- **Docker & Docker Compose**
- **RabbitMQ** (microservices messaging)
- **Azure Blob Storage**
- **SignalR** (real-time notifications)
- **Custom Mediator**, **Entity Framework Core**
- Microservices: Email, Notification, Azure Storage

---

## 📁 Project Structure

```text
E-BangApp/
├── API/src/                 # Main store API (ASP.NET Core)
├── Backend/                 # Backend logic and microservices
│   ├── Email/               # Email sending microservice
│   ├── Notification/        # Notification microservice (uses SignalR)
│   └── AzureStorage/        # Azure Blob Storage microservice
├── Shared/                  # Shared libraries across services
├── docker-compose.yml       # Docker Compose for all services
├── dockerfile.api           # Dockerfile for API
├── dockerfile.email         # Dockerfile for Email service
├── dockerfile.notification  # Dockerfile for Notification service
├── dockerfile.azure         # Dockerfile for Azure Storage service
