using GraphQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Altair;
using GraphQL.Server.Ui.Voyager;
using GraphAPI.Services;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphAPI.Schema;
using GraphQL.SystemTextJson;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IItemService, ItemService>();

            // Add GraphQL services and configure options
            builder.Services.AddGraphQL(builder => builder
                .AddApolloTracing()
                .AddHttpMiddleware<CategorySchema, GraphQLHttpMiddlewareWithLogs<CategorySchema>>()
                .AddWebSocketsHttpMiddleware<CategorySchema>()
                .AddSchema<CategorySchema>(GraphQL.DI.ServiceLifetime.Scoped)
                .AddSystemTextJson()
                .AddWebSockets() // Add required services for web socket support
                .AddGraphTypes(typeof(CategorySchema).Assembly)); // Add all IGraphType implementors in assembly which ChatSchema exists 



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseWebSockets();
            app.UseGraphQLWebSockets<CategorySchema>();
            app.UseGraphQL<CategorySchema, GraphQLHttpMiddlewareWithLogs<CategorySchema>>();
            app.Run();
        }
    }
}