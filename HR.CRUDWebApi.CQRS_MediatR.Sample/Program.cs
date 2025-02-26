using FluentValidation.AspNetCore;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Handlers;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Services;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var sqlConnection = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnection));

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWorks, UnitOfWorks>();

// Register Generic Repository (if applicable)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register Specific Repositories (if you have them)
//builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();


builder.Services.Configure<MailSettingsDto>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSingleton<SmtpClient>();
builder.Services.AddTransient<SendWelComeEmailHandler>();
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
