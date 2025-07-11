# 📘 Student Progress Tracker API

A robust and scalable REST API backend for tracking student learning progress across multiple subjects and activities. Built with ASP.NET Core and Entity Framework Core, this API is designed for educational platforms needing advanced reporting, authentication, and performance.

---

## 🚀 Features

- ✅ RESTful API with Entity Framework Core and SQL Server
- 🔐 JWT Authentication using ASP.NET Core Identity
- 👥 Role-Based Access Control (RBAC) for Admins and Teachers
- 📊 Analytics: Class Summary, Progress Trends, CSV Exports
- 📦 Global Exception Handling with structured JSON responses
- 🧼 Data Sanitization and Validation
- 🚦 Rate Limiting Simulation
- 🩺 Health Check Ready (optional)
- 📈 Logging Strategy using `ILogger` & optional Serilog
- 🗂️ Modular Repositories & Services with Generic Pattern
- 🔁 API Versioning with backward compatibility

---

## 📁 Project Structure

```
/Controllers
/Services
/Repositories
/Models
/DTOs
/Middleware
/Data
```

---

## 🔐 Authentication & RBAC

- Uses **ASP.NET Core Identity** with `Guid` as `UserId`
- JWT token issued on login (`/api/auth/login`)
- Profile endpoint returns authenticated user data
- Roles: `Admin`, `Teacher` (can be seeded)

### Seed Roles Example:

```csharp
await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
await roleManager.CreateAsync(new IdentityRole<Guid>("Teacher"));
await userManager.AddToRoleAsync(user, "Teacher");
```

---

## 🛡️ Global Exception Handling

Implemented via `ExceptionHandlingMiddleware.cs`.

### Example Response:
```json
{
  "error": {
    "message": "An unexpected error occurred.",
    "details": "SqlException: Cannot open database..."
  }
}
```

- All errors logged via `ILogger`
- Returns appropriate HTTP status codes: 400, 401, 403, 404, 500

---

## 🧼 Input Validation & Sanitization

- DTOs use `[Required]`, `[StringLength]`, `[EmailAddress]`, etc.
- Invalid input returns `400 Bad Request` with validation messages
- Sanitization: trimming and normalizing user inputs before processing

---

## 🔃 API Versioning

### Enabled with:
```csharp
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version")
    );
});
```

### Example Endpoints:
- `/api/v1/students`
- `/api/v2/students`

Swagger UI shows versions if configured.

---

## 📈 Logging Strategy

### Uses `ILogger<T>`:
- Logs all important service-level events, warnings, and errors
- Error-level logs in `ExceptionHandlingMiddleware`
- Optional: Integrate [Serilog](https://serilog.net) for file or database logging

### Example:
```csharp
_logger.LogInformation("Fetching student with ID: {StudentId}", id);
_logger.LogWarning("Student not found: {StudentId}", id);
```

---

## 📉 Rate Limiting Simulation

You can use:

- ✅ `.NET 7+` built-in `RateLimiter` middleware
- OR custom middleware like:

```csharp
if (requestCount >= 5)
{
    context.Response.StatusCode = 429;
    context.Response.Headers["Retry-After"] = "30";
}
```

Protects APIs from abuse, especially login or export endpoints.

---

## 🛠️ Database Failure Handling

- Catches `SqlException` or `DbUpdateException`
- Returns `503 Service Unavailable` with clear error message
- Automatically retries if configured:

```csharp
options.UseSqlServer(connectionString, sqlOptions =>
{
    sqlOptions.EnableRetryOnFailure(3);
});
```

---

## 🧪 Testing Strategy

- ✅ Unit Tests using xUnit
- ✅ Integration Tests for key endpoints
- ✅ Postman Collection provided (optional)
- ✅ Seeded mock data: 20+ students across K–12

---

## 📊 Reporting & Analytics Endpoints

| Endpoint                         | Description                    |
|----------------------------------|--------------------------------|
| `GET /api/analytics/class-summary` | Class-level performance summary |
| `GET /api/analytics/progress-trends` | View student trends over time |
| `GET /api/reports/student-export` | Export student data as CSV     |

Supports pagination, sorting, and filtering via query strings.

---

## 🧱 Technologies Used

- .NET 8 (or latest LTS)
- Entity Framework Core
- SQL Server / SQLite
- JWT & ASP.NET Core Identity
- Swagger (OpenAPI)
- Optional: Redis, Serilog, Polly, Docker

---

## ✅ Security Highlights

- ✅ JWT Authentication
- ✅ Role-based authorization
- ✅ Input validation
- ✅ Structured error responses
- ✅ SQL Injection prevention via EF Core
- ✅ Data sanitization
- ✅ Rate limiting & throttling
- ✅ CORS configuration

---

## 📦 API Usage Examples

### Authentication

```http
POST /api/auth/login
{
  "email": "teacher1@example.com",
  "password": "P@ssw0rd"
}
```

### Get Students

```http
GET /api/v1/students?grade=5&searchTerm=Ali&page=1&pageSize=10
```

### Export CSV

```http
GET /api/reports/student-export
Accept: text/csv
```

---

## 🧠 Tips for Deployment

- Use **AppSettings.Production.json** for DB connection and JWT keys
- Add **health checks** to monitor database/API uptime
- Use **Docker** or **Kubernetes** for scalable deployments
- Connect **Redis** for caching frequently used data

---

## 📄 License

MIT License
