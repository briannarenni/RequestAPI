# Employee Reimbursement API
*Originally created in conjunction with [RequestHandler](https://github.com/briannarenni/RequestHandler-221024), a C# console app. Made purely for demo purposes.*
## Description
---
This is a .NET Minimal API that handles a basic employee reimbursement request system. <br>
Employees can register an account to submit and review requests, and managers can login to their accounts to view and process requests.
## Documentation
---
The API is split into 2 major categories for employees and managers, **User** and **Ticket**:

- **User endpoints** handle account functions, such as viewing a user's ID and changing their password.

- **Ticket endpoints** handle reimbursement request functions. Below are the endpoints with their associated methods.

## User Endpoints:
----
```
/users/login
```
Accepts a `username` and `password` string. The username will be checked first, then the password, so that any errors returned will specify which input was incorrect.

```
/users/register
```
Accepts a `username` and `password` string. If the given username isn't already registered, the user account will be created with the given password string, and the user can be logged in immediately.

```
/users/{username}
```
Accepts the current user's `username` string to get that user's account information. Returns a `User` object containing:
- int `userId`
- string `username`
- string `role` (`employee` or `manager`)
- int `numberOfPendingRequests`
- int `totalRequestsSubmitted`

```
/users/{username}/change-password
```
Accepts the current user's `username`, and two matching `password` string. Both passwords will be checked by the API as matching.<br>
If the strings don't match, the returned response will specify that error.

## Ticket Endpoints:
---

## Dependencies
---
The API is built on ASP.NET Core 7 and OpenAPI/Swagger. It's connected to an SQLServer database (hosted with Azure). Packages installed:
- System.Data.SqlClient
- Microsoft.AspNetCore.Http
- Swashbuckle.AspNetCore
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
