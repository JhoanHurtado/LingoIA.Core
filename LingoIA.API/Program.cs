using LingoIA.API.Sockets;
using LingoIA.Application.Hubs;
using LingoIA.Application.Interfaces;
using LingoIA.Application.Mappings;
using LingoIA.Application.Services;
using LingoIA.Application.Services.LanguageTool;
using LingoIA.Domain.Interfaces;
using LingoIA.Infrastructure.Interfaces;
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

builder.Services.AddScoped<ILingoIAClient>(provider =>
    new LingoIAClient("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", "Frank")); // O usar `IOptions` si prefieres

builder.Services.AddScoped<ILanguagePracticeService, LanguagePracticeService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();


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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LingoIA API v1");
        c.RoutePrefix = "swagger"; // Accede en /swagger
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");
// Crear un scope para obtener el servicio `SocketServer`
using (var scope = app.Services.CreateScope())
{
    var socketServer = scope.ServiceProvider.GetRequiredService<SocketServer>();
    _ = Task.Run(() => socketServer.StartAsync());
}


app.Run();
