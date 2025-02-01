using restaurante_catracho_apirest.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Crear el constructor de la aplicaci�n
var builder = WebApplication.CreateBuilder(args);

// Cargar la configuraci�n desde el archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Obtener la clave secreta desde la configuraci�n
var secretKey = builder.Configuration.GetSection("settings").GetSection("secretkey").ToString();
var keyByte = Encoding.UTF8.GetBytes(secretKey);

// Configurar la autenticaci�n JWT
builder.Services.AddAuthentication(config =>
{
    // Especificar el esquema de autenticaci�n por defecto como JWT Bearer
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(config =>
{
    // No se requiere HTTPS para la metadata del token (solo para entornos de desarrollo)
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;

    // Configuraci�n de validaci�n del token
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validar la firma del emisor
        IssuerSigningKey = new SymmetricSecurityKey(keyByte), // Usar la clave secreta
        ValidateIssuer = false, // No validar el emisor (Issuer)
        ValidateAudience = false // No validar la audiencia (Audience)
    };
});

// Agregar servicios al contenedor de inyecci�n de dependencias
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar servicios de acceso a datos como Singletons
builder.Services.AddSingleton<UsuarioData>();
builder.Services.AddSingleton<PedidosData>();
builder.Services.AddSingleton<ProductoData>();
builder.Services.AddSingleton<DetallePedidosData>();
builder.Services.AddSingleton<PagosData>();
builder.Services.AddSingleton<SeguridadData>();

// Configurar CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin() // Permitir peticiones desde cualquier origen
           .AllowAnyHeader() // Permitir cualquier encabezado
           .AllowAnyMethod(); // Permitir cualquier m�todo HTTP
    });
});

// Construir la aplicaci�n
var app = builder.Build();

// Obtener el entorno actual de ejecuci�n
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar HTTPS en las respuestas
app.UseHttpsRedirection();

// Agregar el middleware de enrutamiento
app.UseRouting();

// Aplicar la pol�tica de CORS configurada anteriormente
app.UseCors("NuevaPolitica");

// Habilitar la autenticaci�n y autorizaci�n en la API
app.UseAuthentication();
app.UseAuthorization();

// Middleware para permitir el rebobinado de la solicitud (por si se necesita leer el cuerpo varias veces)
app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

// Configurar los endpoints del controlador
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Mapear los controladores de la API
});

// Mapear los controladores (redundante, pero v�lido)
app.MapControllers();

// Ejecutar la aplicaci�n
app.Run();