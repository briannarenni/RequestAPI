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

// ALL-USER METHODS
// Login verification
app.MapGet("/users/login", (UserRepo uRepo, string username, string password) => uRepo.validateLogin(connString, username, password));

// Register verification
app.MapGet("users/register", (UserRepo uRepo, string username, string password) => uRepo.validateRegister(connString, username, password));

// Add user
app.MapPost("/users", (UserRepo uRepo, string username, string password) => uRepo.addUser(connString, username, password));

// Change user's password
app.MapPut("users/{username}/change-password", (UserRepo uRepo, string username, string pw1, string pw2) => uRepo.updatePassword(connString, username, pw1.Trim(), pw2.Trim()));

// Get employee account info
app.MapGet("/users/{username}", (UserRepo uRepo, string username) => uRepo.getUserInfo(connString, username));

// MANAGER-USER METHODS
// Get all employees
app.MapGet("/employees", (UserRepo uRepo) => uRepo.getEmployees(connString));

// Change user role
app.MapPut("users/change-role", (UserRepo uRepo, int userId) => uRepo.changeRole(connString, userId));

// EMPLOYEE TICKET METHODS
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

// MANAGER TICKET METHODS
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

// Update ticket status
app.MapPut("/tickets", (TicketRepo tRepo, string status, int id) => tRepo.updateTicketStatus(connString, status, id));

app.Run();
