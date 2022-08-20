using System.Reflection;
using CatalogService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5051";
        options.Audience = "catalogapi";
        // audience is optional, make sure you read the following paragraphs
        // to understand your options
        options.TokenValidationParameters.ValidateAudience = false;

        // it's recommended to check the type header to avoid "JWT confusion" attacks
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    { 
        Type = SecuritySchemeType.OAuth2,        
        Flows = new OpenApiOAuthFlows() { 
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:5051/connect/authorize"),
                TokenUrl = new Uri("https://localhost:5051/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "catalogapi", "Catalog Api - full access" }
                }
            }
        }
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>(); // Required to use access token
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientSecret("secret");
        options.OAuthClientId("catalog_api_swagger");
        options.OAuthAppName("Catalog API - Swagger"); // presentation purposes only
        options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMigrationsEndPoint();

app.MapControllers();

app.Run();
