using Microservicio_ConsultasMedicas.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microservicio_ConsultasMedicas.protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("HospitalConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Add services to the container.


builder.Services.AddGrpc();
//dotnet dev-certs https --trust
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7297, listenOptions =>
    {
        listenOptions.UseHttps(); // ✅ Usa el certificado por defecto o personalizado
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});



//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Error de autenticación: {context.Exception}");
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            Console.WriteLine($"Error de autenticación: {context.Response}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"Token validado: {context.SecurityToken}");
            return Task.CompletedTask;
        }
    };

    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ClockSkew = TimeSpan.Zero
    };

});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.MapGrpcService<PacienteServiceImpl>();
app.MapGrpcService<ConsultasServiceImpl>();

app.MapGet("/", () => "Comunicacion a trav�s de GRPC");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
