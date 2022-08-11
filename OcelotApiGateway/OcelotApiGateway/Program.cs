using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging => logging.AddConsole());
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
        .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json")
        .AddEnvironmentVariables();
});
// Add services to the container.
var authenticationProviderKey = "TestKey";
Action<JwtBearerOptions> options = o =>
{
    o.Authority = "https://localhost:5051";
    o.Audience = "catalogapi";
    o.TokenValidationParameters.ValidateAudience = false;

    // it's recommended to check the type header to avoid "JWT confusion" attacks
    o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
};
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication()
    .AddJwtBearer(authenticationProviderKey, options);
builder.Services.AddOcelot()
    .AddSingletonDefinedAggregator<ItemAggregator>();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway API", Version = "v1" });
});
builder.Services.AddCacheManager();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger(); 
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseStaticFiles();
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();
app.Run();


