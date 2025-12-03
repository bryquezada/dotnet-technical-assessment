# Employee Management System

Technical assessment demonstrating .NET 8 Web API with CRUD operations, JWT authentication, and multiple UI implementations using Clean Architecture and Repository Pattern.

## Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

- **Domain Layer**: Core business entities and repository interfaces with no external dependencies
- **Application Layer**: DTOs and service contracts
- **Infrastructure Layer**: Concrete implementations including in-memory repositories and JWT authentication services
- **Presentation Layer**: RESTful API and two UI implementations (Razor Pages and Blazor WebAssembly)

## Technologies

- .NET 8
- ASP.NET Core Web API
- ASP.NET Core Razor Pages
- Blazor WebAssembly
- MudBlazor Component Library
- JWT Bearer Authentication
- Swagger/OpenAPI
- In-Memory Data Storage

## Prerequisites

- .NET 8 SDK
- Any modern web browser

## Project Structure

```
src/
├── EmployeeManagement.API/          # RESTful API with Swagger
├── EmployeeManagement.Application/  # DTOs and interfaces
├── EmployeeManagement.Domain/       # Entities and repository interfaces
├── EmployeeManagement.Infrastructure/ # Repository and service implementations
├── EmployeeManagement.RazorUI/      # Razor Pages UI
└── EmployeeManagement.BlazorUI/     # Blazor WebAssembly UI
```

## Running the Application

### 1. Start the API

```bash
cd src/EmployeeManagement.API
dotnet run
```

The API will be available at `http://localhost:5008` with Swagger UI at `http://localhost:5008/swagger`

### 2. Start Razor Pages UI (Optional)

```bash
cd src/EmployeeManagement.RazorUI
dotnet run
```

The Razor UI will be available at `http://localhost:5280`

### 3. Start Blazor WebAssembly UI (Optional)

```bash
cd src/EmployeeManagement.BlazorUI
dotnet run
```

The Blazor UI will be available at `http://localhost:5025`

## Testing the Application

### API Testing with Swagger

1. Navigate to `http://localhost:5008/swagger`
2. Test authentication endpoint:
   - Expand `POST /api/auth/login`
   - Click "Try it out"
   - Use credentials: `{"username": "admin", "password": "admin123"}`
   - Click "Execute"
   - Copy the token from the response
3. Authorize Swagger:
   - Click the "Authorize" button at the top
   - Enter: `Bearer YOUR_TOKEN_HERE`
   - Click "Authorize"
4. Test protected endpoints:
   - `GET /api/auth/users` - Returns list of users
   - `GET /api/employees` - Returns list of employees
   - `POST /api/employees` - Create new employee
   - `PUT /api/employees/{id}` - Update employee
   - `DELETE /api/employees/{id}` - Delete employee

### Razor Pages UI Testing

1. Navigate to `http://localhost:5280`
2. Click "Login" button
3. Enter credentials:
   - Username: `admin`
   - Password: `admin123`
4. After successful login, you will be redirected to the Users page
5. The navbar will display your username and a Logout button

### Blazor WebAssembly UI Testing

1. Navigate to `http://localhost:5025`
2. Click "Get Started" or "Login" button
3. Enter credentials:
   - Username: `admin`
   - Password: `admin123`
4. After successful login, you will be redirected to the Users page
5. The navbar will display your username and a Logout button

## Test Credentials

The system includes three pre-configured users:

| Username | Password  | Role  |
|----------|-----------|-------|
| admin    | admin123  | Admin |
| user1    | user123   | User  |
| test     | test123   | User  |

## API Endpoints

### Authentication

- `POST /api/auth/login` - Authenticate and receive JWT token
- `GET /api/auth/users` - Get all users (requires authentication)

### Employees

- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update existing employee
- `DELETE /api/employees/{id}` - Delete employee

## Features

### Backend
- RESTful API with full CRUD operations
- JWT Bearer authentication
- Role-based authorization
- In-memory data storage with seed data
- Comprehensive error handling
- Input validation with data annotations
- CORS configuration for cross-origin requests
- Swagger/OpenAPI documentation

### Razor Pages UI
- Modern gradient theme
- Session-based authentication
- Responsive layouts
- Form validation
- Protected routes
- Conditional UI rendering

### Blazor WebAssembly UI
- MudBlazor component library
- Custom authentication state provider
- Local storage JWT persistence
- Glassmorphism design effects
- Single-page application experience
- Real-time form validation

## Security Notes

This is a technical assessment implementation with simplified security for demonstration purposes:

- Passwords are stored in plain text (production systems should use hashing)
- JWT secret is in appsettings.json (production should use User Secrets or Key Vault)
- CORS is enabled for all origins (production should restrict origins)
- HTTP is used instead of HTTPS (production should enforce HTTPS)

## Development Workflow

The project follows a professional Git workflow with feature branches:

1. `feature/domain-and-infrastructure` - Core entities and repositories
2. `feature/employee-crud-api` - CRUD operations and API setup
3. `feature/jwt-authentication` - Authentication implementation
4. `feature/razor-ui-authentication` - Razor Pages UI
5. `feature/ui-improvements` - UI enhancements
6. `fix/final-polish` - Code cleanup
7. `fix/jwt-configuration` - Security improvements
8. `feature/blazor-ui` - Blazor WebAssembly implementation
9. `fix/blazor-ui-polish` - UI refinements

Each feature was developed in isolation and merged to the develop branch through simulated pull requests.

## License

This project is created for technical assessment purposes.
