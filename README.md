# FlowTask API

A modern task management REST API built with .NET 8, following Clean Architecture principles and designed for scalability and performance.

## Architecture

The project is built following Clean Architecture principles and is divided into layers:

- **Domain** — domain entities, business rules, and domain exceptions.
- **Application** — business logic, services, interfaces, DTOs, validation, and use cases.
- **Infrastructure** — repository implementations, database access (Entity Framework Core), migrations, and external services.
- **Web** — ASP.NET Core Web API, controllers, middleware, dependency injection, and Swagger documentation.

## Main Technologies and Tools

- **.NET 8** — modern development platform
- **Entity Framework Core** — ORM for database access with PostgreSQL
- **ASP.NET Core Identity** — user authentication and authorization
- **JWT Bearer** — token-based authentication
- **Mapster** — DTO ↔ Entity mapping
- **FluentValidation** — DTO validation
- **SequentialGuid** — GUID generation for entities
- **Swagger** — auto-generated API documentation
- **xUnit, Moq, Shouldly** — unit testing framework and mocking
- **Docker** — containerization and deployment
- **PostgreSQL** — primary database
- **Memory Caching** — performance optimization
- **CI-CD** — Automation of testing and deployment

## Features

- JWT-based authentication
- Task CRUD operations with user isolation
- Advanced filtering and sorting
- Pagination support
- In-memory caching for performance
- Comprehensive validation
- Global exception handling
- Swagger documentation
- Unit test coverage
- CI-CD pipeline
- Docker containerization
- PostgreSQL database with optimized indexes

## CI/CD Pipeline

The project includes a comprehensive **GitHub Actions CI/CD pipeline** that automates the entire development workflow:

- **Automated Testing** — Runs unit tests on every push and pull request
- **Code Coverage** — Generates and tracks test coverage reports
- **Docker Build** — Automatically builds and pushes Docker images to DockerHub
- **Multi-branch Support** — Triggers on master, dev, and feature branches
- **Quality Gates** — Ensures code quality before deployment

The pipeline ensures code reliability and enables seamless deployment to production environments.

## Why Clean Architecture?

Clean Architecture was chosen for its **scalability**, **maintainability**, and **testability**. The clear separation of concerns allows for easy extension, independent testing of each layer, and the ability to swap out implementations without affecting the core business logic.

## Caching Strategy

The project implements **Cache-Aside** pattern for optimal performance. This strategy perfectly fits the task management domain since users typically don't have 10,000+ tasks, making in-memory caching highly effective for reducing database queries and significantly improving API response times.

## API Endpoints

### Authentication
- `POST /api/v1/users/register` — Register a new user
- `POST /api/v1/users/login` — Login and get JWT token

### Tasks (Requires Authentication)
- `GET /api/v1/tasks` — Get paginated tasks with optional filtering
- `GET /api/v1/tasks/{id}` — Get specific task by ID
- `POST /api/v1/tasks` — Create a new task
- `PUT /api/v1/tasks/{id}` — Update existing task
- `DELETE /api/v1/tasks/{id}` — Delete task

### Query Parameters for GET /api/v1/tasks
- `pageNumber` — Page number (default: 1)
- `pageSize` — Items per page (default: 10)
- `status` — Filter by task status (Pending, InProgress, Completed)
- `priority` — Filter by priority (Low, Medium, High)
- `dueDate` — Filter by due date
- `sortBy` — Sort by field (duedate, priority)
- `sortDescending` — Sort direction (default: false)

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- IDE (Visual Studio, Rider, or VS Code)
- Docker and Docker Compose
- PostgreSQL (if running locally)

### Using Docker (Recommended)

1. Clone the repository
2. Navigate to the project directory
3. Run the application:
   ```bash
   docker-compose up -d
   ```

The application will be available at:
- API: `http://localhost:5248` or `https://localhost:7048`
- Swagger UI: `http://localhost:5248/swagger`
- PgAdmin: `http://localhost:8080` (admin@flowtask.com / admin123)

### Local Development

1. Clone the repository
2. Update connection string in `appsettings.Development.json`
3. Run database migrations:
   ```bash
   dotnet ef database update --project FlowTask.Infrastructure --startup-project FlowTask.Web
   ```
4. Run the application:
   ```bash
   dotnet run --project FlowTask.Web
   ```

### Testing

Run unit tests:
```bash
dotnet test
```