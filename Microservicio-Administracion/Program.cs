using Microservicio_Administracion.Data;
using Microservicio_Administracion.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//migracion
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // <-- esto aplica las migraciones
    if (!db.Especialidades.Any()){
        db.Especialidades.Add(new Especialidad {Id=1, especialidad="Sin Especialidad"});
        db.SaveChanges();
    }
    if (!db.Tipos_Empleados.Any())
    {
        db.Tipos_Empleados.Add(new Tipo_Empleado { Id = 1, tipo = "Administrador" });
        db.SaveChanges();
    }

    if (!db.Centros_Medicos.Any())
    {
        db.Centros_Medicos.Add(new Centro_Medico { Id = 1, nombre = "Central",ciudad="Quito",direccion="direccion" });
        db.SaveChanges();
    }

    if (!db.Empleados.Any())
    {
        db.Empleados.Add(new Empleado {Id=1, nombre = "admin", cedula = "01020304",especialidadID=1,email="admin@admin.com",tipo_empleadoID=1,telefono="0123456789",centro_medicoID=1 });
        db.SaveChanges();
    }
    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario { Id = 1, nombre_usuario = "root",contraseña="1234",empleadoId=1 });
        db.SaveChanges();
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
