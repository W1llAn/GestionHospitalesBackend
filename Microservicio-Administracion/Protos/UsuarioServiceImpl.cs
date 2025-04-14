
using Microsoft.EntityFrameworkCore;
using Grpc.Core;
using Microservicio_Administracion.Data;
using Microservicio_Administracion.Protos;
using Microservicio_Administracion.Models;

namespace Microservicio_Administracion.Protos
{
    public class UsuarioServiceImpl : UsuarioService.UsuarioServiceBase
    {
        private readonly AppDbContext _context;

        public UsuarioServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public override async Task<UsuarioLoginRespuesta> ValidarUsuario(UsuarioLogin usuario, ServerCallContext context)
        {
            if (usuario == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "El usuario no puede ser null"));
            }

            var usuarioBuscar = await _context.Usuarios
                .Include(u => u.empleado)
                .Include(u => u.empleado.Centro_Medico)
                .Include(u => u.empleado.Tipo_Empleado)
                .Include(u => u.empleado.Especialidad)
                .FirstOrDefaultAsync(u => u.nombre_usuario == usuario.NombreUsuario && u.contraseña == usuario.Contrasenia);

            if (usuarioBuscar == null)
            {
                return new UsuarioLoginRespuesta
                {
                    EsValido = false
                };
            }

            var UsuarioDTO= new Protos.Usuario
            {
                Contrasenia = usuarioBuscar.contraseña,
                NombreUsuario = usuarioBuscar.nombre_usuario,
                EmpleadoId = usuarioBuscar.empleadoId,
                Id = usuarioBuscar.Id,
                Empleado = new Empleado
                {
                    Id = usuarioBuscar.empleado.Id,
                    Cedula = usuarioBuscar.empleado.cedula,
                    CentroMedicoID = usuarioBuscar.empleado.centro_medicoID,
                    Email = usuarioBuscar.empleado.email,
                    EspecialidadID = usuarioBuscar.empleado.especialidadID,
                    Nombre = usuarioBuscar.empleado.nombre,
                    Salario = usuarioBuscar.empleado.salario,
                    Telefono = usuarioBuscar.empleado.telefono,
                    TipoEmpleadoID = usuarioBuscar.empleado.especialidadID,
                    CentroMedico = new Centro_Medico
                    {
                        Id = usuarioBuscar.empleado.Centro_Medico.Id,
                        Ciudad = usuarioBuscar.empleado.Centro_Medico.ciudad,
                        Direccion = usuarioBuscar.empleado.Centro_Medico.direccion,
                        Nombre = usuarioBuscar.empleado.Centro_Medico.nombre
                    },
                    Especialidad = new Especialidad
                    {
                        Id = usuarioBuscar.empleado.Especialidad.Id,
                        Especialidad_ = usuarioBuscar.empleado.Especialidad.especialidad
                    },
                    TipoEmpleado = new Tipo_Empleado
                    {
                        Id = usuarioBuscar.empleado.Tipo_Empleado.Id,
                        Tipo = usuarioBuscar.empleado.Tipo_Empleado.tipo
                    }
                }
            };

            return new UsuarioLoginRespuesta
            {
                EsValido = true,
                Usuario = UsuarioDTO
            };
        }
    }
}
