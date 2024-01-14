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
- [License](#license)

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
- JSON object representing the contact with updated information.

### Create Contact

Create a new contact.

**Endpoint:** POST /contacts/create


**Request Body:**
- JSON object representing the new contact.

## Dependencies

- Microsoft.AspNetCore.Mvc
- Newtonsoft.Json
- Microsoft.Extensions.Caching.Memory
- Microsoft.Extensions.Logging


