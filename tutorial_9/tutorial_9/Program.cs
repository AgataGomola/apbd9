using Microsoft.EntityFrameworkCore;
using tutorial_9.Controllers;
using tutorial_9.Data;
using tutorial_9.Repositories;
using tutorial_9.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<Apbd2Context>(
    options => options.UseSqlServer("Name=ConnectionStrings:Default"));
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ITripService, TripService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
