using Microsoft.EntityFrameworkCore;
using REST_Api_Application;
using REST_Api_Application.Service;
using REST_API_Application.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog(logger);


// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();


var connectionString = builder.Configuration.GetConnectionString("DBConnectionString");
builder.Services.AddDbContext<StudentContext>(o =>
{
    o.UseSqlServer(connectionString);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestResponseLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();