using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Epidemiologia.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Base de Datos (Npgsql para PostgreSQL que usa Render)
builder.Services.AddDbContext<EpidemiologiaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EpidemiologiaContext")
    ?? throw new InvalidOperationException("Connection string 'EpidemiologiaContext' not found.")));

// 2. CONFIGURACIÓN DE CORS (Para que tus compañeros del hospital puedan entrar)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirHospital",
        policy => policy.AllowAnyOrigin() // En producción podrías poner la URL de ellos aquí
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. SWAGGER EN PRODUCCIÓN (Importante para que se vea en Render)
// Quitamos el 'if (app.Environment.IsDevelopment())' para que siempre sea visible
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Epidemiología V1");
    c.RoutePrefix = string.Empty; // Esto hace que el Swagger cargue directo al entrar a la URL
});

// 4. USAR LA POLÍTICA DE CORS (Debe ir antes de MapControllers)
app.UseCors("PermitirHospital");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();