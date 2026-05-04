
using fileUploadDemo.Services;
using fileUploadDemo.Services.IServices;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURE FORM UPLOAD SIZE LIMITS
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
    // FormOptions configuration:
    // - MultipartBodyLengthLimit: Maximum size of multipart body (file uploads)
    // - ValueLengthLimit: Individual form value max length (default 4MB)
    // - MemoryBufferThreshold: Size before buffering to disk (default 64KB)
});

// ADD OPENAPI SUPPORT (Minimal API OpenAPI)
builder.Services.AddOpenApi();
// What AddOpenApi() does:
// - Registers OpenAPI document generation services
// - Creates endpoints for OpenAPI JSON specification
// - Required for minimal API with .NET 9+ Scalar/Redoc UI
// - Different from Swagger - uses Microsoft.AspNetCore.OpenApi package

builder.Services.AddScoped<IFileService, FileService>();
// ADD CONTROLLER SUPPORT
builder.Services.AddControllers();
// What AddControllers() does:
// - Registers MVC controller services
// - Enables model binding, validation, formatters
// - Required for [ApiController] and Controller base classes

// ADD API EXPLORER (Required for Swagger)
builder.Services.AddEndpointsApiExplorer();
// What AddEndpointsApiExplorer() does:
// - Adds API explorer service that discovers endpoints
// - Provides metadata about routes, parameters, return types
// - Required for Swagger/Swashbuckle to find your endpoints
// - Without this, Swagger would show no APIs

// ADD SWAGGER GENERATION
builder.Services.AddSwaggerGen();
// What AddSwaggerGen() does:
// - Configures Swashbuckle (Swagger for .NET)
// - Scans controllers and generates Swagger document
// - Reads XML comments if configured
// - Creates /swagger/v1/swagger.json endpoint
// - Adds schema generation for your DTOs

var app = builder.Build();

// ENABLE SWAGGER MIDDLEWARE (Generates JSON)
app.UseSwagger();
// What UseSwagger() does:
// - Adds middleware to serve Swagger JSON document
// - Default endpoint: /swagger/v1/swagger.json
// - Can be customized with options (RouteTemplate, SerializerSettings)
// - Must be called before UseSwaggerUI
// - Works even in production (but usually conditionally enabled)

// ENABLE SWAGGER UI (Interactive Web Interface)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Upload V1");
    // - Configures the Swagger UI web interface
    // - Tells UI where to find the JSON document
    // - "Upload V1" is the display name
    // - You can add multiple endpoints for different API versions
    
    // c.RoutePrefix = "swagger"; // Default path (can change to "" for root)
    // c.DocumentTitle = "File Upload API"; // Custom browser tab title
    // c.DefaultModelsExpandDepth = 0; // Hide schemas section
    // c.EnableTryItOutByDefault(); // Auto-enable "Try it out" button
});
// What UseSwaggerUI() does:
// - Serves HTML/CSS/JS for interactive Swagger UI
// - Provides "Try it out" button for testing APIs
// - Available at /swagger index page
// - Reads your Swagger JSON and generates UI

app.UseStaticFiles();
// What UseStaticFiles() does:
// - Enables serving static files from wwwroot folder
// - Can be used to serve uploaded files from a directory
// - Example: https://localhost:7000/uploads/file.jpg
// - Configure with StaticFileOptions for custom paths

// MAP CONTROLLER ROUTES
app.MapControllers();
// What MapControllers() does:
// - Discovers and maps all controller routes
// - Uses attribute routing ([Route], [HttpGet], etc.)
// - Replaces UseEndpoints() in minimal APIs
// - Must be called after UseRouting if used

// ENABLE HTTPS REDIRECTION
app.UseHttpsRedirection();
app.Run();
/*
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Upload V1");
  
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles(); // if serving uploaded files

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
*/
