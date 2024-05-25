using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cower.Data;
using Cower.Data.Repositories;
using Cower.Data.Repositories.Implementation;
using Cower.Domain.JWT;
using Cower.Domain.Models.Booking;
using Cower.Service.Services;
using Cower.Service.Services.Implementation;
using Cower.Web;
using Cower.Web.StatusCodeHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin() // Разрешает запросы с любых источников
                   .AllowAnyMethod() // Разрешает все методы HTTP
                   .AllowAnyHeader(); // Разрешает все заголовки
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Cower API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    
    c.OperationFilter<AuthorizeCheckOperationFilter>();
});
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower);
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});
builder.WebHost.UseKestrel(kestrel =>
{
    var pfxFilePath = "certificate.pfx";
    var pfxPassword = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

    kestrel.Listen(IPAddress.Any, 8080, listenOptions => {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        listenOptions.UseHttps(pfxFilePath, pfxPassword);
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
var dbDataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dbDataSourceBuilder.MapEnum<BookingStatus>();
var dbDataSource = dbDataSourceBuilder.Build();
builder.Services.AddDbContext<ApplicationContext>((options) => {
    options.UseNpgsql(dbDataSource);
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICoworkingService, CoworkingService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddSingleton<IJwtService, JwtService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICoworkingRepository, CoworkingRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();

var app = builder.Build();

app.UseExceptionHandler(new ExceptionHandlerOptions 
{
    ExceptionHandler = ServerErrorHandler.Invoke
});

app.UseStatusCodePages(async context =>
{
    switch (context.HttpContext.Response.StatusCode)
    {
        case (int)HttpStatusCode.Unauthorized:
            await UnauthorizedHandler.Invoke(context.HttpContext);
            break;
        case (int)HttpStatusCode.Forbidden:
            await FordbiddenHandler.Invoke(context.HttpContext);
            break;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(); // Добавление использования CORS

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
