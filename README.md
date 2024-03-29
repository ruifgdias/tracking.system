# Tracking System

# System Architecture

## Pixel Service

- Components:
  - API Endpoint
  - RabbitMQ Producer

## Storage Service

- Components:
  - RabbitMQ Consumer
  - Write to file

## Communication

- RabbitMQ Exchange


## Architecture Diagram

```mermaid
graph TD;
    PixelService --> ApiEndpoint;
    ApiEndpoint --> RabbitMQProducer;
    RabbitMQProducer --> RabbitMQExchange;
    RabbitMQExchange --> StorageService;
    StorageService --> RabbitMQConsumer;
    RabbitMQConsumer --> WriteToFile;
```