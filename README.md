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

+---------------------+          +-----------------------+           +-------------------+
|                     |          |                       |           |                   |
|   Pixel Service     |          |   RabbitMQ Exchange   | <-------- |   Storage Service |
|                     |          |                       |           |                   |
+---------------------+          +-----------------------+           +-------------------+
          |                               |                                   |
          v                               |                                   v
+---------------------+          +-----------------------+           +-------------------+
|                     |          |                       |           |                   |
|   API Endpoint      | -------->|  RabbitMQ Producer    |           |  RabbitMQ Consumer|
|                     |          |                       |           |                   |
+---------------------+          +-----------------------+           +-------------------+
                                                                              |
                                                                              |
                                                                              v
                                                                    +---------------------+
                                                                    |                     |
                                                                    |    Write To File    |
                                                                    |                     |
                                                                    +---------------------+
