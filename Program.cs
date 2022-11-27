using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDB>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddDbContext<TicketDB>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add logging
builder.Logging.AddSimpleConsole();

// App instance
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

app.MapGet("/", () => "Hello World!");

app.Run();
