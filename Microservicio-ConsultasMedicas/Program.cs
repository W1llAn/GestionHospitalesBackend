using Microservicio_ConsultasMedicas.Data;
using Microservicio_ConsultasMedicas.protos;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("HospitalConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Add services to the container.


builder.Services.AddGrpc();


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
