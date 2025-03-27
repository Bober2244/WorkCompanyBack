using MegaProject.Repository;
using MegaProject.Repository.Extentions;
using MegaProject.Services.Extensions;
using MegaProject.Services.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MegaProject API",
        Version = "v1",
        Description = "API documentation for MegaProject"
    });
});

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddScoped<JwtService>();


var (tokenValidationParameters, secureKey) = TokenValidationParametersFactory.Create(builder.Configuration);
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddSingleton(secureKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("BrigadeControllerPolicy", policy => policy.RequireRole("BrigadeController"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MegaProject API v1");
    });
}


app.UseCors(options => options
    .WithOrigins(new[] { "http://localhost:8080", "http://localhost:8081" })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();