using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<SqlRepo>();

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:sqlConnection");

// App instance
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

app.UseHttpsRedirection();

// Get all tickets
app.MapGet("/tickets", (SqlRepo repo) => repo.getAllTickets(connectionString));

// Get one ticket
app.MapGet("/tickets/{id}", (SqlRepo repo, int ticketId) =>
{
    var response = repo.getTicketById(connectionString, ticketId);
    return (response.Count >= 1) ? Results.Ok() : Results.NotFound();

});

app.MapGet("/tickets/emptickets/{id}", (SqlRepo repo, int userId) =>
{
    var response = repo.getAllUserTickets(connectionString, userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();

});

// TODO: create ticket
// app.MapPost("/tickets", (SqlRepo repo, int userID, Ticket ticket) =>
// {
//     repo.insertTicket(connectionString, userID, ticket);
//     return Results.Created($"/tickets/{ticket.Id}", ticket);
// });

// TODO: update ticket
//updates specific ticket, only the status is allowed to to be changed
// app.MapPut("/tickets/{id}", (SQLRepo repo, Employee employee, int ticketId, string status) =>
// {
//     repo.updateTicket(employee, ticketId, status, connString);
//     return Results.NoContent();
// });




// TODO: Replicate
//creates employee
// app.MapPost("/employees", (SQLRepo repo, Employee employee) =>
// {
//     repo.insertEmployee(employee, connString);
//     return Results.Created($"/employees/{employee.iD}", employee);
// });

//creates ticket
// app.MapPost("/tickets", (SQLRepo repo, int employeeID, Ticket ticket) =>
// {
//     repo.insertTicket(employeeID, ticket, connString);
//     return Results.Created($"/tickets/{ticket.Id}", ticket);
// });

//updates specific employee
// app.MapPut("/employees/{id}", (int id, Employee employee, SQLRepo repo) =>
// {
//     repo.updateEmployee(employee, id, connString);
//     return Results.NoContent();
// });

app.Run();
