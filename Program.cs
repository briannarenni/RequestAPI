using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDb>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("myConxStr")));

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
