using RefundApp.Controllers;
using RefundApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RefundApp.PsudoServices;
using RefundApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container before building the app.
builder.Services.AddControllers();
builder.Services.AddHttpClient<GatewayController>();

// Register LoginService here
builder.Services.AddScoped<LoginService>(); // Add LoginService to DI container
builder.Services.AddScoped<RefundService>(); // Add LoginService to DI container

// Register DbContext and use the connection string from the configuration
builder.Services.AddDbContext<RefundAppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()   // Allows any origin
               .AllowAnyMethod()   // Allows any HTTP method (GET, POST, etc.)
               .AllowAnyHeader();  // Allows any header
    });
});

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(600); // 10 hours session
    options.Cookie.HttpOnly = true; // cannot read cookie with javascript
    options.Cookie.IsEssential = true; // GDPR 
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); // Build the app after registering all services

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RefundAppDbContext>();
    context.Database.EnsureCreated();
    //PsudoUserDbService.Instance(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS middleware before any other middleware that handles requests
app.UseCors("AllowAllOrigins"); // Use the CORS policy defined above

// Add session middleware before UseAuthorization
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
