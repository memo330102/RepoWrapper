GITHUB GRPC SERVICE WRAPPER
-
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
-
Projects
-
Application: Core service logic, including GithubService and exception handling middleware.

Infrastructure: Interfaces for external dependencies (GitHub API client).

Domain: DTOs and data models for GitHub API responses.

GRPC: Service implementation (GithubGrpcService), mappings, and proto definitions.

Tests: Unit and integration tests for all components.

SETUP AND CONFIGURATION
-
Prerequisites
-
.NET 8

Postman (latest version with gRPC support)

GitHub API access (public API)

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
-
GithubGrpcService
-
Handles incoming gRPC requests and delegates work to the application service.

GrpcExceptionHandlerInterceptor
-
Centralized error handling with contextual logging:

TESTING WITH POSTMAN
-
Set Up Postman for gRPC Testing
-
Ensure you are using the latest version of Postman, which supports gRPC.
![image](https://github.com/user-attachments/assets/a7c83f5c-db58-4b0b-9d6c-cec9ae2c76fe)

Open Postman and create a new request.

Select gRPC as the request type from the dropdown menu 

Import the .proto File
-
Click on "Import a Proto File" in the gRPC request setup.
![image](https://github.com/user-attachments/assets/f5e94b40-9fcd-49a6-b2f9-e1156f91ce5a)

Upload your github.proto file in project directory.

Postman will parse the proto file and display the service and method details.

Configure the gRPC Request
-
URL: Set the gRPC server address (grpc://localhost:5118 for local testing).

Service: Choose the GithubWrapper service from the dropdown after loading the .proto file.

Method: Select the SearchRepos method.

Send a Test Request
-
In the Message section, use the following JSON structure to send request : 

{
  "Querry": "wrapper"
}

![image](https://github.com/user-attachments/assets/d027df8c-ccdf-4873-9f04-708ccdf647ea)





