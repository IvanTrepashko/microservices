# E-commerce Microservices Platform

High-load e-commerce platform built with microservices architecture.

## Services Architecture

### 1. API Gateway
- **Tech**: C# (.NET 8), Ocelot
- **Features**:
  - JWT validation
  - Rate limiting
  - Request aggregation
  - Circuit breaker
  - Load balancing

### 2. Identity Service
- **Tech**: C# (.NET 8), PostgreSQL, Redis
- **Features**:
  - Authentication/Authorization
  - JWT management
  - User management
  - Role-based access
  - Session management

### 3. Product Catalog Service
- **Tech**: Go, MongoDB, Redis, Elasticsearch
- **Features**:
  - Product management
  - Category management
  - Search/filtering
  - Product recommendations
  - Cache management

### 4. Inventory Service (CQRS)
- **Tech**: C#, PostgreSQL (write), MongoDB (read), Redis, Kafka
- **Features**:
  - Stock management
  - Reservation system
  - Real-time inventory updates
  - Stock alerts
  - Inventory analytics

### 5. Order Service
- **Tech**: Go, PostgreSQL, Kafka, RabbitMQ
- **Features**:
  - Order processing
  - Order status management
  - Order history
  - Returns management
  - Order analytics

### 6. Payment Service
- **Tech**: C#, PostgreSQL, Redis, RabbitMQ
- **Features**:
  - Payment processing
  - Refund handling
  - Payment methods management
  - Transaction history
  - Fraud detection

### 7. Cart Service
- **Tech**: Go, Redis, RabbitMQ
- **Features**:
  - Cart management
  - Price calculation
  - Cart expiration
  - Abandoned cart handling
  - Cart merging

### 8. Pricing Service
- **Tech**: Go, MongoDB, Redis, Kafka
- **Features**:
  - Dynamic pricing
  - Discount management
  - Promotion engine
  - Price history
  - Bulk pricing

### 9. Notification Service
- **Tech**: C#, MongoDB, RabbitMQ
- **Features**:
  - Email notifications
  - SMS notifications
  - Push notifications
  - Notification templates
  - Notification preferences

### 10. Analytics Service
- **Tech**: Go, MongoDB, Kafka, Elasticsearch
- **Features**:
  - Sales analytics
  - User behavior analysis
  - Performance metrics
  - Custom reports
  - Real-time dashboards

### 11. Review Service
- **Tech**: Go, MongoDB, Elasticsearch, RabbitMQ
- **Features**:
  - Product reviews
  - Rating system
  - Review moderation
  - Review analytics
  - Review search

## Infrastructure

### Message Brokers
- Kafka: Event store, analytics events
- RabbitMQ: Service-to-service communication

### Databases
- PostgreSQL: Transactional data
- MongoDB: Document storage
- Redis: Caching, session storage
- Elasticsearch: Search, logging

### Monitoring & Logging
- ELK Stack: Log aggregation
- Prometheus: Metrics collection
- Grafana: Monitoring dashboards

### DevOps
- Docker: Containerization
- Kubernetes: Orchestration
- CI/CD: GitHub Actions
- Service Mesh: Istio (optional)
