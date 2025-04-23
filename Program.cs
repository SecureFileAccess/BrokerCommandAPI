using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger/OpenAPI setup

// Register GrpcChannel as a singleton for gRPC communication
builder.Services.AddSingleton<GrpcChannel>(sp => GrpcChannel.ForAddress("http://localhost:5000"));

// Add controllers
builder.Services.AddControllers();

// Customize Kestrel server settings
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(6974);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI for API documentation
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map controllers to endpoints
app.MapControllers();

app.Run();
