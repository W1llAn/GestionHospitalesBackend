using Grpc.Core;
using Microservicio_Administracion.Data;
using Microsoft.EntityFrameworkCore;
using consultasMedicas;
using Microservicio_Administracion.Administracion;
namespace Microservicio_Administracion.protos
{
    public class ConsultasServiceImpl : ConsultasService.ConsultasServiceBase
    {
        private readonly DataContext _context;
        private readonly AdministracionServiceClient _administracionClient;

        public ConsultasServiceImpl(DataContext context, AdministracionServiceClient administracionClient)
        {
            _context = context;
            _administracionClient = administracionClient;
        }

        public override async Task<Consulta> GetConsultaCedula(ConsultaCedulaRequest request, ServerCallContext context)
        {
            // Buscar el paciente por cédula
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.cedula == request.Cedula);

            if (paciente == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Paciente no encontrado"));
            }

            // Buscar la consulta asociada al paciente
            var consulta = await _context.ConsultasMedicas
                .FirstOrDefaultAsync(c => c.id_paciente == paciente.id_paciente);

            if (consulta == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Consulta no encontrada"));
            }

            // Obtener el médico (empleado) desde el microservicio de administración
            var empleadoRequest = new EmpleadoRequest { IdEmpleado = consulta.id_medico };
            var empleadoResponse = await _administracionClient.GetEmpleadoAsync(empleadoRequest);

            if (empleadoResponse == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Médico no encontrado"));
            }

            // Mapear los datos a la respuesta gRPC
            var consultaResponse = new Consulta
            {
                IdConsultaMedica = consulta.id_consulta_medica,
                Fecha = consulta.fecha,
                Hora = consulta.hora,
                Motivo = consulta.motivo,
                Diagnostico = consulta.diagnostico,
                Tratamiento = consulta.tratamiento,
                Paciente = new PacienteModel
                {
                    IdPaciente = paciente.id_paciente,
                    Nombre = paciente.nombre,
                    Cedula = paciente.cedula,
                    FechaNacimiento = paciente.fecha_nacimiento,
                    Telefono = paciente.telefono,
                    Direccion = paciente.direccion
                },
                Empleado = empleadoResponse // Viene del microservicio de administración
            };

            return consultaResponse;
        }
    }
}