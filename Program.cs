using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TicketRepo>();
builder.Services.AddTransient<UserRepo>();
string? connString = builder.Configuration.GetValue<string>("ConnectionStrings:sqlConnection");

// App instance
WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

// USER METHODS
// Get user's account info
app.MapGet("/users/{username}", (UserRepo uRepo, string username) => uRepo.getUserInfo(connString, username));

// Login verification
app.MapGet("/users/login", (UserRepo uRepo, string username, string password) =>
{
    bool usernameExists = uRepo.checkUsername(connString, username);
    bool passwordCorrect = false;

// ! string "Username not found" "Password not found"
    if (!usernameExists)
    {
        return Results.BadRequest("Username not found");
    }
    else
    {
        passwordCorrect = uRepo.checkPassword(connString, username, password);
        return (passwordCorrect) ? Results.Ok() : Results.BadRequest("Password incorrect");
    }
});

// Register verification
app.MapGet("users/register", (UserRepo uRepo, string username, string password) =>
{
    bool usernameAvailable = !uRepo.validateRegistration(connString, username, password);
    if (usernameAvailable)
    {
        uRepo.addUser(connString, username, password);
        return Results.Created($"/users", "User registered successfully");
    }
    else
    {
        return Results.BadRequest("Username already exists");
    }
});

// Get all employees
app.MapGet("/employees", (UserRepo uRepo) => uRepo.getEmployees(connString));

// Add user
app.MapPost("/users", (UserRepo uRepo, string username, string password) => uRepo.addUser(connString, username, password));

// Change user's password
app.MapPut("users/{username}/change-password", (UserRepo uRepo, string username, string pw1, string pw2) => uRepo.updatePassword(connString, username, pw1.Trim(), pw2.Trim()));

// TICKET METHODS
// Get all tickets
app.MapGet("/tickets", (TicketRepo tRepo) => tRepo.getAllTickets(connString));

// Get only pending tickets
app.MapGet("/tickets/pending", (TicketRepo tRepo) => tRepo.getPendingTickets(connString));

// Get ticket by ID
app.MapGet("/tickets/{id}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getTicketById(connString, ticketId);
    return (response == null) ? Results.NotFound("Invalid ticket id") : Results.Ok(response);
});

// Get pending ticket by ID
app.MapGet("/tickets/pending/{id}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getSinglePending(connString, ticketId);
    return (response == null) ? Results.NotFound("Invalid ticket id") : Results.Ok(response);
});

// Get employee's tickets
app.MapGet("/tickets/employee/{id}", (TicketRepo tRepo, int userId) =>
{
    List<Ticket>? response = tRepo.getUserTickets(connString, userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});

// Add new ticket
app.MapPost("/tickets", (TicketRepo tRepo, int userId, string username, double amount, string category) =>
{
    Ticket newTicket = new Ticket(userId, username, amount, category);
    tRepo.addTicket(connString, newTicket);
});

// Update ticket status
app.MapPut("/tickets", (TicketRepo tRepo, string status, int id) => tRepo.updateTicketStatus(connString, status, id));

app.Run();
