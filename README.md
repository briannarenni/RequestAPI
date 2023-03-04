# Employee Reimbursement API

Minimal API that implements a basic ticketing system to handle employee reimbursement requests and store them in a database.

_Built with: Net Core 7, Swagger/OpenAPI, SQLServer, and Azure App Services._

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
string firstName
string lastName
string username
string role
string dept
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
string comments (can be null)
```

## Endpoints

---

**_NOTE: To avoid erroneous whitespace errors, all user input should be trimmed before being sent in a request._**

All endpoints accept a JSON object in a request. The API is split into 2 endpoint categories:

- **User endpoints** to handle user account operations
- **Ticket endpoints** to handle reimbursement ticket operatons

Below are all available endpoints and methods. See [Endpoints In-Depth](#endpoints-in-depth) for details on each endpoint.

| Endpoints              | HTTP Method                 | Action                         |
| ---------------------- | --------------------------- | ------------------------------ |
| `/users/register`      | POST                        | Create new employee account    |
| `/users/login`         | POST                        | Log into existing user account |
| `/users/{id}`          | GET                         | Get user account info          |
| `/users/{id}/role`     | PATCH                       | Update user role               |
| `/users/{id}/password` | PATCH                       | Update user password           |
| `/users/{id}/tickets`  | POST                        | Get all tickets by user        |
| `/tickets/submit`      | POST                        | Submit a new ticket            |
|                        | **Admin/Manager Endpoints** |
| `users/employees`      | GET                         | Get a list of all employees    |
| `tickets/`             | GET                         | Get all tickets                |
| `tickets/open`         | GET                         | Get all pending tickets        |
| `/tickets/{id}`        | POST                        | Get ticket by ticket id        |
| `/tickets/{id}`        | PATCH                       | Update a ticket's status       |

## Endpoints In-Depth

---

```
/users/register
```

Accepts a `firstName`, `lastName`, `username`, `password`, and `dept` string to add a new User to the database. Returns a User object on success.

- Response &rarr; `User` object
- Errors &rarr; `400 Username already exists`

---

```
/users/login
```

Accepts a `username` and `password` string to authenticate a user, and returns a User object on success.

- Response &rarr; `User` object
- Errors &rarr; `400 Username already exists` or `400 Password incorrect`

---

```
/users/{id}
```

Accepts the current user's `userId` to fetch account information.

- Returns &rarr; `User` object

---

```
/users/{id}/role
```

Accepts an existing `userId` to toggle their existing role between **Manager** and **Employee**.

- Returns &rarr; `'User role changed to {new role}'`

---

```
/users/{id}/password
```

Accepts the current user's `userId`, and two matching `password` strings.

- Returns &rarr; `'Password updated successfully'`

---

```
/users/{id}/tickets
```

Accepts the current user's `userId`.

- Returns &rarr; Array of `Ticket` objects
- Errors &rarr; `404 Error: No tickets found. Please check that the user ID is valid.`

---

```
/tickets/submit
```

To create a new Ticket, submit the following: Current user's `userId` and `fullName`, a `decimal` amount, a `string` category ('Travel', 'Lodging', 'Food', or 'Other'), and a `comments` string of up to 500 chars (comments str is optional & can be sent as `null`).

- Returns &rarr; `201 Request submitted succesfully`

---

```
/tickets/{id}
```

Accepts 2 methods:
`POST` - Retrieve a ticket by `ticketId`

- Returns &rarr; `Ticket` object
- Error &rarr; `404 Error: Invalid ticket id.`

`PATCH` - Update a ticket with `ticketId` and `status` string of either _'approved'_ or _'denied'_.

- Returns &rarr; `200 OK`
