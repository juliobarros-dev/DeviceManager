# DeviceManager

[![.NET 9](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
[![Dockerized](https://img.shields.io/badge/Docker-blue)](https://www.docker.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**DeviceManager** is a web application built with C# using a layered architecture, based on principles from Clean Architecture and Domain-Driven Design (DDD).  
The project aims to efficiently manage devices and serves as a solid foundation for scalable enterprise applications.

---

## 🧱 Architecture

The project follows a modular, clean structure:

- **Application.WebApi**: The presentation layer responsible for exposing RESTful APIs.
- **Domain.Models**: Contains entities and value objects representing the core domain.
- **Domain.Services**: Implements business rules and orchestrates domain operations.
- **Infrastructure.Database**: Manages data persistence and database communication.
- **Tests**: Includes unit tests to ensure code quality.

---

## 🚀 Technologies Used

- [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Docker](https://www.docker.com/)
- [xUnit](https://xunit.net/)
- [Swagger / Swashbuckle](https://swagger.io/) for API documentation

---

## ⚙️ Requirements

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/install/) (comes with Docker Desktop)
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download) *(to run the API outside the container)*
- [PostgreSQL](https://www.postgresql.org/) *(to run the API outside the container)*
- Terminal with support to `bash` (`WSL`, Git Bash on Windows or terminal Linux/macOS)

---

## 🧪 How to Run Locally

> All the setup is automated using `setup.sh`.

### 1. Clone the repository
```bash
git clone https://github.com/your-username/DeviceManager.git
cd DeviceManager
```

### 2. Run Setup
```bash
sh setup.sh
```

This script will:

- Spin up the PostgreSQL and API containers
- Wait for PostgreSQL to become ready
- Run the application

### 3. Enjoy Device Manager
```bash
✅ All set! Access: http://localhost:7019/
```

---

## 🧰 Running Outside Docker (Optional)
- Make sure to update the connection string
- Run PostgreSQL
```
cd DeviceManager.Application.WebApi
dotnet run
```


---

## 🌐 Access
Once everything is up and running, access the API via:
```bash
http://localhost:7019/
```

---

## 📂 Project Structure
```pgsql
DeviceManager/
│ 
├── DeviceManager.Application.WebApi/
│ 
├── DeviceManager.Domain.Models/
│ 
├── DeviceManager.Domain.Services/
│ 
├── DeviceManager.Infrastructure.Database/
│ 
├── DeviceManager.Tests.CrossCutting/
│ 
├── DeviceManager.Tests.Unit.Application.WebApi/
│ 
├── Docker/
│   └── Local/
│ 
├── setup.sh
│ 
└── DeviceManager.sln
```

## 🧼 How to Stop Everything
To stop and remove the containers:
```bash
cd Docker/Local/Application
docker-compose -p device_manager_api down

cd ../Infrastructure/Database
docker-compose -p device_manager_postgres down
```
Or use Docker Desktop

---

## 🛠️ Troubleshooting
- Port in use: Make sure port 7019 is not being used by another application.
- Permission denied: If using Unix/macOS, ensure you’ve run:
````
chmod +x setup.sh
````

---

## 📌 Notes
- The API runs with the ASPNETCORE_ENVIRONMENT=Development
- Uses custom network: deviceManager-net
- Database user: postgres, password: admin

---

## Running Tests 🔬

After cloning the repository, follow these steps to restore dependencies and run the tests:

```bash
# 1. Restore dependencies
dotnet restore

# 2. Navigate to the respective test project directory
# E.g.
cd DeviceManager.Tests.Unit.Application.WebApi

# 3. Run the tests
dotnet test
```
---

## 🧑‍💻 Author
Made with ❤️ by Julio Nascimento

---

## 📃 License
MIT
