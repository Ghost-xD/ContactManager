# Contact Manager

A simple ASP.NET Core Web API for managing contacts.

## Table of Contents
- [Introduction](#introduction)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
  - [Get Contacts](#get-contacts)
  - [Get Contact by ID](#get-contact-by-id)
  - [Delete Contact](#delete-contact)
  - [Update Contact](#update-contact)
  - [Create Contact](#create-contact)
- [Dependencies](#dependencies)

## Introduction

The Contact Manager is a RESTful API built using ASP.NET Core for managing contact information. It provides endpoints to retrieve, create, update, and delete contacts.

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/Ghost-xD/ContactManager.git

# Project Setup

1. Open the project in your preferred development environment.

2. Build and run the project.

3. Access the API using the specified endpoints.

## API Endpoints

### Get Contacts

Retrieve a list of contacts.

**Endpoint:** GET /contacts/{pageNum}/{pageSize}


**Parameters:**
- `pageNum` (optional): Page number for pagination (default is 0).
- `pageSize` (optional): Number of contacts per page (default is 10).

### Get Contact by ID

Retrieve a specific contact by ID.

**Endpoint:** GET /contacts/{Id}


**Parameters:**
- `Id`: The unique identifier of the contact.

### Delete Contact

Delete a contact by ID.

**Endpoint:** DELETE /contacts/{Id}


**Parameters:**
- `Id`: The unique identifier of the contact to be deleted.

### Update Contact

Update an existing contact.

**Endpoint:** PUT /contacts/update


**Request Body:**
```json
{
  id: number,
  firstName: string,
  lastName: string,
  email: string
}
```
### Create Contact

Create a new contact.

**Endpoint:** POST /contacts/create


**Request Body:**
```json
{
  firstName: string,
  lastName: string,
  email: string
}
```
## Dependencies

- Microsoft.AspNetCore.Mvc
- Newtonsoft.Json
- Microsoft.Extensions.Caching.Memory
- Microsoft.Extensions.Logging

## How to Run

- Use the http port - 5020 and press F5 in Visual Studio.

## Application Structure

- This is a controller based Asp .Net Web Api with CRUD functionality.
- It uses MemoryCache for caching.
- It uses one Custom Middleware for handling global exceptions.
- For validations in the Model class, DataAnnotations are used.

## Scalability Options

- Replacing JSON (mock db) with some relational Database.
- There is a UtilityClass in place which can handle any type of database without modifying the controller class logic.
- Since there is code written for server side pagination, we can handle as much data as we want (when using RDBMS). We can take advantage of EFCORE to prepare a query to provide exact number of records from the db using Skip and Take functions.
