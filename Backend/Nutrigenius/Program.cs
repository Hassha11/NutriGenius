using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nutrigenius.Models;
using Nutrigenius.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<UserContext>();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000")  // Allow requests from this origin
            .AllowAnyMethod()                    // Allow any HTTP method
            .AllowAnyHeader());                  // Allow any HTTP headers
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Enable CORS globally
app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();


namespace Nutrigenius
{
    public partial class Program
    {
        public static void Main()
        {
            var userContext = new Models.UserContext();
            var databaseService = new DatabaseService();
            var userService = new UserService(userContext, databaseService);

            userService.LoadUserId(123); // Example user ID
            Console.WriteLine($"UserId after loading: {userContext.UserId}");
        }
    }
}