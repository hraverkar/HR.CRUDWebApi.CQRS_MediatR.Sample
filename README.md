### Implementing a CRUD Web API using CQRS and MediatR
Code 
=======

Here's the updated README content with the additional information:

---

# HR.CRUDWebApi.CQRS_MediatR.Sample

This project demonstrates how to implement a CRUD Web API using CQRS and MediatR patterns in .NET 8.0.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## Overview

This sample project showcases a simple CRUD Web API built using CQRS (Command Query Responsibility Segregation) and MediatR patterns. It is designed to demonstrate the separation of concerns and the use of mediator pattern for handling requests and notifications.

## Features

- CRUD operations for managing entities
- CQRS pattern for separating command and query responsibilities
- Dependency Injection
- Fluent Validation
- Swagger for API documentation

## Technologies Used

- .NET 8.0
- CQRS and MediatR
- Entity Framework Core
- FluentValidation
- Swagger/OpenAPI
- Docker

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker (optional, for running the application in a container)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/hraverkar/HR.CRUDWebApi.CQRS_MediatR.Sample.git
    cd HR.CRUDWebApi.CQRS_MediatR.Sample
    ```

2. Restore the dependencies:
    ```sh
    dotnet restore
    ```

### Running the Application

1. Build and run the application:
    ```sh
    dotnet run
    ```

2. Open your browser and navigate to `https://localhost:5001/swagger` to see the Swagger API documentation.

## Usage

### CRUD Operations

The API provides endpoints for creating, reading, updating, and deleting entities. These operations follow the CQRS pattern where commands are used for creating, updating, and deleting, while queries are used for reading data.

### CQRS and MediatR

The project uses MediatR to handle request/response and notification patterns. This helps in decoupling the request handling logic from the controllers.

## Configuration

The application configuration is handled using `appsettings.json`. The connection string for the database can be configured in this file:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=host.docker.internal;Database=DemoApp;User Id=sa;Password=Admin1234!;TrustServerCertificate=True;"
  }
}
```

## Password Encryption and Decryption

In this repository, password encryption and decryption are handled by the `EncryptionService`. Here are the key points:

### EncryptionService

- **Encryption**: Uses AES encryption with a key and IV derived from the configuration.
- **Decryption**: Uses AES decryption to convert cipher text back to plain text.

### Example Code

**EncryptionService.cs**:
```csharp
using System.Security.Cryptography;
using System.Text;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Services
{
    public class EncryptionService : IEncryptionService
    {
        public readonly byte[] _key;
        public readonly byte[] _iv;
        public readonly IConfiguration _configuration;

        public EncryptionService(IConfiguration configuration)
        {
            _configuration = configuration;
            var key = _configuration["EncryptionKey"];
            if (key != null)
            {
                _key = Encoding.UTF8.GetBytes(key.Substring(0, 32));
                _iv = Encoding.UTF8.GetBytes(key.Substring(32, 16));
            }
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(plainBytes, 0, plainBytes.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
```

## Repository Information

- Repository: [HR.CRUDWebApi.CQRS_MediatR.Sample](https://github.com/hraverkar/HR.CRUDWebApi.CQRS_MediatR.Sample)
- Description: Using CQRS and MediatR Pattern in .Net Code 8.0
- Language Composition: 
  - C#: 94.6%
  - Dockerfile: 5.4%

## Contributing

Contributions are welcome! Please fork this repository and submit pull requests.

## License

This project is licensed under the MIT License.

---
