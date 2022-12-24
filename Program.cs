using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Handlers;
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

GetHandler get = new GetHandler(connString);

// Get all tickets
app.MapGet("/tickets", (SqlRepo repo) => repo.getAllTickets(connString));

// Get all pending tickets
app.MapGet("/tickets/pending", (SqlRepo repo) => repo.getPendingTickets(connString));

// Get one ticket
app.MapGet("/tickets/{id}", (SqlRepo repo, int ticketId) =>
{
    List<Ticket>? response = repo.getAllTickets(connString);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});

app.MapGet("/tickets/{id}", (SqlRepo repo, int ticketId) =>
{
    return get.GetTicketById(repo, ticketId);
});

// TODO: Refactor methods to this
// Get one pending ticket
app.MapGet("/tickets/pending/{id}", (SqlRepo repo, int ticketId) =>
{
    List<Ticket>? response = repo.getSinglePending(connString, ticketId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Invalid ticket id");
});

// Get employee's tickets
app.MapGet("/tickets/employee/{id}", (SqlRepo repo, int userId) =>
{
    List<Ticket>? response = repo.getUserTickets(connString, userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});

// * Use in docs: Ticket newTicket = new Ticket(userId, username, amount, category);
app.MapPost("/tickets", (SqlRepo repo, HttpRequest request) =>
{
    // Parse the request body to retrieve the values for the Ticket object
    int userId = int.Parse(request.Form["userId"]);
    string username = request.Form["username"];
    double amount = double.Parse(request.Form["amount"]);
    string category = request.Form["category"];
    Ticket newTicket = new Ticket(userId, username, amount, category);

    // Call the addNewTicket method and pass in the Ticket object
    repo.addTicket(connString, newTicket);

    return Results.Ok();
});

app.MapPut("/tickets", (SqlRepo repo, HttpRequest request) =>
{
    // Parse the request body to retrieve the values for the Ticket object
    string status = request.Form["status"];
    if (int.TryParse(request.Form["id"], out int id))
    {
        repo.updateTicketStatus(connString, status, id);
        return Results.Ok();
    }
    else
    {
        return Results.BadRequest("Invalid ticket ID");
    }
});

//TODO: User Methods

app.Run();
