# Employee Reimbursement API

Minimal API that handles a basic employee reimbursement request system. _Created in conjunction with [RequestHandler](https://github.com/briannarenni/RequestHandler-221024), a C# console app._

**Built With:**

-   NET Core 7
-   Swagger/OpenAPI
-   SQLServer database
-   Hosted on Azure

### Features

-   Employees can register a new account to submit requests
-   Employees can view their submission history
-   Managers can view, approve, or deny all pending requests
-   Managers can view all submitted requests
-   Existing users can reset their account's password

---

## Documentation

The API is split into 2 endpoint categories:

-   **User endpoints** handle account functions, such as viewing a user's ID and changing their password.

-   **Ticket endpoints** handle reimbursement request functions.

| Endpoints                           | HTTP Methods | Action                              |
| ----------------------------------- | ------------ | ----------------------------------- |
| `/users/register`                   | POST         | Create a new employee account       |
| `/users/login`                      | POST         | Log into an existing user account   |
| `/users/{username}`                 | POST         | Gets current user's account stats   |
| `/users/{username}/change-password` | PATCH        | Changes an existing user's password |
| `/employees`                        | GET          | Get all employees                   |
| `/tickets`                          | GET          | Get all requests                    |
| `/tickets/pending`                  | GET          | Get all pending requests            |
| `/tickets/employee/{id}`            | POST         | Get employee submissions            |
| `/tickets/{id}`                     | POST         | View request by id                  |
| `/tickets/{id}`                     | PATCH        | Update request by id                |
| `/tickets/pending/{id}`             | PATCH        | Update pending request by id        |
 
---

### User Endpoints (cont)

`/users/login` accepts a `username` and `password` string. The username will be checked first, then the password, so that any errors returned will specify which input was incorrect.

`/users/register` accepts a `username` and `password` string. If the given username isn't already registered, the user account will be created with the given password string, and the user can be logged in immediately.

`/users/{username}` accepts the current user's `username` as a string, then returns a `User` object containing:

-   int `userId`
-   string `username`
-   string `role` (`employee` or `manager`)
-   int `numberOfPendingRequests`
-   int `totalRequestsSubmitted`

`/users/{username}/change-password` accepts the current user's `username`, and two matching `password` strings. Both will be checked by the API to confirm match. If the strings don't match, the returned error response will specify that.

### Ticket Endpoints (cont)

`/tickets/employee/{id}` accepts the current user's `id`.

`/tickets/{id}` accepts any _existing_ ticket id. Use `POST` to view the ticket, and `PATCH` to update its status.

`/tickets/pending/{id}` accepts any _pending_ ticket id, and a `PATCH` request to update its status from pending.
