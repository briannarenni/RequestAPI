# Employee Reimbursement API

Minimal API that implements a basic ticketing system to handle employee reimbursement requests and store them in a database.

*Built with: Net Core 7, Swagger/OpenAPI, SQLServer, and Azure App Services.*

## Features
---
- New employee accounts can be registered
- Employees can submit new tickets and view their submission history
- Managers can view all tickets, process pending tickets, and control what users have manager permissions.

## Object Schemas
---
### User
```
int userId
string username
string role
int numberOfPendingTickets
int totalNumberOfSubmittedTickets
```

### Ticket
```
int ticketId
DateTime submittedOn
int submittedBy
string employeeName
decimal amount
string category
string status
```

## Endpoints
---
**_NOTE: To avoid erroneous whitespace errors, all user input should be trimmed before being sent in a request._**

The API is split into 2 endpoint categories:

-   **User endpoints** to handle user account operations
-   **Ticket endpoints** to handle reimbursement ticket operatons

Below are all available endpoints and methods. See [Endpoints In-Depth](#endpoints-in-depth) for details on each endpoint.

| Endpoints                       | HTTP Method                 | Action                         |
| ------------------------------- | --------------------------- | ------------------------------ |
| `/users/register`               | POST                        | Create new employee account    |
| `/users/login`                  | POST                        | Log into existing user account |
| `/users/{user}/details`         | POST                        | Get user account details       |
| `/users/{user}/update-role`     | POST                        | Update user role               |
| `/users/{user}/update-password` | PATCH                       | Update user password           |
| `/tickets/{user}`               | POST                        | Get all tickets by user        |
| `/tickets/submit`               | POST                        | Submit a new ticket            |
|                                 | **Admin/Manager Endpoints** |
| `/employees`                    | GET                         | Get a list of all employees    |
| `tickets/all`                   | GET                         | Get all tickets                |
| `tickets/open`                  | GET                         | Get all pending tickets        |
| `/tickets/{ticket}`             | POST                        | Get ticket by ticket id        |
| `/tickets/{ticket}`             | PATCH                       | Update a ticket's status       |

## Endpoints In-Depth
---
```
/users/register
```
Accepts a `username` and `password` string to add a new User to the database.
  - Response &rarr; `200 OK`
  - Errors &rarr; `400 Username already exists`

---

```
/users/login
```
Accepts a `username` and `password` string to authenticate user.
- Returns &rarr; `200 OK`
- Errors &rarr; `400 Username already exists` or `400 Password incorrect`

---

```
/users/{user}/details
```
Accepts the current user's username string to fetch account information.
- Returns &rarr; `User` object

---

```
users/{user}/update-role
```
Accepts an existing `userId` to toggle their existing role between **Manager** and **Employee**.
- Returns &rarr; `200 OK`

---

```
/users/{username}/update-password
```
Accepts the current user's `username`, and two matching `password` strings.
- Returns &rarr; `200 OK`

---

```
/tickets/{user}
```
Accepts the current user's `userId`.
- Returns &rarr; Array of `Ticket` objects
- Errors &rarr; `404 Error: No tickets found. Please check that the user ID is valid.`

---

```
/tickets/submit
```
To create a new ticket: current user's `userId` and `username`, a `decimal` amount, a `string` category of 'Travel', 'Lodging', 'Food', or 'Other', and a `string` comment of up to 500 chars (comment is optional and can be sent as `null`).
- Returns &rarr; `201 Request submitted succesfully`

---

```
/tickets/{ticket}
```

`POST` - Retrieve a ticket by `ticketId`
- Returns &rarr; `Ticket`
- Error &rarr; `404 Error: Invalid ticket id.`

`PATCH` - Update a ticket with `ticketId` and `status` string of either *'approved'* or *'denied'*.
- Returns &rarr; `200 OK`
