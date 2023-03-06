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

app.MapPost("/users/login", (UserRepo uRepo, [FromBody] UserCreds loginData) =>
{
    string username = loginData.Username;
    string password = loginData.Password;
    return uRepo.validateLogin(username, password);
});

app.MapPost("/users/register", (UserRepo uRepo, [FromBody] NewUser regData) =>
{
    string firstName = regData.FirstName;
    string lastName = regData.LastName;
    string username = regData.Username;
    string password = regData.Password;
    string? dept = regData.Dept;
    return uRepo.validateRegister(firstName, lastName, username, password, dept);
});

app.MapGet("/users/{id}", (UserRepo uRepo, [FromBody] UserID data) =>
{
    int userId = data.userId;
    return uRepo.getUserInfo(userId);
});

app.MapGet("/users/", (UserRepo uRepo) => uRepo.getUsers());

app.MapGet("/users/employees", (UserRepo uRepo) => uRepo.getEmployees());

app.MapPatch("/users/{id}/role", (UserRepo uRepo, [FromBody] UserID data) =>
{
    int userId = data.userId;
    return uRepo.changeRole(userId);
});

app.MapPatch("/users/{id}/password", (UserRepo uRepo, [FromBody] PasswordUpdate pwData) =>
{
    int userId = pwData.UserId;
    string pw1 = pwData.Password;
    string pw2 = pwData.ConfirmPassword;
    return uRepo.updatePassword(userId, pw1, pw2);
});

app.MapPost("/users/{id}/tickets", (TicketRepo tRepo, [FromBody] UserID data) =>
{
    int userId = data.userId;

    List<Ticket>? response = tRepo.getUserTickets(userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Error: No tickets found. Please check that the user ID is valid.");
});

app.MapPost("/tickets/submit", (TicketRepo tRepo, [FromBody] NewTicket data) =>
{
    int submittedBy = data.UserId;
    string employeeName = data.Name;
    double amount = data.Amount;
    string category = data.Category;
    string comments = data.Comments;
    return tRepo.addTicket(submittedBy, employeeName, amount, category, comments);
});

app.MapGet("/tickets/", (TicketRepo tRepo) => tRepo.getAllTickets());

app.MapGet("/tickets/open", (TicketRepo tRepo) => tRepo.getPendingTickets());

app.MapPost("/tickets/{id}", (TicketRepo tRepo, [FromBody] TicketID data) =>
{
    int ticketId = data.ticketId;
    Ticket? response = tRepo.getTicketById(ticketId);
    return (response == null) ? Results.NotFound("Error: Invalid ticket id.") : Results.Ok(response);
});

app.MapPatch("/tickets/{id}", (TicketRepo tRepo, [FromBody] TicketUpdate data) =>
{
    int ticketId = data.ticketId;
    string status = data.status;
    return tRepo.updateTicketStatus(ticketId, status);
});

app.Run();

public record UserID(int userId);
public record TicketID(int ticketId);

public record TicketUpdate(int ticketId, string status);

public record UserCreds(string Username, string Password);

public record NewUser(string FirstName, string LastName, string Username, string Password, string Dept);

public record PasswordUpdate(int UserId, string Password, string ConfirmPassword);

public record NewTicket(int UserId, string Name, double Amount, string Category, string Comments);
