using Microsoft.EntityFrameworkCore;
using DSW_3P_PR01.Data;
using DSW_3P_PR01.Repositories;
using DSW_3P_PR01.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddScoped<IMisionRepository, MisionSQLRepository>();
builder.Services.AddScoped<IMisionService, MisionService>();
//Configuracion de SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection")
    )
);

// Configuracion de JWT
var jwt = builder.Configuration.GetSection("Jwt");
var key = Convert.FromBase64String(
   jwt["Key"] ?? throw new InvalidOperationException(
       "Falta configurar Jwt:Key"));
builder.Services
   .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters =
           new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidIssuer = jwt["Issuer"],
               ValidateAudience = true,
               ValidAudience = jwt["Audience"],
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey =
                   new SymmetricSecurityKey(key)
           };
   });

    //Configuracion de CORS
    builder.Services.AddCors(options =>
{
    options.AddPolicy("QuestLogFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5500",
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //Mapear el swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API de Misiones V1");
    });
}

app.UseCors("QuestLogFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

//ORM
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer
// dotnet add package Microsoft.EntityFrameworkCore.Design
// dotnet tool install --global dotnet-ef --version 10.0.*

//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version "10.0.*"