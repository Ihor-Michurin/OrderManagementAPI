# OrderManagementAPI

This repository contains an example Order Management API with basic business logic for managing customers and their orders. Using PostgreSQL as DBMS.

## Setup and Run

1. Clone the repository to your local machine:
2. Navigate to the project directory
3. Open the solution in your preferred IDE (Visual Studio, Visual Studio Code, etc.).
4. Configure if needed the database connection string and ServiceBus connection string in the `appsettings.json` file for OrderManagementAPI project and connectionString, topicName, subscriptionName variables and database connection string for Consumer project.
5. Build the solution to restore NuGet packages and compile the project.
6. Run BusinessLogic.Tests
7. Run Consumer
8. Run OrderManagementAPI in Docker container


## Architectural Overview
The Order Management API follows a simple layered architecture:

Controllers: Expose API endpoints and handle incoming HTTP requests.
Business Logic: Contains the core business logic and interacts with the database.
Data Access: Defines the database context for Entity Framework Core.
Models: Defines the data models.

Tests: Contains unit tests for the business logic methods.

Technologies Used
.NET Core 6
Entity Framework Core
xUnit (for unit testing)
Moq (for mocking dependencies in tests)