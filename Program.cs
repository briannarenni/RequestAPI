using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TicketRepo>();
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
app.MapGet("/tickets", (TicketRepo tRepo) => tRepo.getAllTickets(connString));

app.MapGet("/tickets/pending", (TicketRepo tRepo) => tRepo.getPendingTickets(connString));

app.MapGet("/tickets/{id}", (TicketRepo tRepo, int ticketId) =>
{
    List<Ticket>? response = tRepo.getAllTickets(connString);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Invalid ticket id");
});

app.MapGet("/tickets/pending/{id}", (TicketRepo tRepo, int ticketId) =>
{
    List<Ticket>? response = tRepo.getSinglePending(connString, ticketId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Invalid ticket id");
});

app.MapGet("/tickets/employee/{id}", (TicketRepo tRepo, int userId) =>
{
    List<Ticket>? response = tRepo.getUserTickets(connString, userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});

// * Use in docs: Ticket newTicket = new Ticket(userId, username, amount, category);
app.MapPost("/tickets", (TicketRepo tRepo, Ticket ticket) => tRepo.addTicket(connString, ticket));

app.MapPut("/tickets", (TicketRepo tRepo, string status, int id) => tRepo.updateTicketStatus(connString, status, id));

//TODO: User Methods

app.Run();
