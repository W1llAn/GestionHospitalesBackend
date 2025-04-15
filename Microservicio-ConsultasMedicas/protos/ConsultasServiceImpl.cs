using Grpc.Core;
using Grpc.Net.Client;
using Microservicio_Administracion.Data;
using Microsoft.EntityFrameworkCore;
using ConsultasMedicas.Protos;
using Microservicio_Administracion.Administracion;
using Microservicio_Administracion.Models;
using Microservicio_ConsultasMedicas.Protos;


namespace Microservicio_Administracion.protos
{
    public class ConsultasServiceImpl :ConsultasService.ConsultasServiceBase
    {
        private readonly DataContext _context;

        private readonly IConfiguration _config;

        public ConsultasServiceImpl(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public override async Task<Consulta> GetConsultaCedula(ConsultaCedulaRequest request, ServerCallContext context)
        {
            // Buscar el paciente por cédula
            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(p => p.cedula == request.Cedula);

            if (paciente == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Paciente no encontrado"));
            }

            // Buscar la consulta asociada al paciente
            var consulta = await _context.ConsultasMedicas
                .FirstOrDefaultAsync(c => c.paciente.id_paciente == paciente.id_paciente);

            if (consulta == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Consulta no encontrada"));
            }

            // Obtener el médico (empleado) desde el microservicio de administración
            var empleadoRequest = new EmpleadoGet { Id = consulta.id_empleado };
            using var canal = GrpcChannel.ForAddress(_config["grcp:administracion"]);
            var cliente = new AdministracionService.AdministracionServiceClient(canal);
            var empleadoResponse = await cliente.GetEmpleado(empleadoRequest);

            if (empleadoResponse == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Médico no encontrado"));
            }

            // Mapear los datos a la respuesta gRPC
            var consultaResponse = new Consulta
            {
                // IdConsultaMedica = consulta.id_consulta_medica,
                Fecha = consulta.fecha.ToString("yyyy-MM-dd"),
                Hora = consulta.hora,
                Motivo = consulta.motivo,
                Diagnostico = consulta.diagnostico,
                Tratamiento = consulta.tratamiento,
                Paciente = new PacienteModel
                {
                    IdPaciente = paciente.id_paciente,
                    Nombre = paciente.nombre,
                    Cedula = paciente.cedula,
                    FechaNacimiento = paciente.fecha_nacimiento.ToString("yyyy-MM-dd"),
                    Telefono = paciente.telefono,
                    Direccion = paciente.direccion
                },
                Empleado = empleadoResponse
            };

            return consultaResponse;
        }
    }
}