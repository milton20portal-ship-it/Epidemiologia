using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Epidemiologia.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Base de Datos (PostgreSQL)
builder.Services.AddDbContext<EpidemiologiaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EpidemiologiaContext")
    ?? throw new InvalidOperationException("Connection string 'EpidemiologiaContext' not found.")));

// 2. CONFIGURACIÓN DE CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirHospital",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. SWAGGER SIEMPRE ACTIVO (Para Render)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Epidemiologia v1");
    c.RoutePrefix = string.Empty; // Swagger será la página de inicio
});

// 4. MIDDLEWARES
app.UseCors("PermitirHospital");

// Comentamos redirección HTTPS porque Render ya lo gestiona y evita errores de puertos
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();