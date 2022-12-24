using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<SqlRepo>();

string? connString = builder.Configuration.GetValue<string>("ConnectionStrings:sqlConnection");

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

// TODO: API Documentation
// Get all tickets
app.MapGet("/tickets", (SqlRepo repo) => repo.getAllTickets(connString));

// Get pending tickets
app.MapGet("/tickets/pending", (SqlRepo repo) => repo.getPendingTickets(connString));

// Get one ticket
app.MapGet("/tickets/{id}", (SqlRepo repo, int ticketId) =>
{
    var response = repo.getTicketById(connString, ticketId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound(response);
});

// Get all employee tickets
app.MapGet("/tickets/emptickets/{id}", (SqlRepo repo, int userId) =>
{
    var response = repo.getAllUserTickets(connString, userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});

// Get single pending
app.MapGet("/tickets/pending/{id}", (SqlRepo repo, int ticketId) => {
    var response = repo.getSinglePending(connString, ticketId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});


app.Run();
