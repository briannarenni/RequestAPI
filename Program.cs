using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<SqlRepo>();
string? connString = builder.Configuration.GetValue<string>("ConnectionStrings:sqlConnection");

// TODO: API Documentation

// App instance
WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

app.UseHttpsRedirection();

// Methods
app.MapGet("/tickets", (SqlRepo repo) => repo.getAllTickets(connString));

app.MapGet("/tickets/pending", (SqlRepo repo) => repo.getPendingTickets(connString));

app.MapGet("/tickets/{id}", (SqlRepo repo, int ticketId) =>
{
    List<Ticket>? response = repo.getAllTickets(connString);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Invalid ticket id");
});

app.MapGet("/tickets/pending/{id}", (SqlRepo repo, int ticketId) =>
{
    List<Ticket>? response = repo.getSinglePending(connString, ticketId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Invalid ticket id");
});

app.MapGet("/tickets/employee/{id}", (SqlRepo repo, int userId) =>
{
    List<Ticket>? response = repo.getUserTickets(connString, userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});

// * Use in docs: Ticket newTicket = new Ticket(userId, username, amount, category);
app.MapPost("/tickets", (SqlRepo repo, Ticket ticket) => repo.addTicket(connString, ticket));

app.MapPut("/tickets", (SqlRepo repo, string status, int id) => repo.updateTicketStatus(connString, status, id));

//TODO: User Methods

app.Run();
