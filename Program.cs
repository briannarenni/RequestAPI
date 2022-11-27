using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<SqlRepo>();

// App instance
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
}

app.UseHttpsRedirection();

app.MapGet("/users", (SqlRepo repo) =>
    repo.getAllPersons(connvalue));

app.Run();
