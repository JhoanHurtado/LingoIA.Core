using LingoIA.Application.Hubs;
using LingoIA.Application.Interfaces;
using LingoIA.Application.Mappings;
using LingoIA.Application.Services;
using LingoIA.Application.Services.ContractsServices;
using LingoIA.Infrastructure.Persistence;
using LingoIA.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LingoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddScoped<ILingoIAClient, LingoIAClient>();

builder.Services.AddScoped<ILanguagePracticeService, LanguagePracticeService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthContract, AuthService>();
builder.Services.AddScoped<IConversationService, ConversationService>();


// 📌 Registrar el servidor de sockets
builder.Services.AddSingleton<SocketServer>();

builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddControllers();

// Agregar servicios de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LingoIA API",
        Version = "v1",
        Description = "API para la aplicación LingoIA"
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LingoIA API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");
using (var scope = app.Services.CreateScope())
{
    var socketServer = scope.ServiceProvider.GetRequiredService<SocketServer>();
    _ = Task.Run(() => socketServer.StartAsync());
}


app.Run();
