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

app.MapPost("/users/register", (UserRepo uRepo, string username, string password) => uRepo.validateRegister(username, password));

app.MapPost("/users/login", (UserRepo uRepo, string username, string password) => uRepo.validateLogin(username, password));

app.MapPost("/users/{user}/details", (UserRepo uRepo, string username) => uRepo.getUserInfo(username));

app.MapPatch("/users/{user}/update-role", (UserRepo uRepo, int userId) => uRepo.changeRole(userId));

app.MapPatch("/users/{user}/update-password", (UserRepo uRepo, string username, string pw1, string pw2) =>
{
    pw1.Trim();
    pw2.Trim();
    uRepo.updatePassword(username, pw1, pw2);
});

app.MapPost("/tickets/{user}", (TicketRepo tRepo, int userId) =>
{
    List<Ticket>? response = tRepo.getUserTickets(userId);
    return (response.Count >= 1) ? Results.Ok(response) : Results.NotFound("Error: No tickets found. Please check that the user ID is valid.");
});

app.MapPost("/tickets/submit", (TicketRepo tRepo, int userId, string username, double amount, string category) =>
{
    Ticket newTicket = new Ticket(userId, username, amount, category);
    tRepo.addTicket(newTicket);
});

app.MapGet("/employees", (UserRepo uRepo) => uRepo.getEmployees());

app.MapGet("/tickets/all", (TicketRepo tRepo) => tRepo.getAllTickets());

app.MapGet("/tickets/open", (TicketRepo tRepo) => tRepo.getPendingTickets());

app.MapPost("/tickets/{ticket}", (TicketRepo tRepo, int ticketId) =>
{
    Ticket? response = tRepo.getTicketById(ticketId);
    return (response == null) ? Results.NotFound("Error: Invalid ticket id.") : Results.Ok(response);
});

app.MapPatch("/tickets/{ticket}", (TicketRepo tRepo, int ticketId, string status) => tRepo.updateTicketStatus(ticketId, status));

app.Run();
