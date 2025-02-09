
# Discovery Service

This project implements a Discovery Service using Consul for service registration and discovery. It is a foundational microservice designed for service registration, health checks, and dynamic discovery within a microservices architecture.

---

## Table of Contents

- [About](#about)
- [Prerequisites](#prerequisites)
- [Setup and Installation](#setup-and-installation)
- [Features and Configuration](#features-and-configuration)
- [Usage](#usage)
- [Testing and Verification](#testing-and-verification)
- [License](#license)

---

## About

The Discovery Service registers itself with HashiCorp Consul and exposes endpoints for health checks (`/health`) and querying registered services (`/services`). It ensures seamless integration and monitoring of services in a dynamic environment.

---

## Prerequisites

Before proceeding, ensure the following tools and dependencies are installed:

1. **.NET SDK 8.0 or later**  
   Download the latest .NET SDK from the official [.NET Download Page](https://dotnet.microsoft.com/download).

2. **Consul Agent**  
   Install and run a Consul agent locally or in your network.  
   - **Install Consul:** [Consul Installation Guide](https://developer.hashicorp.com/consul/docs/install).  
   - **Run a local Consul instance:**  
     ```bash
     docker run -d --name=consul -p 8500:8500 consul
     ```

3. **Consul NuGet Package**  
   The application uses the `Consul` NuGet package for integration with HashiCorp Consul.

---

## Setup and Installation

1. **Create and Initialize the Project:**

   Run the following commands to set up the project:

   ```bash
   dotnet new webapi -n DiscoveryService
   cd DiscoveryService
   dotnet add package Consul
   ```

2. **Build and Restore Dependencies:**

   ```bash
   dotnet build
   ```

---

## Features and Configuration

### Key Features

- **Service Registration:**  
  On application startup, the service registers itself with Consul, providing metadata like name, unique ID, and health check details.

- **Health Check Endpoint:**  
  The `/health` endpoint ensures the service's availability and functionality.

- **Service Lookup Endpoint:**  
  The `/services` endpoint queries all registered services via the local Consul agent.

- **Graceful Deregistration:**  
  On shutdown, the service deregisters itself, preventing stale registry entries.

### Configuration

The following configurations are required for Consul integration:

- **Consul Address:**  
  Default: `http://127.0.0.1:8500`  
  Set via environment variable: `Consul:Address`.

- **Service Details:**  
  - **Name:** `DiscoveryService`  
  - **Port:** 5000 (Default ASP.NET Core port)  
  - **Health Check Endpoint:** `http://localhost:5000/health`

---

## Usage

### Running the Service

Execute the following command to run the service:

```bash
dotnet run
```

The application will start and listen on `http://localhost:5000`.

---

## Testing and Verification

### Health Check

Verify the service's health status by accessing the `/health` endpoint:

```bash
curl http://localhost:5000/health
```

Expected response:

```json
"Healthy"
```

### Service Lookup

Query all registered services using the `/services` endpoint:

```bash
curl http://localhost:5000/services
```

This will return a JSON response with details of all registered services.

### Consul Dashboard

Access the Consul Web UI at `http://127.0.0.1:8500` to view the Discovery Service's registration and health status.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
```
