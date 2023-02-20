using Microsoft.AspNetCore.Mvc;
using TicketAPI_Data;
using TicketAPI_Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<UserRepo>(c => new UserRepo(c.GetService<IConfiguration>()));
builder.Services.AddTransient<TicketRepo>(c => new TicketRepo(c.GetService<IConfiguration>()));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

app.UseCors();

app.MapGet("/users", (UserRepo uRepo) => uRepo.getUsers());

app.MapPost("/users/login", (UserRepo uRepo, [FromBody] UserCreds loginData) =>
{
    string username = loginData.Username;
    string password = loginData.Password;
    return uRepo.validateLogin(username, password);
});

app.MapPost("/users/register", (UserRepo uRepo, [FromBody] NewUser regData) =>
{
    string username = regData.Username;
    string password = regData.Password;
    string firstName = regData.FirstName;
    string lastName = regData.LastName;
    return uRepo.validateRegister(username, password, firstName, lastName);
});

app.MapGet("/users/{id}", (UserRepo uRepo, int userId) => uRepo.getUserInfo(userId));

app.MapGet("users/employees", (UserRepo uRepo) => uRepo.getEmployees());

app.MapPatch("/users/{id}/role", (UserRepo uRepo, int userId) => uRepo.changeRole(userId));

app.MapPatch("/users/{id}/password", (UserRepo uRepo, [FromBody] PasswordUpdate pwData) =>
{
    int userId = pwData.UserId;
    string pw1 = pwData.Password;
    string pw2 = pwData.ConfirmPassword;
    return uRepo.updatePassword(userId, pw1, pw2);
});

app.MapPost("/users/{id}/tickets", (TicketRepo tRepo, int userId) =>
{
    List<Ticket>? response = tRepo.getUserTickets(userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Error: No tickets found. Please check that the user ID is valid.");
});

app.MapPost("/tickets/submit", (TicketRepo tRepo, [FromBody] NewTicket data) =>
{
    Ticket newTicket = new Ticket(data.UserId, data.Username, data.Amount, data.Category, data.Comments);
    return tRepo.addTicket(newTicket);
});

app.MapGet("/tickets/", (TicketRepo tRepo) => tRepo.getAllTickets());

app.MapGet("/tickets/open", (TicketRepo tRepo) => tRepo.getPendingTickets());

app.MapPost("/tickets/{id}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getTicketById(ticketId);
    return (response == null) ? Results.NotFound("Error: Invalid ticket id.") : Results.Ok(response);
});

app.MapPatch("/tickets/{id}", (TicketRepo tRepo, int ticketId, string status) => tRepo.updateTicketStatus(ticketId, status));

app.Run();

public record UserCreds(string Username, string Password);
public record NewUser(string Username, string Password, string FirstName, string LastName);
public record PasswordUpdate(int UserId, string Password, string ConfirmPassword);
public record NewTicket(int UserId, string Username, double Amount, string Category, string Comments);
