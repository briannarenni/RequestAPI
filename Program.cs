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

app.MapGet("/tickets", (SqlRepo repo) => repo.getAllTickets(connectionString));

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
