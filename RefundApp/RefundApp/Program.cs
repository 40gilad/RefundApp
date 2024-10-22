using RefundApp.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient<GatewayController>();


// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(600); //10 hours session
    options.Cookie.HttpOnly = true; // cannot read cookie with javascript
    options.Cookie.IsEssential = true; //GDPR 
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

// Add session middleware before UseAuthorization
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
