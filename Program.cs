using TicketAPI_Data;
using TicketAPI_Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TicketRepo>();
builder.Services.AddTransient<UserRepo>();
string? connString = builder.Configuration.GetValue<string>("ConnectionStrings:sqlConnection");

// App instance
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

app.MapGet("/users/login", (UserRepo uRepo, string username, string password) => uRepo.validateLogin(connString, username, password));

app.MapGet("users/register", (UserRepo uRepo, string username, string password) => uRepo.validateRegister(connString, username, password));

app.MapPost("/users", (UserRepo uRepo, string username, string password) => uRepo.addUser(connString, username, password));

app.MapPut("users/{username}/change-password", (UserRepo uRepo, string username, string pw1, string pw2) => uRepo.updatePassword(connString, username, pw1.Trim(), pw2.Trim()));

app.MapGet("/users/{username}", (UserRepo uRepo, string username) => uRepo.getUserInfo(connString, username));

app.MapGet("/employees", (UserRepo uRepo) => uRepo.getEmployees(connString));

app.MapPut("users/change-role", (UserRepo uRepo, int userId) => uRepo.changeRole(connString, userId));

app.MapPost("/tickets", (TicketRepo tRepo, int userId, string username, double amount, string category) =>
{
    Ticket newTicket = new Ticket(userId, username, amount, category);
    tRepo.addTicket(connString, newTicket);
});

app.MapGet("/tickets/employee/{id}", (TicketRepo tRepo, int userId) =>
{
    List<Ticket>? response = tRepo.getUserTickets(connString, userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound();
});

app.MapGet("/tickets", (TicketRepo tRepo) => tRepo.getAllTickets(connString));

app.MapGet("/tickets/pending", (TicketRepo tRepo) => tRepo.getPendingTickets(connString));

app.MapGet("/tickets/{id}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getTicketById(connString, ticketId);
    return (response == null) ? Results.NotFound("Invalid ticket id") : Results.Ok(response);
});

app.MapGet("/tickets/pending/{id}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getSinglePending(connString, ticketId);
    return (response == null) ? Results.NotFound("Invalid ticket id") : Results.Ok(response);
});

app.MapPut("/tickets/{id}", (TicketRepo tRepo, string status, int id) => tRepo.updateTicketStatus(connString, status, id));

app.Run();
