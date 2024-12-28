using RefundApp.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient<GatewayController>();

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

var app = builder.Build();

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