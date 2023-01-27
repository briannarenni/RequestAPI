using TicketAPI_Data;
using TicketAPI_Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<UserRepo>(c => new UserRepo(c.GetService<IConfiguration>()));
builder.Services.AddTransient<TicketRepo>(c => new TicketRepo(c.GetService<IConfiguration>()));

// App instance
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

app.MapGet("/users/login", (UserRepo uRepo, string username, string password) => uRepo.validateLogin(username, password));

app.MapGet("users/register", (UserRepo uRepo, string username, string password) => uRepo.validateRegister(username, password));

app.MapPost("/users", (UserRepo uRepo, string username, string password) => uRepo.addUser(username, password));

app.MapPut("users/{username}/change-password", (UserRepo uRepo, string username, string pw1, string pw2) => uRepo.updatePassword(username, pw1.Trim(), pw2.Trim()));

app.MapGet("/users/{username}", (UserRepo uRepo, string username) => uRepo.getUserInfo(username));

app.MapGet("/employees", (UserRepo uRepo) => uRepo.getEmployees());

app.MapPut("users/change-role", (UserRepo uRepo, int userId) => uRepo.changeRole(userId));

app.MapPost("/tickets", (TicketRepo tRepo, int userId, string username, double amount, string category) =>
{
    Ticket newTicket = new Ticket(userId, username, amount, category);
    tRepo.addTicket(newTicket);
});

app.MapGet("/tickets/employee/{id}", (TicketRepo tRepo, int userId) =>
{
    List<Ticket>? response = tRepo.getUserTickets(userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Error: No tickets found. Please check that the user ID is valid.");
});

app.MapGet("/tickets", (TicketRepo tRepo) => tRepo.getAllTickets());

app.MapGet("/tickets/pending", (TicketRepo tRepo) => tRepo.getPendingTickets());

app.MapGet("/tickets/{id}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getTicketById(ticketId);
    return (response == null) ? Results.NotFound("Error: Invalid ticket id.") : Results.Ok(response);
});

app.MapGet("/tickets/pending/{id}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getSinglePending(ticketId);
    return (response == null) ? Results.NotFound("Error: Invalid ticket id.") : Results.Ok(response);
});

app.MapPut("/tickets/{id}", (TicketRepo tRepo, string status, int id) => tRepo.updateTicketStatus(status, id));

app.Run();
