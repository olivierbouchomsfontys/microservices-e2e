# RabbitMQ

Demo for a microservice architecture, using RabbitMQ and API Gateway pattern.

## Prerequisites

* .NET 5
* Docker

## How to run

### RabbitMQ

* Run `docker-compose up -d` to launch RabbitMQ.
* Navigate to `localhost:7100` and log in with user `guest` and password `guest`
* Add exchange `viemexchange` with type `fanout`
* Add queue `CustomerService` and bind to `viemexchange`
* Add queue `OrderService` and bind to `viemexchange`

### API

* Run all projects. In Rider it can be launched by the compound `All`
