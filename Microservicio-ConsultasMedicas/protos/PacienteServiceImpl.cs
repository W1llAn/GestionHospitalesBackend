using Grpc.Core;
using Microservicio_Administracion.Data;
using Microservicio_ConsultasMedicas.Protos;
using Microsoft.EntityFrameworkCore;

namespace Microservicio_Administracion.protos
{
    public class PacienteServiceImpl : PacienteService.PacienteServiceBase
    {
        private readonly DataContext _context;

        public PacienteServiceImpl(DataContext context)
        {
            _context = context;
        }

        public override async Task<GetPacienteResponse> GetPaciente(GetPacienteRequest request, ServerCallContext context)
        {
            Models.Paciente? paciente = await _context.Paciente.FirstOrDefaultAsync(p => p.id_paciente == request.IdPaciente);

            if (paciente == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Paciente no encontrado"));
            }

            PacienteModel pacienteModel = new()
            {
                IdPaciente = paciente.id_paciente,
                Nombre = paciente.nombre,
                Cedula = paciente.cedula,
                FechaNacimiento = paciente.fecha_nacimiento.ToString("yyyy-MM-dd"),
                Telefono = paciente.telefono,
                Direccion = paciente.direccion
            };

            return new GetPacienteResponse { Paciente = pacienteModel };
        }

        public override async Task<CrearPacienteResponse> CrearPaciente(CrearPacienteRequest request, ServerCallContext context)
        {
            if (!DateOnly.TryParse(request.Paciente.FechaNacimiento, out DateOnly fechaNacimiento))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Formato de fecha inválido"));
            }

            var nuevoPaciente = new Paciente
            {
                nombre = request.Paciente.Nombre,
                cedula = request.Paciente.Cedula,
                fecha_nacimiento = fechaNacimiento,
                telefono = request.Paciente.Telefono,
                direccion = request.Paciente.Direccion
            };

            _context.Paciente.Add(nuevoPaciente);
            _ = await _context.SaveChangesAsync();

            PacienteModel pacienteCreado = new()
            {
                IdPaciente = nuevoPaciente.id_paciente,
                Nombre = nuevoPaciente.nombre,
                Cedula = nuevoPaciente.cedula,
                FechaNacimiento = nuevoPaciente.fecha_nacimiento.ToString("yyyy-MM-dd"),
                Telefono = nuevoPaciente.telefono,
                Direccion = nuevoPaciente.direccion
            };

            return new CrearPacienteResponse { Paciente = pacienteCreado };
        }
    }
}
