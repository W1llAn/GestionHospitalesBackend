using Grpc.Core;
using Microservicio_ConsultasMedicas.Data;
using Microservicio_ConsultasMedicas.Models;
using Microservicio_ConsultasMedicas.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Microservicio_ConsultasMedicas.protos
{
    public class PacienteServiceImpl : PacienteService.PacienteServiceBase
    {
        private readonly DataContext _context;

        public PacienteServiceImpl(DataContext context)
        {
            _context = context;
        }
        [Authorize]
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

        // Create (Existente)
        [Authorize]
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
        [Authorize]
        public override async Task<ActualizarPacienteResponse> ActualizarPaciente(ActualizarPacienteRequest request, ServerCallContext context)
        {
            var paciente = await _context.Paciente.FindAsync(request.Paciente.IdPaciente);

            if (paciente == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Paciente no encontrado"));
            }

            if (!DateOnly.TryParse(request.Paciente.FechaNacimiento, out DateOnly fechaNacimiento))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Formato de fecha inválido"));
            }

            paciente.nombre = request.Paciente.Nombre;
            paciente.cedula = request.Paciente.Cedula;
            paciente.fecha_nacimiento = fechaNacimiento;
            paciente.telefono = request.Paciente.Telefono;
            paciente.direccion = request.Paciente.Direccion;

            await _context.SaveChangesAsync();

            return new ActualizarPacienteResponse
            {
                Paciente = new PacienteModel
                {
                    IdPaciente = paciente.id_paciente,
                    Nombre = paciente.nombre,
                    Cedula = paciente.cedula,
                    FechaNacimiento = paciente.fecha_nacimiento.ToString("yyyy-MM-dd"),
                    Telefono = paciente.telefono,
                    Direccion = paciente.direccion
                }
            };
        }
        [Authorize]
        public override async Task<EliminarPacienteResponse> EliminarPaciente(EliminarPacienteRequest request, ServerCallContext context)
        {
            var paciente = await _context.Paciente.FindAsync(request.IdPaciente);

            if (paciente == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Paciente no encontrado"));
            }

            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();

            return new EliminarPacienteResponse { Success = true };
        }
        [Authorize]
        public override async Task<GetPacienteListaResponse> GetAllPaciente(EmptyResponse request, ServerCallContext context)
        {
            var pacientes = await _context.Paciente.ToListAsync();
            var pacientesLista= new List<PacienteModel>();
            foreach (var paciente in pacientes)
            {
                pacientesLista.Add(new PacienteModel { 
                IdPaciente=paciente.id_paciente,
                Cedula=paciente.cedula,
                Direccion=paciente.direccion,
                FechaNacimiento=paciente.fecha_nacimiento.ToString(),
                Nombre=paciente.nombre,
                Telefono=paciente.telefono
                });
            }
            return new GetPacienteListaResponse
            {
                Pacientes = { pacientesLista }
            };
        }
    }
}