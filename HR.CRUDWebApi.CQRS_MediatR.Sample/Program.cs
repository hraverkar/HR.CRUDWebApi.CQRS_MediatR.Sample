using FluentValidation;
using FluentValidation.AspNetCore;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Services;
using HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnection));
// Register Unit of Work
builder.Services.AddScoped<IUnitOfWorks, UnitOfWorks>();
// Register Generic Repository (if applicable)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.Configure<MailSettingsDto>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<SmtpClient>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

//builder.Services.AddTransient<SendWelComeEmailHandler>();
//var configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
//    .AddEnvironmentVariables()
//    .Build();

//builder.Services.AddSingleton<IConfiguration>(configuration);
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//app.UseRateLimiter();
//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
