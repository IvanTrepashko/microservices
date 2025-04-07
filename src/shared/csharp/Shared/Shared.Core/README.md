# Shared.Core

A core shared library for microservices containing common functionality and utilities.

## Features

- Common exceptions
- Shared utilities
- Core abstractions

## Installation

```bash
dotnet add package Shared.Core
```

## Usage

```csharp
using Shared.Core.Exceptions;

// Example usage
throw new NotFoundException<User>("123");
```

## License

This project is licensed under the MIT License. 