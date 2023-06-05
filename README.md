# TDC Innovation 2023 - .NET Architecture

Our implementation of Vertical Slice Architecture in .NET

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_tdc-vertical-slice-architecture&metric=coverage)](https://sonarcloud.io/summary/new_code?id=lgcmotta_tdc-vertical-slice-architecture)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_tdc-vertical-slice-architecture&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=lgcmotta_tdc-vertical-slice-architecture)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_tdc-vertical-slice-architecture&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=lgcmotta_tdc-vertical-slice-architecture)

Lecture co-hosts: [@lgcmotta](https://github.com/lgcmotta) and [@ffernandolima](https://github.com/ffernandolima).

## Vertical Slices Architecture :cake:

Banking App was designed to act as a simplistic implementation of a bank backend using the Vertical Slice Architecture.

![architecture-diagram-overview](https://github.com/lgcmotta/tdc-vertical-slice-architecture/assets/33238105/2b131fd9-c91d-4bf2-9e39-a0dfb5b3b616)

The microservices are organized into components (use cases), and each feature is an independent part of the system. 
A feature can never import code from some other component. 
This is designed to prevent side effects when the feature code changes. 
However, features can import general-purpose code.

Features can be either a `Command` or a `Query`, but never both. Although the output from a `Query` can be a `Command`.

The feature entrypoint is responsible to send the `Command` or request the `Query` data.

![command-query-flow](https://github.com/lgcmotta/tdc-vertical-slice-architecture/assets/33238105/4c69fec5-550a-4be4-8895-e4feb751774a)

## Running Banking App :bank:

### Requirements

- .NET 7 SDK
- Docker

### Environment Setup :whale2:

Start container dependencies with:

```bash
docker compose up -d
``` 

To have the Banking App full picture up and running you need to start the following projects:

- `./src/Geteway/BankingApp.Gateway`
- `./src/Accounts/BankingApp.Accounts.API`
- `./src/Transactions/BankingApp.Transactions.API`
- `./src/Fees/BankingApp.Fees.API`

You can use your IDE to launch these projects, or the `dotnet run` command.

## Contribute :wave:

Feel free to open an issue or a pull request. 
