GITHUB GRPC SERVICE WRAPPER
OVERVIEW
- 
This project implements a gRPC service to wrap GitHub's API for repository search. It provides a SearchRepos RPC method, allowing clients to query GitHub repositories efficiently. The service handles API calls, transforms data into gRPC-compatible responses, and provides global error handling and logging.

FEATURES
- 
GRPC Service: GithubGrpcService implements a SearchRepos RPC method.
Integration with GitHub API: Uses Refit-based client to call GitHub's REST API.
Data Mapping: Converts API response to gRPC-specific data models.
Error Handling: Global exception interceptor for consistent and informative error responses.
Unit & Integration Tests: Comprehensive tests for service methods and mappers.
Logging: Structured logging with Serilog, supports console and file outputs.

ARCHITECTURE
Projects
-
Application: Core service logic, including GithubService and exception handling middleware.
Infrastructure: Interfaces for external dependencies (e.g., GitHub API client).
Domain: DTOs and data models for GitHub API responses.
GRPC: Service implementation (GithubGrpcService), mappings, and proto definitions.
Tests: Unit and integration tests for all components.

SETUP AND CONFIGURATION
Prerequisites
-
.NET 8

Configuration
-
Add the GitHub API base URL and logging informations in the appsettings.json file:
{
  "GitHubApi": {
    "BaseUrl": "https://api.github.com"
  },
  "Serilog": {
    "MinimumLevel": "Debug",
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "obj//Debug//net8.0//Logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": [ "FromLogContext" ]
  }
}

DEPENDENCIES
- 
Refit: For GitHub API HTTP client abstraction.
Serilog: For structured logging.
gRPC: For service implementation and communication.
Moq: For testing mocks.

CORE CLASSES
GithubGrpcService
-
Handles incoming gRPC requests and delegates work to the application service.

GrpcExceptionHandlerInterceptor
-
Centralized error handling with contextual logging:




