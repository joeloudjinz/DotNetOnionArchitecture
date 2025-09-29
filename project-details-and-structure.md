# Implementation Plan

**1. Final Project and Folder Structure**

This structure physically separates the logical layers of the architecture, ensuring a clean separation of concerns and enforcing the dependency rule.

```
└── InzRate.sln
    ├── InzRate.Core.Domain
    │   ├── Entities
    │   │   ├── Movie.cs
    │   │   ├── User.cs
    │   │   └── Review.cs      // Aggregate Root
    │   ├── Enums
    │   │   └── ...
    │   ├── Events             // For Domain Events
    │   │   └── ReviewCreatedEvent.cs
    │   ├── Exceptions
    │   │   └── InvalidRatingException.cs
    │   └── ValueObjects
    │   └── Rating.cs
    ├── InzRate.Core.Application
    │   ├── Contracts
    │   │   ├── Persistence    // Repository Interfaces
    │   │   │   ├── IUserRepository.cs
    │   │   │   ├── IMovieRepository.cs
    │   │   │   ├── IReviewRepository.cs
    │   │   │   └── IUnitOfWork.cs
    │   │   └── Identity       // Optional: For user context
    │   │       └── IUserService.cs
    │   ├── DTOs               // Data Transfer Objects
    │   │   └── MovieDto.cs
    │   │   └── ReviewDto.cs
    │   ├── Exceptions
    │   │   ├── BadRequestException.cs
    │   │   └── NotFoundException.cs
    │   ├── Features           // CQRS Commands and Queries
    │   │   ├── Movies
    │   │   │   ├── Queries
    │   │   │   │   └── GetMovieById
    │   │   │   │   └── GetAllMovies
    │   │   ├── Reviews
    │   │   │   ├── Commands
    │   │   │   │   ├── CreateReview
    │   │   │   │   └── UpdateReview
    │   │   │   └── Queries
    │   │   │       └── GetReviewsForMovie
    │   │   └── Users
    │   │       ├── Commands
    │   │       │   ├── RegisterUser
    │   │       │   └── LoginUser
    │   ├── Mappings
    │   │   └── MappingProfile.cs  // AutoMapper Profiles
    │   └── Behaviours         // MediatR pipeline behaviours
    │       └── ValidationBehaviour.cs
    ├── InzRate.Core.Infrastructure
    │   ├── Persistence
    │   │   ├── AppDbContext.cs
    │   │   ├── Repositories
    │   │   │   ├── UserRepository.cs
    │   │   │   ├── MovieRepository.cs
    │   │   │   └── ReviewRepository.cs
    │   │   ├── UnitOfWork.cs
    │   │   └── Configurations   // EF Core Configurations
    │   │       └── ReviewConfiguration.cs
    │   ├── Identity
    │   │   └── UserService.cs   // Implementation for IUserService
    │   └── Data
    │       └── Seed.cs          // Data Seeding Logic
    └── InzRate.App.Api
    │   ├── Endpoints          // Carter Modules or Minimal API Groups
    │   │   ├── MovieEndpoints.cs
    │   │   └── UserEndpoints.cs
    │   ├── Middleware
    │   │   └── GlobalExceptionHandler.cs
    │   └── Program.cs
    ├── InzRate.Test.Domain.UnitTests
    └── InzRate.Test.Application.UnitTests

```

### **2. Architectural Principles and Constraints**

Adherence to these rules is non-negotiable for maintaining the architectural integrity of the system.

- **DO:** Place all core business logic, entities, aggregates, value objects, and domain events in the `InzRate.Domain` project.
- **DON'T:** Add any NuGet package references for external concerns (e.g., `Microsoft.EntityFrameworkCore`, `Serilog`, `AutoMapper`) to the `InzRate.Domain` project. It must remain pure and framework-independent.
- **DO:** Define all external-facing abstractions (interfaces) required by the application in the `InzRate.Application` project. This includes contracts for persistence (`IRepository`, `IUnitOfWork`) and other services (`IUserService`, `IEmailService`).
- **DON'T:** Place concrete implementations of repositories or external services in the `InzRate.Application` project. This layer defines the "what," not the "how."
- **DO:** Ensure project references flow in one direction: `Api` → `Application` ← `Infrastructure`, and `Application` → `Domain`. The `Domain` project must not reference any other project in the solution.
- **DON'T:** Allow `IQueryable<T>` to be returned from repository interfaces. This leaks persistence concerns into the application layer and violates the separation of concerns. Return `Task<IEnumerable<T>>` or specific DTOs instead.
- **DO:** Use the `MediatR` library in the `InzRate.Application` layer to implement the CQRS pattern. Commands and Queries should be distinct, self-contained use cases.
- **DON'T:** Place business logic inside API controllers or endpoint handlers. The presentation layer's only responsibility is to receive requests, delegate work to the `Application` layer (by sending a command or query), and return the result.
- **DO:** Implement architectural tests using a library like `NetArchTest` to programmatically enforce dependency rules during the build process.

### **3. Recommended NuGet Packages**

This table outlines the essential packages for each layer, providing a clear justification for their inclusion.

| Project Layer | Package Name | Justification |
| --- | --- | --- |
| **`Application`** | `MediatR` | Implements a clean, in-process mediator pattern, perfect for lightweight CQRS to decouple use cases. |
| **`Application`** | `FluentValidation.DependencyInjectionExtensions` | Provides a powerful and declarative way to define validation rules for commands and queries. |
| **`Application`** | `AutoMapper.Extensions.Microsoft.DependencyInjection` | Simplifies the mapping of domain entities to DTOs, reducing boilerplate code in application handlers. |
| **`Infrastructure`** | `Microsoft.EntityFrameworkCore.SqlServer` | The EF Core provider for SQL Server. This is the concrete data access implementation. |
| **`Infrastructure`** | `Microsoft.EntityFrameworkCore.Design` | Provides design-time tooling for EF Core migrations. |
| **`Infrastructure`** | `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | Provides the implementation for [ASP.NET](http://asp.net/) Core Identity for user management. |
| **`Infrastructure`** | `Microsoft.Extensions.Options.ConfigurationExtensions` | Binds configuration from `appsettings.json` to strongly-typed options classes. |
| **`Api`** | `Carter` | Provides a lightweight and clean way to define Minimal API endpoints in a modular fashion. |
| **`Api`** | `Serilog.AspNetCore` | Provides robust, structured logging capabilities for the presentation layer. |
| **`Api`** | `Swashbuckle.AspNetCore` | Automatically generates OpenAPI/Swagger documentation for the API. |
| **`tests/*`** | `xunit`, `Moq`, `FluentAssertions` | Standard, high-quality libraries for writing unit tests, mocking dependencies, and creating readable assertions. |
| **`tests/Architecture`** | `NetArchTest.Rules` | A library for creating automated architectural tests to enforce dependency rules between projects. |