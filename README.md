# RaftLabsAssignment2025

## Description
.NET Core Library that fetches user data from Reqres.in API, built with Clean Architecture principles.

## Solution Structure
- **RaftLabsAssignment2025**: Class Library with services, API clients, and models.
- **RaftLabsAssignment2025.ConsoleApp**: Console application to demo usage.
- **RaftLabsAssignment2025.Tests**: Unit tests using xUnit and Moq.

## Features
- HttpClient integration via IHttpClientFactory.
- Async/await best practices.
- Retry policies (Polly).
- In-memory caching.
- Configuration using Options Pattern.
- Clean and testable design.

## Prerequisites
- .NET 8 SDK or later
- Visual Studio 2022+ or VSCode

## How to Run
1. Clone the repository
2. Open `RaftLabsAssignment2025.sln` as Administrator
3. Set `RaftLabsAssignment2025.ConsoleApp` as startup project.
4. Run!

## Running Tests
```bash
cd RaftLabsAssignment2025.Tests
dotnet test
